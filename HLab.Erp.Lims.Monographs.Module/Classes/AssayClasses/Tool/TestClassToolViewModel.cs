using HLab.Core.Annotations;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Core.ToolBoxes;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
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
            public override bool Allowed => Erp.Acl.IsGranted(AclRights.BetaTest);
            public override string MenuPath => "tools";
        }
        
        public IMessageBus MessageBus { get; } 
        public TestClassToolViewModel(IMessageBus messageBus, ObservableQuery<TestClass> searchList)
        {
            MessageBus = messageBus;
            SearchList = searchList;
            H<TestClassToolViewModel>.Initialize(this);

            searchList.Update();
        }

        public string Title => "{Tests}";

        public ObservableQuery<TestClass> SearchList { get; }

        [TriggerOn(nameof(SearchList), "Selected")]
        private void UpdateConsommableSelected()
        {
            MessageBus.Publish(new DetailMessage(SearchList.Selected));
        }

        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
        }
    }
}
