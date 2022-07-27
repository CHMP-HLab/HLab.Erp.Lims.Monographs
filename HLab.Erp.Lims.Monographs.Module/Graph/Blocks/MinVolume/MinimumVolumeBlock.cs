using System;
using System.Windows.Media;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Solution;
using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.MinVolume
{


    public class MinimumVolumeBlock : GraphBlock, IToolGraphBlock
    {
        public MinimumVolumeBlock(Injector i) : base(i)
        {
            Input = MainLeftGroup.GetOrAddPin<InputPinMinVolume>("in");
            Output = MainRightGroup.GetOrAddPin<OutputPin>("out", ValueTypes.Volume);
        }

        class InputPinMinVolume : InputPinVolume
        {
            [TriggerOn(nameof(Group),"Block","Output","Value")]
            public override double Value => base.Value;

            public override double GetValue(int n)
            {
                var b = ((MinimumVolumeBlock) Group.Block);
                var v = b.Output.GetValue(n);
                var min = b.MinimumVolume.Value;
                return Math.Max(v,min);
            }
        }

        public IPin Input { get; }
        public IPin Output { get; }

        public VolumeValue MinimumVolume { get; } = new VolumeValue();//=> N.Get(() => new VolumeValue());
        public override string Caption => "Min";
        public override Color Color => Colors.BlueViolet;
        public override string IconName => "Blocks/Minimum";
    }
}
