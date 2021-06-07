using System.Windows.Input;
using HLab.Core.Annotations;
using HLab.Erp.Core;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<TreeListModule>;

    public class TreeListModule : NotifierBase, IBootloader
    {
        private readonly IErpServices _erp;

        public TreeListModule(IErpServices erp)
        {
            _erp = erp;
            H.Initialize(this);
        }

        public ICommand OpenMonographsCommand { get; } = H.Command(c => c.Action(
            e => e._erp.Docs.OpenDocumentAsync<MonographsListViewModel>()
        ));

        public void Load(IBootContext b)
        {
            _erp.Menu.RegisterMenu("data/monograph", "{Monographs}", OpenMonographsCommand, "Icons/Entities/Monograph");
        }
    }
}