using System.Runtime.Serialization;
using System.Windows.Media;
using HLab.ColorTools.Wpf;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Consumable
{
    using H = H<ConsumableBlock>;

    internal class ConsumableBlock : GraphBlock
    {
        public ConsumableBlock(Injector i) : base(i) => H.Initialize(this);

        public MonographConsumable MonographConsumable
        {
            get => _monographConsumable.Get();
            set => _monographConsumable.Set(value);
        }

        readonly IProperty<MonographConsumable> _monographConsumable = H.Property<MonographConsumable>();


        [TriggerOn(nameof(MonographConsumable), "Consumable", "UnitGroup")]
        public void Update()
        {
            switch (MonographConsumable.Consumable.UnitGroup)
            {
                case "v":
                    MainRightGroup.GetOrAddPin<OutputPin>("out", ValueTypes.Volume);
                    break;
                case "m":
                    MainRightGroup.GetOrAddPin<OutputPin>("out", ValueTypes.Weight);
                    break;
                case "u":
                    MainRightGroup.GetOrAddPin<OutputPin>("out", ValueTypes.Unit);
                    break;
            }
        }

        [DataMember]
        public int ConsumableId => MonographConsumable.Consumable.Id;
        
        public override string Caption => _caption.Get();

        readonly IProperty<string> _caption = H.Property<string>(c => c
            .On(self => self.MonographConsumable.Consumable.Name)
            .Set(self => self. MonographConsumable.Consumable.Name)
        );



        public override string IconName => _iconName.Get();

        readonly IProperty<string> _iconName = H.Property<string>(c => c
            .On(self => self.MonographConsumable.Consumable.Type.IconPath)
            .Set(self => self.MonographConsumable.Consumable.Type.IconPath)
        );


        public override Color Color => _color.Get();

        readonly IProperty<Color> _color = H.Property<Color>(c => c
            .On(self => self.MonographConsumable.Consumable.Type.Color)
            .Set(self => self.MonographConsumable.Consumable.Type.Color.ToColor().AdjustBrightness(-0.5)));


        [DataMember]
        public GraphValueType OutputType
        {
            get => _outputType.Get();
            set
            {
                if(_outputType.Set(value))
                    MainRightGroup.GetOrAddPin<OutputPin>("out", OutputType);
            }
        }

        readonly IProperty<GraphValueType> _outputType = H.Property<GraphValueType>();

    }
}
