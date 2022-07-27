using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Icons.Annotations.Icons;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<FormTreeElementViewModel>;
    public interface ITreeContentViewModel
    { }

    internal class FormTreeElementViewModel : MonographTreeElement<Form>, ITreeContentViewModel
    {
        public Visibility IconVisibility => Visibility.Visible;
        public Brush TabColor => new LinearGradientBrush
        {
            StartPoint = new Point(0,0),
            EndPoint = new Point(1,0),
            GradientStops =
            {
                new GradientStop(Colors.Black,0),
                new GradientStop(Colors.MidnightBlue,1)
            }
        };

        public List<int> MonographSourceId => _monographSourceId.Get();

        readonly IProperty<List<int>> _monographSourceId = H.Property<List<int>>(c => c
            .On(e => e.MonographSource.Item().PharmacopoeiaId)
            .Set(e => e.MonographSource
                .Where(m => m.PharmacopoeiaId != null)
                .Select(m => m.PharmacopoeiaId.Value)
                .Distinct()
                .ToList())
        );

        public IObservableFilter<Pharmacopoeia> Children { get; } = H.Filter<Pharmacopoeia>(c => c
            .AddFilter((s,e) => s.MonographSourceId.Contains(e.Id))
            .Link(e => e.Root?.PharmacopoeiaSource)
            .On(e => e.Root.PharmacopoeiaSource.Item())
            .On(e => e.MonographSourceId)
            .Update()
        );

        public FormTreeElementViewModel(IDataService db, IIconService icons) : base(db, icons)
        {
        }
    }
}