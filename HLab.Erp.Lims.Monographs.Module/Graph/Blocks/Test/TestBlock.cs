using System.Windows.Media;
using HLab.ColorTools.Wpf;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Solution;
using HLab.Mvvm.Flowchart.Models;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Test
{
    using H = H<AssayBlock>;

    internal class AssayBlock : GraphBlock
    {
        public AssayBlock(Injector i) : base(i)
        {
            MainLeftGroup.GetOrAddPin<InputPinUnitTest>("in");

            H.Initialize(this);
        }

        class InputPinUnitTest : InputPinUnit
        {
            [TriggerOn]
            public override double Value => base.Value;

            public override double GetValue(int n) => n;
        }
        public MonographTest MonographTest
        {
            get => _monographAssay.Get();
            set => _monographAssay.Set(value);
        }

        readonly IProperty<MonographTest> _monographAssay = H.Property<MonographTest>();


        public override string Caption => _caption.Get();

        readonly IProperty<string> _caption = H.Property<string>(c => c
            .On(self => self.MonographTest.TestClass.Name)
            .Set(self => self.MonographTest.TestClass.Name)
        );


        public override string IconName => _iconName.Get();

        readonly IProperty<string> _iconName = H.Property<string>(c => c
            .On(self => self.MonographTest.TestClass.IconPath)
            .Set(self => self.MonographTest.TestClass.IconPath)
        );

        public override Color Color => _color.Get();

        readonly IProperty<Color> _color = H.Property<Color>(c => c
            .On(self => self.MonographTest.TestClass.Color)
            .Set(self => self.MonographTest.TestClass.Color.ToColor().AdjustBrightness(-0.5))
        );
    }
}
