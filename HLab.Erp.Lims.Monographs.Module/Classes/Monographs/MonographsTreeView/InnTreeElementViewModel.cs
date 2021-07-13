using System.Collections.Generic;
using System.Linq;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<InnTreeElementViewModel>;

    class InnTreeElementViewModel : MonographTreeElement<Inn>, ITreeContentViewModel
    {
        public InnTreeElementViewModel() => H.Initialize(this);


        [TriggerOn(nameof(Model),"Name")]
        public string Caption => Model.Name;


        public List<int> MonographSourceId => _monographSourceId.Get();
        private readonly IProperty<List<int>> _monographSourceId = H.Property<List<int>>(c => c
            .On(e => e.MonographSource.Item().FormId)
            .Set(e => e.MonographSource
                .Where(m => m.FormId!=null)
                .Select(m => m.FormId.Value)
                .Distinct()
                .ToList()
            )
        );


        public IObservableFilter<Form> Children { get; } = H.Filter<Form>(c => c
            .AddFilter((s,e) => s.MonographSourceId.Contains(e.Id))
            .Link(e => e.Root?.FormSource)
            .On(e => e.Root.FormSource)
            .On(e => e.MonographSourceId)
            .Update()
        );
    }
}