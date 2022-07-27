using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using MSHTML;
using Timer = System.Threading.Timer;

namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetriever
{
    //TODO : Port to cefsharp


    internal static class WebBrowserExt
    {
        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }
        public static void SetSilent(this WebBrowser browser, bool silent=true)
        {
            if (browser == null)
                throw new ArgumentNullException(nameof(browser));

            // get an IWebBrowser2 from the document
            var sp = browser.Document as IOleServiceProvider;
            if (sp == null)
            {
                browser.Navigated += BrowserOnNavigated;
                return;
            }

            var iidIWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
            var iidIWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

            object webBrowser;
            sp.QueryService(ref iidIWebBrowserApp, ref iidIWebBrowser2, out webBrowser);

            if (webBrowser == null)
            {
                browser.Navigated += BrowserOnNavigated;
                    return;
            }

            webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
        }

        static void BrowserOnNavigated(object sender, NavigationEventArgs arg)
        {
            WebBrowser browser = sender as WebBrowser;
            if (browser == null) return;

            browser.SetSilent();
            //browser.Navigated -= BrowserOnNavigated;
        }
    }

    internal class DomParser
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        static extern int UrlMkSetSessionOption(
            int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        const int URLMON_OPTION_USERAGENT = 0x10000001;
        const int URLMON_OPTION_USERAGENT_REFRESH = 0x10000002;

        public void ChangeUserAgent()
        {
            string ua = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:24.0) Gecko/20100101 Firefox/24.0";

            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT_REFRESH, null, 0, 0);
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, ua, ua.Length, 0);
        }

        public string Html = "";

        readonly WebBrowser _browser;

        readonly Timer _timer;

        //public bool SupplierPrice = false;

        public event EventHandler<string> Parsed;

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Navigated += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        public DomParser(WebBrowser browser, Uri url, EventHandler<string> handler)
        {
            Parsed += handler;

            _browser = browser;


            _timer = new Timer(TimerTask);

            ChangeUserAgent();
            //HideScriptErrors(_browser,true);

            

            InternetSetCookie("www.sigmaaldrich.com", "country", "FRA");
            InternetSetCookie("www.sigmaaldrich.com", "cookienotify", "1000");
            InternetSetCookie("www.sigmaaldrich.com", "SialLocaleDef", "CountryCode~FR|WebLang~-2|");

            _browser.LoadCompleted += _browser_LoadCompleted;
            _browser.Navigated += BrowserOnNavigated;

            _timer.Change(10000, 0);

            browser.Navigate(url);

            var java = new Thread(CloseJavaErrors);
            java.Start();
        }

        bool _javaCloseRunning = false;

        void CloseJavaErrors()
        {
            _javaCloseRunning = true;
            while (_javaCloseRunning)
            {
                IntPtr hwnd = FindWindowByCaption(IntPtr.Zero, "Erreur de script");
                while (hwnd != IntPtr.Zero)
                {
                    PostMessage(hwnd, 0x10, IntPtr.Zero, IntPtr.Zero);
                    hwnd = FindWindowByCaption(IntPtr.Zero, "Erreur de script");
                }

                DoEvents();
            }
        }

        void BrowserOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            _browser.Navigated -= BrowserOnNavigated;
            //InjectScript(_browser,"body",DisableScriptError);
        }


        void TimerTask(object stateObj)
        {
            _timer.Dispose();
            Parsed?.Invoke(this, "");
        }
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }

        const string DisableScriptError = @"javascript:window.onerror=function(msg, url, line) { return true; }; void(0);";


        void InjectScript(WebBrowser browser,string tag, string scriptCode)
        {
            //HTMLDocumentClass doc = browser.Document as HTMLDocumentClass;
            HTMLDocument doc = browser.Document as HTMLDocument;


            //Questo crea lo script per la soprressione degli errori
            IHTMLScriptElement script = (IHTMLScriptElement)doc.createElement("SCRIPT");
            script.type = "text/javascript";
            script.text = scriptCode;

            IHTMLElementCollection nodes = doc.getElementsByTagName(tag);

            foreach (IHTMLElement elem in nodes)
            {
                if (tag == "body")
                {
                    HTMLBody body = (HTMLBody)elem;
                    body.appendChild((IHTMLDOMNode)script);                    
                }

                if (tag == "head")
                {
                    HTMLHeadElement head = (HTMLHeadElement)elem;
                    head.appendChild((IHTMLDOMNode)script);
                }
            }
        }


        void _browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //InjectDisableScript(_browser);
            _timer.Dispose();

            //            while (true)
            {
                //
                var d = (_browser?.Document as HTMLDocument);
                if (d == null)
                {
                    Parsed?.Invoke(this, "");
                    return;
                }

                foreach (IHTMLElement ele in d.all)
                {
                    if (ele.id == "pricingContainerMessage")
                    {
                        var html = ele.innerHTML;
                        while (html.StartsWith("<DIV class=priceAvailLoading"))
                        {
                            DoEvents();
                            html = ele.innerHTML;
                        }

                        _javaCloseRunning = false;

                        foreach (IHTMLElement ele2 in (IEnumerable<IHTMLElement>)ele.all)
                        {
                            if (ele2.className == "price" && ele2.tagName == "TD")
                            {
                                html = ele2.innerHTML;
                                foreach (IHTMLElement ele3 in (IEnumerable<IHTMLElement>)ele2.all)
                                {
                                    if (ele3.tagName == "P")
                                    {
                                        html = ele3.innerHTML;
                                        Parsed?.Invoke(this, html);
                                        return;
                                    }
                                }
                            }
                        }
                        Parsed?.Invoke(this, html);
                        return;
                    }
                }
            }
        }
    }
}
