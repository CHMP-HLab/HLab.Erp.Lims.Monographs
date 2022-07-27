using System.Runtime.Serialization;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Solution;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Volumes
{
    using H = H<InputPinVolumes>;

    public class InputPinVolumes : InputPinVolume
    {
        public InputPinVolumes() => H.Initialize(this);

        [DataMember]
        public double Volumes
        {
            get => _volumes.Get();
            set => _volumes.Set(value);
        }

        readonly IProperty<double> _volumes = H.Property<double>(c => c.Default(0.0));


        public double Ratio => _ratio.Get();

        readonly IProperty<double> _ratio = H.Property<double>(c => c
            .On(e => ((VolumesBlock)e.Group.Block).NbVolumes)
            .Set(e => e.Volumes / ((VolumesBlock)e.Group.Block).NbVolumes)
        );


        public override double Value => _value.Get();

        readonly IProperty<double> _value = H.Property<double>(c => c
                .On(e => e.Ratio)
                .On("Group.Block.Output.Value")
                .Set(e => e.Value) // TODO : base.Value
        );


        public override double GetValue(int n)
        {
            return ((VolumesBlock)Group.Block).Output.GetValue(n) * Ratio
                ;
        }
    }
}