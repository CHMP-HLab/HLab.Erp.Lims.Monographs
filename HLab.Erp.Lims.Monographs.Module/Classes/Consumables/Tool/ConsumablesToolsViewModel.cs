using System.Globalization;
using System.Windows.Documents;
using System.Windows.Input;

using HLab.Core.Annotations;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Core.ToolBoxes;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.EditorTool;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Tool
{
    using H = H<ConsumablesToolsViewModel>;

    internal class ConsumablesToolsViewModel : ViewModel
        , IMvvmContextProvider
        , IToolListViewModel
    {
        public class Bootloader : NestedBootloader
        {
            readonly IAclService _acl;
            public Bootloader(IAclService acl)
            {
                _acl = acl;
            }
            public override bool Allowed => _acl.IsGranted(AclRights.BetaTest);
            public override string MenuPath => "tools";
        }

        readonly IDataService _db;

        public ConsumablesToolsViewModel(IMessagesService msg, IDataService db)
        {
            _db = db;
            msg.Subscribe<SelectedMonographieEditor>(OnSelectedText);
            msg.Subscribe<FlowDocument>(SetDocument);

            H.Initialize(this);
        }

        public string Title => "{Consumables}";

        void SetDocument(FlowDocument doc) => Document = doc;

        public FlowDocument Document
        {
            get => _document.Get();
            set => _document.Set(value);
        }

        readonly IProperty<FlowDocument> _document = H.Property<FlowDocument>();

        public string Search
        {
            get => _search.Get();
            set => _search.Set(value);
        }

        readonly IProperty<string> _search = H.Property<string>();


        void OnSelectedText(SelectedMonographieEditor msg)
        {
            Search = msg.Text;
        }


        [TriggerOn(nameof(Search))]
        [TriggerOn(nameof(Document))]
        public void SearchDocument()
        {
            if (string.IsNullOrWhiteSpace(Search) && Document != null)
            {
                SearchList.AddFilter("Search", e => Document.InDocument((e.Name ?? "").Trim())).FluentUpdate();
                (CreateCommand as NCommand)?.CanExecute(false);
            }
            else
            {
                //SearchConsommableList.AddFilter(
                //    "Search",
                //    consommable 
                //    => consommable.Designation.Contains(Search.Trim()) || Search.Contains(consommable.Designation)
                //).FluentUpdate();

                SearchList.AddPostFilter(
                    "Search",
                    c =>
                        CultureInfo.CurrentCulture.CompareInfo.IndexOf(Search.Trim(), c.Name, CompareOptions.IgnoreCase) >= 0 ||
                        CultureInfo.CurrentCulture.CompareInfo.IndexOf(c.Name, Search, CompareOptions.IgnoreCase) >= 0

                ).FluentUpdate/*Async*/(true);


                (CreateCommand as NCommand)?.CanExecute(!_db.Any<Consumable>(e => e.Name == Search));
            }
        }

        public ObservableQuery<Consumable> SearchList
        {
            get => _searchList.Get();
            set => _searchList.Set(value.FluentUpdate());
        }

        readonly IProperty<ObservableQuery<Consumable>> _searchList = H.Property<ObservableQuery<Consumable>>(c => c
            //.On(e => e)
            //.Update()
        );




        //[TriggerOn("SearchConsommableList")]
        //[TriggerOn("ViewModeContext")]
        //public ObservableViewModelCollection<ConsommableDragDropViewModel> SearchConsommableViewModels => N.Get(
        //    () => new ObservableViewModelCollection<ConsommableDragDropViewModel>()
        //    .SetViewModeContext(ViewModeContext)
        //    .SetViewMode("ToolItem")
        //    .Link(SearchConsommableList)
        //);

        public ICommand Command { get; } = H.Command(c => c
            .Action(
                (e) =>
                {
                    var cs = e._db.Add<Consumable>(i =>
                    {
                        i.Name = e.Search;
                    });

                    e.SearchList.FluentUpdate();
                    e.SearchList.Selected = cs;
                })
        );

        public ICommand CreateCommand { get; }= H.Command(c => c
            .Action(
                (e, n) =>
                {
                    var cs = e._db.Add<Consumable>(f =>
                    {
                        f.Name = e.Search;
                    });

                    e.SearchList.FluentUpdate();
                    e.SearchList.Selected = cs;

                })
        );



        [TriggerOn(nameof(SearchList), "Selected")]
        public void UpdateConsommableSelected()
        {
            //_msg.Publish(new DetailMessage(SearchList.Selected));
            //DetailPanel = SearchConsommableList.Selected;
        }

        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
        }
    }
}
