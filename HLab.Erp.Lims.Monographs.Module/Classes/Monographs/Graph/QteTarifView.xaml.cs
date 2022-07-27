using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    /// <summary>
    /// Logique d'interaction pour QteTarifView.xaml
    /// </summary>
    public partial class QteTarifView : UserControl
    {
        public QteTarifView()
        {
            InitializeComponent();
        }

        void QuantiteTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var fe = sender as TextBox;
            if (fe != null && !fe.IsReadOnly)
            {
                fe.Background = new SolidColorBrush(Colors.AliceBlue);
                fe.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        void QuantiteTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var fe = sender as TextBox;
            if (fe != null && !fe.IsReadOnly)
            {
                fe.Background = new SolidColorBrush(Colors.Transparent);
                fe.Foreground = new SolidColorBrush(Colors.AliceBlue);
            }
        }

    }
}
