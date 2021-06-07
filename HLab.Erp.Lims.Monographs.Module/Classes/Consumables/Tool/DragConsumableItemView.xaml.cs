using System.Windows.Controls;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Core.ToolBoxes;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Tool
{
    /// <summary>
    /// Logique d'interaction pour DragConsommableItemView.xaml
    /// </summary>
    /// 
    public partial class DragConsumableItemView : UserControl, 
        IView<ViewModeDefault, ConsumableDragDropViewModel>
        , IViewClassToolItem
        , IViewClassDraggable
    {
        public DragConsumableItemView()
        {
            InitializeComponent();
        }
    }
}
