using System.Windows;
using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs
{
    /// <summary>
    /// Logique d'interaction pour MonographieView.xaml
    /// </summary>
    public partial class MonographView : UserControl, IViewClassDocument,
        IView<ViewModeDefault,MonographViewModel>
    {
        public MonographView()
        {
            InitializeComponent();
        }

        void ConsommableMonographieView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public string ContentId => GetType().Name;
    }
}
