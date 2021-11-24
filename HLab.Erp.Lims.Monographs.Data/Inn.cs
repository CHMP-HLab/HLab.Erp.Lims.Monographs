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


        [Ignore]
        public string Caption => _caption.Get();
        private readonly IProperty<string> _caption = H.Property<string>(c => c
            .On(e => e.Name)
            .Set(e => string.IsNullOrWhiteSpace(e.Name)?"{New INN}":$"{{INN}}\n{e.Name}")
        );

        [Ignore]
        public string IconPath => "IconMolecule";

    }
}
