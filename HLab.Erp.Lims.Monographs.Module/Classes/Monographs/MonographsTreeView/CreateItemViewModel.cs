using HLab.Mvvm;
using HLab.Mvvm.Application;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<CreateItemViewModel>;

    internal class CreateItemViewModel : ViewModel<IListableModel>
    {

        public string IconPath => _icon.Get();

        readonly IProperty<string> _icon = H.Property<string>(c => c
            .On(e => e.Model.IconPath)
            .Set(e => e.Model.IconPath)
        );


        public string Caption => _caption.Get();

        readonly IProperty<string> _caption = H.Property<string>(c => c
            .On(e => e.Model.Caption)
            .Set(e => e.Model.Caption)
        );

    }
}
