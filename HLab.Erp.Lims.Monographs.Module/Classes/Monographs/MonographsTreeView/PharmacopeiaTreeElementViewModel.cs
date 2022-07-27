using System.Windows;
using System.Windows.Media;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Icons.Annotations.Icons;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView
{
    using H = H<PharmacopoeiaTreeElementViewModel>;
    public class PharmacopoeiaTreeElementViewModel : MonographTreeElement<Pharmacopoeia>
    {
        public Visibility IconVisibility => Visibility.Visible;
        public Brush TabColor => new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops =
            {
                new GradientStop(Colors.Transparent,0),
                new GradientStop(Colors.Silver,1)
            }
        };

        //public bool IsExpanded { get => N<>.Get(() => true); set => N.Set(value); }
        //public bool IsSelected { get => N.Get(() => false); set => N.Set(value); }

        public IObservableFilter<Monograph> Children { get; } = H.Filter<Monograph>(c => c
            .Link(e => e.MonographSource)
            .On(e => e.MonographSource)
            .Update()
        );

        public PharmacopoeiaTreeElementViewModel(IDataService db, IIconService icons) : base(db, icons)
        {
            H.Initialize(this);
        }
    }
}
