using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Views;
using HLab.Mvvm.Wpf.Views;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<CreateMonographViewModel>;

    internal class InnViewModel : ViewModel<Inn>
    {

        public string Caption => Model.Name;
    }

    internal class FormViewModel : ViewModel<Form>
    {
        public string Caption => Model.Name;
    }

    internal class PharmacopoeiaViewModel : ViewModel<Pharmacopoeia>
    {
        public string Caption => Model.Name;
    }


    internal class CreateMonographViewModel : ViewModel
    {
        readonly IDataService _db;

        public CreateMonographViewModel(IDataService db, IMvvmService mvvm)
        {
            _db = db;
            H.Initialize(this);
        }

        public bool Create(Inn inn,Form form, Pharmacopoeia pharmacopoeia, string version)
        {
            if (inn != null)
            {
                InnEnabled = false;
                InnList.Selected = InnList.FirstOrDefault(d => d.Id == inn.Id);
            }
            else InnEnabled = true;

            if (form != null)
            {
                FormList.Selected = FormList.FirstOrDefault(f => f.Id == form.Id);
                FormEnabled = false;
            }
            else FormEnabled = true;

            if (pharmacopoeia != null)
            {
                PharmacopoeiaList.Selected = PharmacopoeiaList.FirstOrDefault(f => f.Id == pharmacopoeia.Id);
                PharmacopoeiaEnabled = false;
            }
            else PharmacopoeiaEnabled = true;

            if (!string.IsNullOrWhiteSpace(version))
            {
                PharmacopoeiaVersion = version;
                PharmacopoeiaVersionEnabled = false;
            }
            else PharmacopoeiaVersionEnabled = true;

            var view = MvvmContext.GetView<ViewModeDefault>(this).AsWindow();

            view.ShowDialog();

            return Created;
        }

        public bool Created
        {
            get => _created.Get();
            set => _created.Set(value);
        }

        readonly IProperty<bool> _created = H.Property<bool>(c => c.Default(false));

        public bool InnEnabled
        {
            get => _innEnabled.Get();
            set => _innEnabled.Set(value);
        }

        readonly IProperty<bool> _innEnabled = H.Property<bool>();


        public bool FormEnabled
        {
            get => _formEnabled.Get();
            set => _formEnabled.Set(value);
        }

        readonly IProperty<bool> _formEnabled = H.Property<bool>();


        public bool PharmacopoeiaEnabled
        {
            get => _pharmacopoeiaEnabled.Get();
            set => _pharmacopoeiaEnabled.Set(value);
        }

        readonly IProperty<bool> _pharmacopoeiaEnabled = H.Property<bool>();


        public bool PharmacopoeiaVersionEnabled
        {
            get => _pharmacopoeiaVersionEnabled.Get();
            set => _pharmacopoeiaVersionEnabled.Set(value);
        }

        readonly IProperty<bool> _pharmacopoeiaVersionEnabled = H.Property<bool>();



        public ObservableQuery<Inn> InnList
        {
            get => _innList.Get();
            set => _innList.Set(value.AddFilterFunc((d) => d.OrderBy(e => e.Name)).FluentUpdate());
        }

        readonly IProperty<ObservableQuery<Inn>> _innList = H.Property<ObservableQuery<Inn>>(c => c
            //.On(e => e)
            //.Update()
        );



        public ObservableQuery<Form> FormList
        {
            get => _formList.Get();
            set => _formList.Set(value.AddFilterFunc(d => d.OrderBy(fp => fp.Name)).FluentUpdate());
        }

        readonly IProperty<ObservableQuery<Form>> _formList = H.Property<ObservableQuery<Form>>(c => c
            //.On(e => e)
            //.Update()
        );



        public ObservableQuery<Pharmacopoeia> PharmacopoeiaList
        {
            get => _pharmacopoeiaList.Get();
            set => _pharmacopoeiaList.Set(value.AddFilterFunc(d => d.OrderBy(fp => fp.Name)).FluentUpdate());
        }

        readonly IProperty<ObservableQuery<Pharmacopoeia>> _pharmacopoeiaList = H.Property<ObservableQuery<Pharmacopoeia>>(c => c
            //.On(e => e)
            //.Update()
        );



        public List<string> PharmacopoeiaVersionList => _pharmacopoeiaVersionList.Get();

        readonly IProperty<List<string>> _pharmacopoeiaVersionList = H.Property<List<string>>(c => c
            .On(e => e.PharmacopoeiaList.Selected)
            .Set(async e => await e._db.FetchAsync<Monograph>().Select(m => m.PharmacopoeiaVersion).Distinct().OrderBy(i => i).ToListAsync())
        );


        public string PharmacopoeiaVersion
        {
            get => _pharmacopoeiaVersion.Get();
            set => _pharmacopoeiaVersion.Set(value);
        }

        readonly IProperty<string> _pharmacopoeiaVersion = H.Property<string>();

        public ICommand OkCommand { get; } = H.Command(c => c
            .CanExecute(self =>
                (self.InnList.Selected != null) &&
                (self.FormList.Selected != null) &&
                (self.PharmacopoeiaList.Selected != null) &&
                !string.IsNullOrWhiteSpace(self.PharmacopoeiaVersion))
            .Action(
                (self) =>
                {
                    self._db.Add<Monograph>((m) => {
                        m.InnId = self.InnList.Selected.Id;
                        m.FormId = self.FormList.Selected.Id;
                        m.PharmacopoeiaId = self.PharmacopoeiaList.Selected.Id;
                        m.PharmacopoeiaVersion = self.PharmacopoeiaVersion;
                    });

                    self.Created = true;
                    //TODO : if (sender is Window w) w.Close();
                }
                )
            .On(self => self.InnList.Selected)
            .On(self => self.FormList.Selected)
            .On(self => self.PharmacopoeiaList.Selected)
            .On(self => self.PharmacopoeiaVersion)
            .CheckCanExecute()
        );

        public ICommand CancelCommand { get; } = H.Command(c => c
            .On(e => e)
            .Action(
                (self) =>
                {
                self.Created = false;
                //TODO : (sender as Window)?.Close();

                })
        );
    }
}
