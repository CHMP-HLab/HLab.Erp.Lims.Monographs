using System.Windows;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Tool;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph
{

    using H = H<NewSolutionViewModel>;
    public class NewSolutionViewModel : NotifierBase, IDropViewModel
    {
        public NewSolutionViewModel()
        {
        }

        public Visibility RightAnchorVisibility => Visibility.Hidden;
        public Visibility LeftAnchorVisibility => Visibility.Hidden;


        public MonographGraphViewModel Root
        {
            get => _root.Get();
            set => _root.Set(value);
        }

        private readonly IProperty<MonographGraphViewModel> _root = H.Property<MonographGraphViewModel>();

        public string NewSolutionName
        {
            get => _newSolutionName.Get(); 
            set => _newSolutionName.Set(value);
        }
        private readonly IProperty<string> _newSolutionName = H.Property<string>(c => c.Default("Nouvelle Solution"));

        //public NotifyCommand AddCommand => this.GetCommand(()=>
        //{
        //    if (string.IsNullOrWhiteSpace(NewSolutionName)) return;
        //    Root.GetOrAddSolution(NewSolutionName);           
        //},()=>true);

        public bool IsSelectable { get => _isSelectable.Get(); set => _isSelectable.Set(value); }
        private readonly IProperty<bool> _isSelectable = H.Property<bool>(c => c.Default(true));

        public bool Selected { get => _selected.Get(); set => _selected.Set(value); }
        private readonly IProperty<bool> _selected = H.Property<bool>(c => c.Default(true));

        public bool DragEnter(ErpDragDrop e)
        {
            if (e.DraggedElement is DragConsumableItemView)
            {
                if (e.DraggedElement.DataContext is ConsumableDragDropViewModel c)
                {
                    Selected = true;
                    return true;
                }

                return false;
            }
            return false;
        }

        public bool DragLeave(ErpDragDrop data)
        {
            Selected=false;
            return true;
        }


        public void DropOut(ErpDragDrop e)
        {
            IsSelectable = true;
        }

        public bool DropIn(ErpDragDrop e)
        {
            if (e.DraggedElement != null)
            {
                    var item = e.DraggedElement.DataContext as ConsumableDragDropViewModel;
                    e.DraggedElement = null;

                    if (item != null)
                    {
                        var consommable = item.Model;
                        //Root.GetOrAddLink(consommable, NewSolutionName);

                        

                        //var vm = Root.SolutionsViewModels.OfType<SolutionGraphViewModel>().FirstOrDefault(s => s.Model.Designation == NewSolutionName);

                        //if (vm != null)
                        //    vm.State.Selected = true;
                    }
                return true;
            }
            return false;
        }

        public bool DragStart(ErpDragDrop e)
        {
            if (e.DraggedElement is DragConsumableItemView)
            {
                var c = e.DraggedElement.DataContext as ConsumableDragDropViewModel;
                if (c == null) return false;

                IsSelectable = true;
                return true;
            }
            return false;
        }
    }
}
