using HLab.Core.Annotations;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Core.ToolBoxes;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Tool
{
    public class TestClassToolViewModel : ViewModel
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
        
        public IMessagesService MessageBus { get; } 
        public TestClassToolViewModel(IMessagesService messageBus, ObservableQuery<TestClass> searchList)
        {
            MessageBus = messageBus;
            SearchList = searchList;
            H<TestClassToolViewModel>.Initialize(this);

            searchList.Update();
        }

        public string Title => "{Tests}";

        public ObservableQuery<TestClass> SearchList { get; }

        [TriggerOn(nameof(SearchList), "Selected")]
        void UpdateConsommableSelected()
        {
            MessageBus.Publish(new DetailMessage(SearchList.Selected));
        }

        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
        }
    }
}
