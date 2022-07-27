using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<MonographTest>;
    public partial class MonographTest : Entity
    {
        public MonographTest() => H.Initialize(this);

        public int? MonographId
        {
            get => _monograph.Id.Get();
            set => _monograph.Id.Set(value);
        }

        [Ignore]
        public Monograph Monograph
        {
            get => _monograph.Get();
            set => _monograph.Set(value);
        }

        readonly IForeign<Monograph> _monograph = H.Foreign<Monograph>(); 

        public string Name
        {
            get => _name.Get();
            set => _name.Set(value);
        }

        readonly IProperty<string> _name = H.Property<string>(c => c.Default(""));


        public string Description
        {
            get => _description.Get();
            set => _description.Set(value);
        }

        readonly IProperty<string> _description = H.Property<string>(c => c.Default(""));


        public string Values
        {
            get => _values.Get();
            set => _values.Set(value);
        }

        readonly IProperty<string> _values = H.Property<string>(c => c.Default(""));


        public string Specifications
        {
            get => _specifications.Get();
            set => _specifications.Set(value);
        }

        readonly IProperty<string> _specifications = H.Property<string>(c => c.Default(""));


        public string Comment
        {
            get => _comment.Get();
            set => _comment.Set(value);
        }

        readonly IProperty<string> _comment = H.Property<string>(c => c.Default(""));


        public int? Order
        {
            get => _order.Get();
            set => _order.Set(value);
        }

        readonly IProperty<int?> _order = H.Property<int?>(c => c.Default((int?)null));



        public int? TestClassId
        {
            get => _assayClassId.Get();
            set => _assayClassId.Set(value);
        }

        readonly IProperty<int?> _assayClassId = H.Property<int?>();

        [Ignore]
        public TestClass TestClass
        {
            get => _assayClass.Get();
            set => TestClassId = value.Id;
        }

        readonly IProperty<TestClass> _assayClass = H.Property<TestClass>(c => c.Foreign(e => e.TestClassId));


        [Ignore]
        public string UnitGroup => "u";

        [Ignore]
        public string RightUnitGroup => null;
        //public override ObservableQuery<Unit> UniteList
        //    => N.Get(() => new ObservableQuery<Unit>().AddFilter(u => u.Group == UnitGroup).FluentUpdate());
    }
}
