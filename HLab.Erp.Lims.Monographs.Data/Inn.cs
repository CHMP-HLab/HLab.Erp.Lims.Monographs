using HLab.Erp.Core;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<Inn>;
    public class Inn : Entity, IListableModel
    {
        public Inn() => H.Initialize(this);

         public string Name
        {
            get => _name.Get();
            set => _name.Set(value);
        }

        private readonly IProperty<string> _name = H.Property<string>(c => c.Default(""));


        [TriggerOn(nameof(Name)), Ignore]
        public string Caption => Name;

        [Ignore]
        public string IconPath => "IconMolecule";

    }
}
