using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour ItemTreeView.xaml
    /// </summary>
    public partial class ItemTreeView : UserControl,
        IView<ViewModeTree, IMonographTreeElement>
    {
        public ItemTreeView()
        {
            InitializeComponent();
        }

    }
}
