using System.Linq;
using Google.Protobuf.WellKnownTypes;
using HLab.Base.Fluent;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
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
            readonly IAclService _acl;

            public Bootloader(IAclService acl)
            {
                _acl = acl;
            }
            public override bool Allowed => _acl.IsGranted(AclRights.BetaTest);
            public override string MenuPath => "param";
        }

        public MonographsListViewModel(
            ObservableQuery<Monograph> monographs,
            ObservableQuery<Form> formSource,
            ObservableQuery<Pharmacopoeia> pharmacopoeiaSource,
            ObservableQuery<Inn> innSource
            )
        {
            Monographs = monographs;
            FormSource = formSource;
            PharmacopoeiaSource = pharmacopoeiaSource;
            InnSource = innSource;

            H.Initialize(this);

            InnSource.Start();
            Monographs.Start();
            FormSource.Start();
            PharmacopoeiaSource.Start();
        }

        public string Title => "{Monographs}";
        public string ContentId => "monographs";

        public IMonographTreeElement Parent { get; set; } = null;
        public MonographsListViewModel Root => this;


        public IObservableFilter<Inn> Children { get; } = H.Filter<Inn>(c => c
              //.AddFilter((s, e) => s.Monographs.Select(m => m.InnId).Contains(e.Id))
              .Link(e => e.InnSource)
        );


        public string SearchText
        {
            get => _searchText.Get();
            set => _searchText.Set(value);
        }

        readonly IProperty<string> _searchText = H.Property<string>(c => c.Default(""));


        readonly ITrigger _searchTrigger = H.Trigger(c => c
            .On(e => e.SearchText)
            .On(e => e.WithMonographsOnly)
            .Do(e => e.SetSearch())
        );

        void SetSearch()
        {
            Children.AddFilter(e => e.Name != null && e.Name.Contains(SearchText), 0, "Search");

            if(WithMonographsOnly)
                Children.AddFilter((e) => Monographs.Select(m => m.InnId).Contains(e.Id), "With");
            else Children.RemoveFilter("With");
            
            Children.OnTriggered();
        }

        public bool WithMonographsOnly
        {
            get => _withMonographsOnly.Get();
            set => _withMonographsOnly.Set(value);
        }

        readonly IProperty<bool> _withMonographsOnly = H.Property<bool>(c => c.Default(true));


        public ObservableQuery<Monograph> Monographs { get; }

        public ObservableQuery<Form> FormSource { get; }

        public ObservableQuery<Pharmacopoeia> PharmacopoeiaSource { get; }

        public ObservableQuery<Inn> InnSource { get; }

        public Inn Inn => null;

        public Form Form => null;

        public Pharmacopoeia Pharmacopoeia => null;

        public string Version => null;
    }
}