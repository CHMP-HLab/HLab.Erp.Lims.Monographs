using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection.Gradients;
using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection
{
    using H = H<InjectionBlock>;

    public class InjectionBlock : GraphBlock, IToolGraphBlock
    {
        public InjectionBlock()
        {
            Samples = GetOrAddGroup("Samples", PinLocation.Left, "Echantillons");
            Channels = GetOrAddGroup("Channels", PinLocation.Left, "Cannaux");

            Output = MainRightGroup.GetOrAddPin<OutputPin>("out", ValueTypes.Unit);

            H.Initialize(this);
        }

        public IPinGroup Samples { get; }
        public IPinGroup Channels { get; }

        class InputPinSample : InputPin
        {
            public InputPinSample() => H<InputPinSample>.Initialize(this);

            public override GraphValueType ValueType => ValueTypes.Volume;

            [TriggerOn]
            public override double Value => base.Value;

            public override double GetValue(int n)
            {
                return n * PerInjectionVolume.Value * NbInjections.Value; //Todo
            }

            public VolumeValue PerInjectionVolume { get; } = new VolumeValue();// => N.Get(() => );
            public UnitValue NbInjections { get; } = new UnitValue();
        }
        class InputPinChannel : InputPin
        {
            public InputPinChannel() => H<InputPinChannel>.Initialize(this);

            public override GraphValueType ValueType => ValueTypes.Volume;

            [TriggerOn]
            public override double Value => base.Value;

            public override double GetValue(int n)
            {
                return ((InjectionBlock) Group.Block).Output.GetValue(n) * Channel.Ratio; //Todo
            }

            public GradientChannel Channel => _channel.Get();
            private readonly IProperty<GradientChannel> _channel = H<InputPinChannel>.Property<GradientChannel>(c => c
                .On(self => ((InjectionBlock)self.Group.Block).Gradient)
                .On(self => self.Caption)
                .Set(
                    self => ((InjectionBlock)self.Group.Block).Gradient.GetOrAddChannel(self.Caption)
                ));

        }

        public ObservableCollection<double> Time { get; } = new ObservableCollection<double>(); // => N.Get(() => new ObservableCollection<double>());

        public override bool AskForInputPin(GraphValueType type)
        {
            if (type != ValueTypes.Volume) return false;
            if (TempPins.Any(p => p is InputPinChannel)) return false;

            TempPins.Add(Samples.GetOrAddPin<InputPinSample>());
            TempPins.Add(Channels.GetOrAddPin<InputPinChannel>());


            return true;
        }

        public OutputPin Output { get; }

        public override string Caption => "Injection";

        public override Color Color => Colors.Blue;

        public override string IconName => "Blocks/Gradient";
        public Gradient Gradient { get; } = new Gradient(); // => N.Get(() => );

    }
}
