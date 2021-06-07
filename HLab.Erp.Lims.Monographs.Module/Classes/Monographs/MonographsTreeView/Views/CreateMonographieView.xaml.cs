using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour CreateMonographie.xaml
    /// </summary>
    public partial class CreateMonographieView : UserControl, IView<ViewModeDefault,CreateMonographViewModel>
    {
        public CreateMonographieView()
        {
            InitializeComponent();
        }
    }
}
