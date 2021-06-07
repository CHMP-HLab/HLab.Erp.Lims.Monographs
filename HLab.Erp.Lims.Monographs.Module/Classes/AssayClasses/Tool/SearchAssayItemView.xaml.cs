using System.Windows.Controls;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Core.ToolBoxes;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.HistoricalTools;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Tool;
using HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.HistoricalTools;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Tool
{
    /// <summary>
    /// Logique d'interaction pour SearchTestItemView.xaml
    /// </summary>
    public partial class SearchAssayItemView : UserControl,
        IView<ViewModeDefault, AssayDragDropViewModel>,
        IView<ViewModeDefault, TestClassDragDropViewModel>
        , IViewClassToolItem
        , IViewClassDraggable
    {
        public SearchAssayItemView()
        {
            InitializeComponent();
        }
    }
}
