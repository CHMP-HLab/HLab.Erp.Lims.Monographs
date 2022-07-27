using System;
using System.Windows;
using HLab.Mvvm;
using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks
{
    using H = H<PinValueViewModel>;

    internal class PinValueViewModel : ViewModel<IPin>
    {
        public PinValueViewModel() => H.Initialize(this);

        public IGraphValue Value1 => _value1.Get();
        readonly IProperty<IGraphValue> _value1 = H.Property<IGraphValue>(c => c.Set(e => e.GetGraphValue()));

        public IGraphValue ValueN => _valueN.Get();
        readonly IProperty<IGraphValue> _valueN = H.Property<IGraphValue>(c => c.Set(e => e.GetGraphValue()));

        [TriggerOn(nameof(Model),"Value")]
        public void Calculate()
        {
            Value1.Value = Model.GetValue(1);
            ValueN.Value = Model.GetValue(2) - Value1.Value;
        }

        public Visibility E1Visibility => _e1Visibility.Get();

        readonly IProperty<Visibility> _e1Visibility = H.Property<Visibility>(c => c
            .On(e => e.Value1.Value)
            .On(e => e.ValueN.Value)
            .Set(e => (Math.Abs(e.Value1.Value - e.ValueN.Value) < double.Epsilon)?Visibility.Collapsed:Visibility.Visible)
        );


        public Visibility EnVisibility => _enVisibility.Get();

        readonly IProperty<Visibility> _enVisibility = H.Property<Visibility>(c => c
            .On(e => e.ValueN.Value)
            .Set(e => Math.Abs(e.ValueN.Value) < double.Epsilon ? Visibility.Collapsed : Visibility.Visible)
        );


        IGraphValue GetGraphValue()
        {
            if(Model.ValueType==ValueTypes.Volume)
            {
                return new VolumeValue();
            }
            if (Model.ValueType == ValueTypes.Weight)
            {
                return new WeightValue();
            }
            if (Model.ValueType == ValueTypes.Unit)
            {
                return new UnitValue();
            }
            return null;
        }
    }
}
