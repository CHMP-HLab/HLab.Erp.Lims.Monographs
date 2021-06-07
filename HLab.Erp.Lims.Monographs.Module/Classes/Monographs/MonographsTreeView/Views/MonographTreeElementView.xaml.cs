using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour MonographieTreeView.xaml
    /// </summary>
    public partial class MonographTreeElementView : UserControl,
        IView<ViewModeContent, MonographTreeElementViewModel>
    {
        public MonographTreeElementView()
        {
            InitializeComponent();
        }
    }
}
