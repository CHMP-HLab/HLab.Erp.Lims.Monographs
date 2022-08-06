using System.Windows.Input;
using HLab.Core.Annotations;
using HLab.Erp.Core;
using HLab.Mvvm.Application;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Tool;

using H = H<ConsumablesToolsModule>;

public class ConsumablesToolsModule : NotifierBase, IBootloader
{
    readonly IDocumentService _docs;
    readonly IMenuService _menu;

    public ConsumablesToolsModule(IDocumentService docs, IMenuService menu)
    {
        _docs = docs;
        _menu = menu;
        H.Initialize(this);
    }

    public ICommand ConsumablesListCommand { get; } = H.Command(c => c
        .Action(
            e => e._docs.OpenDocumentAsync<ConsumablesToolsViewModel>()
        )
    );

    public void Load(IBootContext b)
    {
        _menu.RegisterMenu("data/consumables", "{Consumables}", ConsumablesListCommand, "Icons/Consumable");
    }
}