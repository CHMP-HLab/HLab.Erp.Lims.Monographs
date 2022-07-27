using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Media;
using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Volumes
{
    using H = H<VolumesBlock>;

    [DataContract]
    public class VolumesBlock : GraphBlock, IToolGraphBlock
    {
        public VolumesBlock(Injector i) : base(i)
        {
            H.Initialize(this);
            Output = MainRightGroup.GetOrAddPin<OutputPin>("out",ValueTypes.Volume);
        }

        public OutputPin Output { get; private set;}

        public double NbVolumes => _nbVolumes.Get();

        readonly IProperty<double> _nbVolumes = H.Property<double>(c => c
            .On(self => ((InputPinVolumes)self.MainLeftGroup.Pins.Item()).Volumes)
            .Set(self => self.MainLeftGroup.Pins.Sum(p => ((InputPinVolumes)p).Volumes)));


        public override bool AskForInputPin(GraphValueType type)
        {
            if (type != ValueTypes.Volume) return false;
            if (TempPins.Any(p => p is InputPinVolumes)) return false;

            TempPins.Add(MainLeftGroup.GetOrAddPin<InputPinVolumes>());

            return true;
        }

        public override string Caption => "Volumes";
        public override Color Color => Colors.Green;

        public override string IconName => "Blocks/Volumes";
    }
}
