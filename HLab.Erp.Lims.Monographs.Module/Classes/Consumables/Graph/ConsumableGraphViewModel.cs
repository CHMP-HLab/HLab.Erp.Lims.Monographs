using System.Windows;
using System.Windows.Input;
using HLab.ColorTools.Wpf;
using HLab.Core.Annotations;
using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.EditorTool;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Graph
{
    using H = H<ConsumableGraphViewModel>;

    public class ConsumableGraphViewModel : GraphViewModel<MonographConsumable>
    {
        private readonly IMessageBus _msg;
        private readonly IDataService _db;
        public ConsumableGraphViewModel(IMessageBus msg, IDataService db, IDialogService dialog)
        {
            _msg = msg;
            _db = db;
            _dialog = dialog;
            _msg.Subscribe<MonographEditorViewModel>(a =>
            {
                if (a.Monograph?.Id == Model.MonographId) Bind(a);
            });
        }

        private void Bind(MonographEditorViewModel vm)
        {
            var span = vm.Bind(Model.AnchorId(), State, "Background");
            if (span != null) span.MouseLeftButtonDown += Span_MouseLeftButtonDown;
        }

        private void Span_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Selected = true;
        }

        public override bool IsConnectable(string thisClass, IGraphViewModel model, string modelClass)
        {
            if (thisClass != "right") return false;

            //if (model is SolutionGraphViewModel solution)
            //    return !Model.RightLinks.Any(e => ReferenceEquals(e.Right, solution.Model));

            return false;
        }

        public override void ConnectTo(string thisClass, IGraphViewModel model, string modelClass)
        {
            if (!IsConnectable(thisClass, model, modelClass)) return;
            //Root.GetOrAddLink(Model, ((SolutionGraphViewModel)model).Model);
            return;
        }


        public Visibility RightAnchorVisibility => Visibility.Visible;
        public Visibility LeftAnchorVisibility => Visibility.Hidden;


        //public override int HorizontalOrder => int.MaxValue;
        //public override int Column => 0;

        public bool IsSelectable { get; } = true;

        public override int Order => Model.Order ?? 0;

        [TriggerOn(nameof(Model), "Consumable","Type","Color")]
        [TriggerOn(nameof(State))]
        public override void SetColor()
        {
            if (State == null) return;
            State.Color = (Model?.Consumable?.Type?.Color ?? 0).ToColor();
        }


        //public Monograph Monograph { get => this.Get<Monograph>(); set => N.Set(value); }
        private readonly IDialogService _dialog;

        public override ICommand DeleteCommand { get; } = H.Command(c => c
//            .CanExecute(e => (e.RightLinks?.Count ?? 0) == 0 && (e.LeftLinks?.Count ?? 0) == 0)
            .Action(
                (e, n) => {            
                    e.DeleteModel(e._dialog,e._db);
                    e.Root.Model.Consumables.FluentUpdate();
                }
                )
            //.On(e => e.RightLinks.Item())
            //.On(e => e.LeftLinks.Item())
            //.CheckCanExecute()
        );



        public string IconPath => _iconName.Get();
        private readonly IProperty<string> _iconName = H.Property<string>(c => c
            .On(e => e.Model.Consumable.Type.IconPath)
            .Set(e => e.Model.Consumable.Type?.IconPath)
        );

 
    }
}
