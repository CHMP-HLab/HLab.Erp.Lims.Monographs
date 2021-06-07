using System.Windows.Input;
using HLab.Erp.Core.WebService;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Mvvm.Application;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs
{
    using H = H<MonographViewModel>;

    public abstract class MonographViewModel : ViewModel<Monograph>, IViewClassDocument
    {
        public void Inject(IBrowserService browser)
        {
            Browser = browser;
        }
        public IBrowserService Browser { get; set; }

        public string Title => 
            Model.Inn.Name + " " + 
            Model.Form.Name + "\n" + 
            Model.Pharmacopoeia.Abbreviation + " " + 
            Model.PharmacopoeiaVersion;


        public ICommand OpenWebPharmacopoeiaCommand { get; } = H.Command(c => c
            .Action(
                (self) =>
                {
                    if (self.Model.Pharmacopoeia.ReferenceUrl == null) return;

                    var url = string.IsNullOrWhiteSpace(self.Model.Reference) ? self.Model.Pharmacopoeia.SearchUrl.Replace("(search)", self.Model.Inn.Name) : self.Model.Pharmacopoeia.ReferenceUrl.Replace("(reference)", self.Model.Reference);

                    self.Browser.Navigate(url);
                })
        );


        public ICommand OpenLocalPharmacopoeiaCommand { get; } = H.Command(c => c
            .Action(
                (self) =>
                {
                    // TODO : MonographEditorViewModel.OpenCommand.Execute(Model);
                })
        );


        public string IconForm => _iconForm.Get();
        private readonly IProperty<string> _iconForm = H.Property<string>(c => c
            .On(e => e.Model.Form.IconPath)
            .Set(e => e.Model.Form.IconPath)
        );


        //TODO : new icon system
        public string IconPharmacopoeia => _iconPharmacopoeia.Get();
        private readonly IProperty<string> _iconPharmacopoeia = H.Property<string>(c => c
            .On(e => e.Model.Pharmacopoeia.IconPath)
            .Set(e => e.Model.Pharmacopoeia.IconPath)
        );



        public string ContentId => "document";
    }
}
