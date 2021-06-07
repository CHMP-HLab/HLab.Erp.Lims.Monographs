using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour CreateItemView.xaml
    /// </summary>
    public partial class CreateItemView : UserControl, IView<ViewModeList, CreateItemViewModel>
    {
        public CreateItemView()
        {
            InitializeComponent();
        }
    }
}
