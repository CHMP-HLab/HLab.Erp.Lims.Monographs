using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<ConsumableType>;
     public class ConsumableType : Entity
     {
         public ConsumableType() => H.Initialize(this);

        public string Name
        {
            get => _name.Get(); set => _name.Set(value);
        }

        private readonly IProperty<string> _name = H.Property<string>(c => c.Default(""));

        public int? Order
        {
            get => _order.Get(); set => _order.Set(value);
        }

        private readonly IProperty<int?> _order = H.Property<int?>(c => c.Default((int?)null));


        public int Color
        {
            get => _color.Get(); set => _color.Set(value);
        }

        private readonly IProperty<int> _color = H.Property<int>(c => c.Default(0));

        public string IconPath
        {
            get => _iconPath.Get(); set => _iconPath.Set(value);
        }

        private readonly IProperty<string> _iconPath = H.Property<string>(c => c.Default(""));

    }
}
