using System.Windows.Controls;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.HistoricalTools;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.HistoricalTools
{
    /// <summary>
    /// Logique d'interaction pour TestEchantillonDragDropView.xaml
    /// </summary>
    public partial class SampleAssayDragDropView : UserControl,
        IView<ViewModeDraggable, SampleAssayDragDropViewModel>,
        IView<ViewModeList, SampleAssayDragDropViewModel>
    {
        public SampleAssayDragDropView()
        {
            InitializeComponent();
        }
    }
}
