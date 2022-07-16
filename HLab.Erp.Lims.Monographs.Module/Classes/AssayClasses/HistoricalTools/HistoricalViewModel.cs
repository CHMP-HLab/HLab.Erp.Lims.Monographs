using System.Collections.Generic;
using System.Linq;

using HLab.Core.Annotations;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Icons.Annotations.Icons;
using HLab.Mvvm;
using HLab.Mvvm.Application;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.HistoricalTools
{
    using H = H<HistoricalViewModel>;

    public class HistoricalViewModel : ViewModel
    {
        public class Bootloader : NestedBootloader
        {
            public override bool Allowed => Erp.Acl.IsGranted(AclRights.BetaTest);
            public override string MenuPath => "tools";
        }

        private readonly IMessageBus _msg;
        private readonly IIconService _icons;
        private readonly IDataService _db;

        public HistoricalViewModel(IMessageBus msg, IIconService icons, IDataService db)
        {
            _msg = msg;
            _icons = icons;
            _db = db;
            H.Initialize(this);
            _msg.Subscribe<SelectedMessage>(OnSelected);
        }

        public object Icon => _icons.GetIconAsync("IconHistory").Result;
        public string Title => "Historique";

        private void OnSelected(SelectedMessage message)
        {
            if (message.Entity is Monograph m)
            {
                FormId = m.Form?.Id ?? -1;
                Inn = m.Inn?.Name;
                PharmacopoeiaId = m.Pharmacopoeia?.Id??-1;                    
            }
        }

        public int FormId
        {
            get => _formId.Get();
            set => _formId.Set(value);
        }
        private readonly IProperty<int> _formId = H.Property<int>(c => c.Default((int)default));


        public string Inn
        {
            get => _inn.Get();
            set => _inn.Set(value);
        }
        private readonly IProperty<string> _inn = H.Property<string>(c => c.Default((string)default));

        public int PharmacopoeiaId
        {
            get => _pharmacopoeiaId.Get();
            set => _pharmacopoeiaId.Set(value);
        }

        private readonly IProperty<int> _pharmacopoeiaId = H.Property<int>(c => c.Default((int)default));


        [TriggerOn(nameof(Inn))]
        [TriggerOn(nameof(FormId))]
        public List<int> ProductsId => _productsId.Get();
        private readonly IProperty<List<int>> _productsId = H.Property<List<int>>(c => c
            .Set(async e => await e._db.FetchWhereAsync<Product>(f => f.FormId == e.FormId && f.Name == e.Inn).Select(f => f.Id).ToListAsync()));



        public ObservableQuery<Sample> SearchHistoryList
        {
            get => _searchHistoryList.Get();
            set => _searchHistoryList.Set(value.AddFilter("pharmacopee", e => e.PharmacopoeiaId == PharmacopoeiaId)
                    .AddFilter("produits",
                        e => e.ProductId != null && ProductsId.Any() && ProductsId.Contains(e.ProductId.Value)));
        }

        private readonly IProperty<ObservableQuery<Sample>> _searchHistoryList = H.Property<ObservableQuery<Sample>>(c => c
            .On(e => e.PharmacopoeiaId)
            .On(e => e.ProductsId)
            .Do((e,f) => f.Get().Update())
        );

        [TriggerOn(nameof(SearchHistoryList), "Selected")]
        private void UpdateHistoriqueSelected()
        {
            _msg.Publish(new DetailMessage(SearchTestList.Selected));
        }

        public ObservableQuery<TestClass> SearchTestList
        {
            get => _searchTestList.Get();
            set => _searchTestList.Set(value.FluentUpdate());
        }

        private readonly IProperty<ObservableQuery<TestClass>> _searchTestList = H.Property<ObservableQuery<TestClass>>(c => c
            //.On(e => e)
            //.Update()
        );




        [TriggerOn(nameof(SearchTestList), "Selected")]
        private void UpdateTestSelected()
        {
            _msg.Publish(new DetailMessage(SearchTestList.Selected));
        }


        public int SelectedId => _selectedId.Get();
        private readonly IProperty<int> _selectedId = H.Property<int>(c => c
            .On(e => e.SearchHistoryList.Selected)
            .Set(e => e.SearchHistoryList?.Selected?.Id ?? -1));



        public ObservableQuery<SampleTest> SearchTestEchantillonList
        {
            get => _searchTestEchantillonList.Get();
            set => _searchTestEchantillonList.Set(value.AddFilter("OneToMany", e => e.SampleId == SelectedId));
        }

        private readonly IProperty<ObservableQuery<SampleTest>> _searchTestEchantillonList = H.Property<ObservableQuery<SampleTest>>(c => c
            .On(e => e.SelectedId)
            .Do(e => e.SearchHistoryList.OnTriggered())
        );


    }
}
