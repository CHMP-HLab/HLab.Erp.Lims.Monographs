using System.Windows.Input;
using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Icons.Annotations.Icons;
using HLab.Mvvm;
using HLab.Mvvm.Application;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<MonographTreeElementViewModel>;

    internal class MonographTreeElementViewModel : MonographTreeElement<Monograph>
    {
        readonly IDocumentService _docs;
        readonly IDialogService _dialog;

        public MonographTreeElementViewModel(IDataService db, IIconService icons, IDocumentService docs, IDialogService dialog) : base(db,icons)
        {
            _docs = docs;
            _dialog = dialog;
            H.Initialize(this);
        }

        public string Caption => Model.Pharmacopoeia.Abbreviation + " - " + Model.PharmacopoeiaVersion;


        public ICommand OpenCommand { get; } = H.Command(c => c
            .Action(
                (e) => { e._docs.OpenDocumentAsync(e.Model); })
        );


        public ICommand DeleteCommand { get; } = H.Command(c => c
            .Action(
                (self, sender) =>
                {
                    if (self.DeleteModel(self._dialog, self.Db, "Supprimer la monographie", self.Model.Caption))
                    {
                        self.Root.Monographs.FluentUpdate();
                    }

                })
        );

    }
}