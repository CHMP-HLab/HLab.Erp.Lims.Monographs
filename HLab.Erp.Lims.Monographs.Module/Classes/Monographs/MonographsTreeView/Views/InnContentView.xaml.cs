using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour DciContentView.xaml
    /// </summary>
    public partial class InnContentView : UserControl, IView<ViewModeContent,InnTreeElementViewModel>
    {
        public InnContentView()
        {
            InitializeComponent();
        }

    }
}
