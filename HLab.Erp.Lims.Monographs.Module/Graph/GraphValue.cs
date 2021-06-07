using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph
{


    public abstract class MonographValue : NotifierBase, IGraphValue
    {
        public double Value
        {
            get => _value.Get();
            set => _value.Set(value);
        }

        private readonly IProperty<double> _value = H<MonographValue>.Property<double>(c => c.Default(0.0));
       
        public abstract GraphValueType Type { get; }
    }

    public class WeightValue : MonographValue
    {
        public override GraphValueType Type => ValueTypes.Weight;
    }
    public class VolumeValue : MonographValue
    {
        public override GraphValueType Type => ValueTypes.Volume;
    }
    public class UnitValue : MonographValue
    {
        public override GraphValueType Type => ValueTypes.Unit;
    }
}