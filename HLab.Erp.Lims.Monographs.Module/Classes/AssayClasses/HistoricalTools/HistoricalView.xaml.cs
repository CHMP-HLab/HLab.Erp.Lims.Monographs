using System;
using System.Windows;
using System.Windows.Controls;
using HLab.Erp.Core;
using HLab.Erp.Core.DragDrops;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;


namespace HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.HistoricalTools
{
    /// <summary>
    /// Logique d'interaction pour Historical.xaml
    /// </summary>
    public partial class HistoricalView : UserControl, IViewClassAnchorable,
        IView<ViewModeDefault, HistoricalViewModel>
    {

        public IMvvmService Mvvm { get; }

        public Func<Panel, FrameworkElement, bool, ErpDragDrop> GetDragDrop { get; set; }

        public void Inject(IDragDropService dragDrop)
        {
            var drag2 = GetDragDrop((Panel)dragDrop.GetDragCanvas(), ListView, true);

            drag2.Start += drag_Start;
            drag2.Drop += drag_Drop;
        }
        public HistoricalView(IMvvmService mvvm)
        {
            Mvvm = mvvm;
            InitializeComponent();
        }
        private void drag_Drop(object source)
        {

        }

        private void drag_Start(ErpDragDrop source)
        {
            if (source == null) return;
            if (ListView.SelectedValue == null) return;

            source.DraggedElement = (FrameworkElement)Mvvm.ViewHelperFactory.Get(this).Context.GetView<ViewModeDraggable>(
                ListView.SelectedValue
            );
        }

        public string ContentId => this.GetType().Name;
    }
}
