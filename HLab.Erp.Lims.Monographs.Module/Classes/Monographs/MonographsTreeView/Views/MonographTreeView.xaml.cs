using System.Windows.Controls;
using System.Windows.Input;
using HLab.Erp.Core;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour MonographTreeView.xaml
    /// </summary>
    public partial class MonographTreeView : UserControl, 
        IView<ViewModeDefault, MonographsListViewModel>, IViewClassAnchorable
    {
        public MonographTreeView()
        {
            InitializeComponent();
        }

        public string ContentId => nameof(MonographTreeView);

        void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
