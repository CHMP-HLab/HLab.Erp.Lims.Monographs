using System.Linq;
using System.Windows.Media;
using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Solution
{
    public class InputPinWeight : InputPin
    {
        public override GraphValueType ValueType => ValueTypes.Weight;
    }
    public class InputPinVolume : InputPin
    {
        public override GraphValueType ValueType => ValueTypes.Volume;
    }
    public class InputPinUnit : InputPin
    {
        public override GraphValueType ValueType => ValueTypes.Unit;
    }



    class SolutionBlock : GraphBlock, IToolGraphBlock
    {
        class SoluteWeightPin : InputPinWeight
        {
            public override double GetValue(int n)
            {
                return ((SolutionBlock)Group.Block).Output.GetValue(n) * Weight.Value / Volume.Value;

            }

            public WeightValue Weight { get; } = new WeightValue();
            public VolumeValue Volume { get; } = new VolumeValue(); 
        }

        class SoluteVolumePin : InputPinVolume
        {
            public override double GetValue(int n)
            {
                return ((SolutionBlock)Group.Block).Output.GetValue(n) * VolumeA.Value / VolumeB.Value;

            }

            public VolumeValue VolumeA { get; } = new VolumeValue();
            public VolumeValue VolumeB { get; } = new VolumeValue();
        }

        class SolvantPin : InputPinVolume
        {
            public override double GetValue(int n)
            {
                var block = ((SolutionBlock) Group.Block);
                return block.Output.GetValue(n) - block.GetSolvantVolume(n);

            }
        }

        public SolutionBlock()
        {
            WeightSoluteGroup = GetOrAddGroup("SLTW", PinLocation.Left,"Soluté");
            VolumeSoluteGroup = GetOrAddGroup("SLTV", PinLocation.Left,null);
            SolvantGroup = GetOrAddGroup("SLV", PinLocation.Left, "Solvant");

            Output = MainRightGroup.GetOrAddPin<OutputPin>("out",ValueTypes.Volume);
            Solvant = SolvantGroup.GetOrAddPin<SolvantPin>("slv", ValueTypes.Volume);

            H<SolutionBlock>.Initialize(this);
        }

        public IPinGroup WeightSoluteGroup { get; }
        public IPinGroup VolumeSoluteGroup { get; }
        public IPinGroup SolvantGroup { get; }

        public IPin Output { get; }
        public IPin Solvant { get; }

        public double GetSolvantVolume(int n) => VolumeSoluteGroup.Pins.Sum(p => p.GetValue(n) - Output.GetValue(n));


        public override string Caption => "Solution";
        public override Color Color => Colors.DarkOrange;

        public override string IconName => "Blocks/Dissolution";

        public override bool AskForInputPin(GraphValueType type)
        {
            if (type == ValueTypes.Volume)
            {
                if (TempPins.Any(p => p is SoluteVolumePin)) return false;
                TempPins.Add(VolumeSoluteGroup.GetOrAddPin<SoluteVolumePin>());
                return true;
            }

            if (type == ValueTypes.Weight)
            {
                if (TempPins.Any(p => p is SoluteVolumePin)) return false;
                TempPins.Add(WeightSoluteGroup.GetOrAddPin<SoluteWeightPin>());
                return true;
            }

            return false;
        }
    }
}
