using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection
{
    /// <summary>
    /// Logique d'interaction pour InjectionBlockContent.xaml
    /// </summary>
    public partial class InjectionBlockContent : UserControl,
        IView<ViewModeDefault,InjectionBlock>,
        IView<ViewModeEdit,InjectionBlock>,
        IViewClassBlockContent
    {
        public InjectionBlockContent()
        {
            InitializeComponent();
        }
    }
}
