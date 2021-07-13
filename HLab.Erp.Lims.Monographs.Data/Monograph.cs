using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Notify.PropertyChanged;
using NPoco;


namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<Monograph>;

    public partial class Monograph : Entity
    {
        public Monograph() => H.Initialize(this);

        public int? PharmacopoeiaId
        {
            get => _pharmacopoeiaId.Get();
            set => _pharmacopoeiaId.Set(value);
        }

        private readonly IProperty<int?> _pharmacopoeiaId = H.Property<int?>();

        [Ignore]
        public Pharmacopoeia Pharmacopoeia
        {
            get => _pharmacopoeia.Get();
            set => PharmacopoeiaId = value.Id;
        }

        private readonly IProperty<Pharmacopoeia> _pharmacopoeia = H.Property<Pharmacopoeia>(c => c.Foreign(e => e.PharmacopoeiaId));



        public string PharmacopoeiaVersion
        {
            get => _pharmacopoeiaVersion.Get();
            set => _pharmacopoeiaVersion.Set(value);
        }

        private readonly IProperty<string> _pharmacopoeiaVersion = H.Property<string>(c => c.Default(""));



        public string Reference
        {
            get => _reference.Get();
            set => _reference.Set(value);
        }

        private readonly IProperty<string> _reference = H.Property<string>(c => c.Default(""));


        public byte[] Document
        {
            get => _document.Get();
            set => _document.Set(value);
        }
        private readonly IProperty<byte[]> _document = H.Property<byte[]>(c => c.Default((byte[])null));


        public string DocumentFormat
        {
            get => _documentFormat.Get();
            set => _documentFormat.Set(value);
        }
        private readonly IProperty<string> _documentFormat = H.Property<string>(c => c.Default(""));


        public int? InnId
        {
            get => _innId.Get();
            set => _innId.Set(value);
        }
        private readonly IProperty<int?> _innId = H.Property<int?>(c => c.Default((int?)null));

        [Ignore]

        public Inn Inn
        {
            get => _inn.Get();
            set => InnId = value.Id;
        }
        private readonly IProperty<Inn> _inn = H.Property<Inn>(c => c.Foreign(e => e.InnId));



        public int? FormId
        {
            get => _formId.Get();
            set => _formId.Set(value);
        }

        private readonly IProperty<int?> _formId = H.Property<int?>();

        [Ignore]
        public Form Form
        {
            get => _form.Get();
            set => FormId = value.Id;
        }
        private readonly IProperty<Form> _form = H.Property<Form>(c => c.Foreign(e => e.FormId));



        [Ignore]
        public ObservableQuery<MonographConsumable> Consumables
        {
            get => _consumables.Get();
            set => _consumables.Set(value
                .AddFilter("OneToMany", e => e.MonographId == Id)
            );
        }

        private IProperty<ObservableQuery<MonographConsumable>> _consumables =
            H.Property<ObservableQuery<MonographConsumable>>();


        [Ignore]
        public ObservableQuery<MonographTest> Tests
        {
            get => _tests.Get();
            set => _tests.Set(value
                    .AddFilter("OneToMany", e => e.MonographId == Id)
                    //.FluentUpdate()            
            );
        }
        private readonly IProperty<ObservableQuery<MonographTest>> _tests =
            H.Property<ObservableQuery<MonographTest>>();


        /*       
            ALTER TABLE `limsmonographies`.`monographie` 
            ADD COLUMN `GraphSource` BLOB NULL AFTER `DciId`;
         */
        public string GraphSource
        {
            get => _graphSource.Get();
            set => _graphSource.Set(value);
        }

        private readonly IProperty<string> _graphSource = H.Property<string>(c => c.Default(""));

        [Ignore]
        public string IconPath => "Icons/Monograph";
    }
}
