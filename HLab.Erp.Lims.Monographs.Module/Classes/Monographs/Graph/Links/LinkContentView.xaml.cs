using System.Windows;
using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph.Links
{
    /// <summary>
    /// Logique d'interaction pour LinkContentView.xaml
    /// </summary>
    public partial class LinkContentView : UserControl,
        IView<ViewModeEdit,LinkGraphViewModel>, IViewClassFlowchart
    {
        public LinkContentView()
        {
            InitializeComponent();
        }

        public LinkGraphViewModel ViewModel => DataContext as LinkGraphViewModel;

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            var formats = e.Data.GetFormats();

            var text = e.Data.GetData("UnicodeText") as string;

            ViewModel.Link();

            e.Handled = true;
        }
    }
}
