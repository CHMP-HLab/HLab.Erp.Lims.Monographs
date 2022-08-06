using System.Windows.Input;
using HLab.Core.Annotations;
using HLab.Erp.Core;
using HLab.Mvvm.Application;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<TreeListModule>;

    public class TreeListModule : NotifierBase, IBootloader
    {
        readonly IMenuService _menu;
        readonly IDocumentService _doc;

        public TreeListModule(IMenuService menu, IDocumentService doc)
        {
            _menu = menu;
            _doc = doc;
            H.Initialize(this);
        }

        public ICommand OpenMonographsCommand { get; } = H.Command(c => c.Action(
            e => e._doc.OpenDocumentAsync<MonographsListViewModel>()
        ));

        public void Load(IBootContext b)
        {
            _menu.RegisterMenu("data/monograph", "{Monographs}", OpenMonographsCommand, "Icons/Entities/Monograph");
        }
    }
}