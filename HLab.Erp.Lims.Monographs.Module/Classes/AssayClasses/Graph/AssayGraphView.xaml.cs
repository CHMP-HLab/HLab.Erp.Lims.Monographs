using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Graph;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

namespace HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Graph
{
    /// <summary>
    /// Logique d'interaction pour TestGraphView.xaml
    /// </summary>
    public partial class AssayFlowchartView : UserControl,
        IView<ViewModeDefault,TestGraphViewModel>, IViewClassFlowchart
    {
        public AssayFlowchartView()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Windows.Markup.XamlParseException)
            {
                
            }
        }

        private Dictionary<TextBox,Brush> _background = new Dictionary<TextBox, Brush>();
        private Dictionary<TextBox,Brush> _foreground = new Dictionary<TextBox, Brush>();

        private void TextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var fe = sender as TextBox;
            if (fe != null && !fe.IsReadOnly)
            {
                _background[fe] = fe.Background;

                fe.Background = new SolidColorBrush(Colors.AliceBlue);
                fe.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var fe = sender as TextBox;
            if (fe != null && !fe.IsReadOnly)
            {
                fe.Background = _background[fe];
                fe.Foreground = new SolidColorBrush(Colors.AliceBlue);
            }
        }

    }
}
