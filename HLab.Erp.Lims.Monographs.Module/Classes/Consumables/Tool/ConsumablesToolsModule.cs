using System.Windows.Input;
using HLab.Core.Annotations;
using HLab.Erp.Core;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Tool
{
    using H = H<ConsumablesToolsModule>;

    public class ConsumablesToolsModule : NotifierBase, IBootloader
    {
        private readonly IErpServices _erp;

        public ConsumablesToolsModule(IErpServices erp)
        {
            _erp = erp;
            H.Initialize(this);
        }

        public ICommand ConsumablesListCommand { get; } = H.Command(c => c
            .Action(
                e => e._erp.Docs.OpenDocumentAsync<ConsumablesToolsViewModel>()
            )
        );

        public void Load(IBootContext b)
        {
            _erp.Menu.RegisterMenu("data/consumables", "{Consumables}", ConsumablesListCommand, "Icons/Consumable");
        }
    }
}
