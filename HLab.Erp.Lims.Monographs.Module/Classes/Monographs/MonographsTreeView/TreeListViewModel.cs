using System.Linq;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<MonographsListViewModel>;

    public class MonographsListViewModel : ViewModel, IMonographTreeElement
    {
        public class Bootloader : NestedBootloader
        {
            public override bool Allowed => Erp.Acl.IsGranted(AclRights.BetaTest);
            public override string MenuPath => "param";
        }

        public MonographsListViewModel() => H.Initialize(this);

        public string Title => "{Monographs}";
        public string ContentId => "monographs";

        public IMonographTreeElement Parent { get; set; } = null;
        public MonographsListViewModel Root => this;


        public IObservableFilter<Inn> Children { get; } = H.Filter<Inn>(c => c
              .AddFilter((s, e) => s.Monographs.Select(m => m.InnId).Contains(e.Id))
              .Link(e => e.InnSource)
        );


        public string SearchText
        {
            get => _searchText.Get();
            set => _searchText.Set(value);
        }
        private readonly IProperty<string> _searchText = H.Property<string>(c => c.Default(""));


        [TriggerOn(nameof(SearchText))]
        [TriggerOn(nameof(HasMonographsOnly))]
        private void SetSearch()
        {
            Children.AddFilter(e => e.Name != null && e.Name.Contains(SearchText), 0, "Search");
            Children.OnTriggered();
        }

        public bool HasMonographsOnly
        {
            get => _hasMonographsOnly.Get();
            set => _hasMonographsOnly.Set(value);
        }
        private readonly IProperty<bool> _hasMonographsOnly = H.Property<bool>(c => c.Default(true));


        public ObservableQuery<Monograph> Monographs
        {
            get => _monographs.Get();
            set => _monographs.Set(value.FluentUpdate());
        }
        private readonly IProperty<ObservableQuery<Monograph>> _monographs = H.Property<ObservableQuery<Monograph>>();


        public ObservableQuery<Form> FormSource
        {
            get => _formSource.Get();
            private set => _formSource.Set(value.FluentUpdate());
        }
        private readonly IProperty<ObservableQuery<Form>> _formSource = H.Property<ObservableQuery<Form>>();

        public ObservableQuery<Pharmacopoeia> PharmacopoeiaSource
        {
            get => _pharmacopoeiaSource.Get();
            private set => _pharmacopoeiaSource.Set(value.FluentUpdate());
        }
        private readonly IProperty<ObservableQuery<Pharmacopoeia>> _pharmacopoeiaSource = H.Property<ObservableQuery<Pharmacopoeia>>();

        public ObservableQuery<Inn> InnSource
        {
            get => _innSource.Get();
            private set
            {
                value.UpdateAsync(() =>
                {
                    if (_innSource.Set(value))
                        Children.OnTriggered();
                });
            }
        }
        private readonly IProperty<ObservableQuery<Inn>> _innSource = H.Property<ObservableQuery<Inn>>();

        public Inn Inn => null;

        public Form Form => null;

        public Pharmacopoeia Pharmacopoeia => null;

        public string Version => null;
    }
}