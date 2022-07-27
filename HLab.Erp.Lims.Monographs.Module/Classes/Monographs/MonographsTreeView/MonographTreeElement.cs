using System.Windows;
using System.Windows.Input;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Icons.Annotations.Icons;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    public interface IMonographTreeElement 
    {
        IMonographTreeElement Parent { get; set; }
        MonographsListViewModel Root { get; }
        Inn Inn { get; }
        Form Form { get; }
        Pharmacopoeia Pharmacopoeia { get; }
    }


    public abstract class MonographTreeElement<T> : ViewModel<T>, IMonographTreeElement, IMvvmContextProvider
        where T : IEntity, IListableModel
    {
        public IDataService Db { get; }
        public IIconService Icons { get; }

        protected MonographTreeElement(IDataService db, IIconService icons)
        {
            Db = db;
            Icons = icons;
            H<MonographTreeElement<T>>.Initialize(this);
        }

        public Inn Inn => _inn.Get();

        readonly IProperty<Inn> _inn = H<MonographTreeElement<T>>.Property<Inn>(c => c
            .On(e => e.Model)
            .On(e => e.Parent.Inn)
            .Set(e => (e.Model as Inn) ?? e.Parent.Inn));

        public Form Form => _form.Get();

        readonly IProperty<Form> _form = H<MonographTreeElement<T>>.Property<Form>(c => c
            .On(e => e.Model)
            .On(e => e.Parent.Form)
            .Set(e => (e.Model as Form) ?? e.Parent?.Form));

        public Pharmacopoeia Pharmacopoeia => _pharmacopoeia.Get();

        readonly IProperty<Pharmacopoeia> _pharmacopoeia = H<MonographTreeElement<T>>.Property<Pharmacopoeia>(c => c
            .On(e => e.Model)
            .On(e => e.Parent.Pharmacopoeia)
            .Set(e => (e.Model as Pharmacopoeia) ?? e.Parent?.Pharmacopoeia));


        public string Version => "";

        [TriggerOn(nameof(Model), "IconName")] public string IconPath => _iconPath.Get();

        IProperty<string> _iconPath = H<MonographTreeElement<T>>.Property<string>(c => c
            .On(e => e.Model.IconPath)
            .Set(e => e.Model?.IconPath)
            
            );
        public bool IsExpanded
        {
            get => _isExpanded.Get();
            set => _isExpanded.Set(value);
        }

        readonly IProperty<bool> _isExpanded = H<MonographTreeElement<T>>.Property<bool>(c => c.Default(true));

        public bool IsSelected
        {
            get => _isSelected.Get();
            set => _isSelected.Set(value);
        }

        readonly IProperty<bool> _isSelected = H<MonographTreeElement<T>>.Property<bool>(c => c.Default(false));


        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
            ctx.AddCreator<IMonographTreeElement>(m => { m.Parent = this; });
        }

        public ICommand ClickCommand { get; } = H<MonographTreeElement<T>>.Command(c => c
            .Action(
                (self) => self.ChildrenVisibility = self.ChildrenVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible
                )
        );


        public Visibility ChildrenVisibility
        {
            get => _childrenVisibility.Get();
            set => _childrenVisibility.Set(value);
        }

        readonly IProperty<Visibility> _childrenVisibility = H<MonographTreeElement<T>>.Property<Visibility>(c => c.Default(Visibility.Visible));


        public virtual MonographsListViewModel Root => Parent?.Root;

        public virtual IMonographTreeElement Parent
        {
            get => _parent.Get();
            set => _parent.Set(value);
        }

        readonly IProperty<IMonographTreeElement> _parent = H<MonographTreeElement<T>>.Property<IMonographTreeElement>();


        public IObservableFilter<Monograph> MonographSource { get; } = H<MonographTreeElement<T>>.Filter<Monograph>(c => c
                .AddFilter((s,e) => s.Inn == null || e.InnId == s.Inn.Id)
                .AddFilter((s,e) => s.Form == null || e.FormId == s.Form.Id)
                .AddFilter((s,e) => s.Pharmacopoeia == null || e.PharmacopoeiaId == s.Pharmacopoeia.Id)
                .Link(e => e.Root?.Monographs)

                .On(e => e.Root.Monographs)
                .On(e => e.Inn)
                .On(e => e.Form)
                .On(e => e.Pharmacopoeia)
                .Update()
        );

        public ICommand CreateCommand { get; } = H<MonographTreeElement<T>>.Command(c => c
            .Action(
                (self) => {
                    var created = self.MvvmContext.Locate<CreateMonographViewModel>()
                        .Create(self.Inn, self.Form, self.Pharmacopoeia, self.Version);

                    if (created)
                    {
                        self.Root.Monographs.FluentUpdate();
                    }
                })
        );

    }


}