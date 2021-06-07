using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Test
{
    /// <summary>
    /// Logique d'interaction pour TestContentBlock.xaml
    /// </summary>
    public partial class TestContentBlock : UserControl,
        IView<ViewModeDefault, AssayBlock>,
        IView<ViewModeEdit, AssayBlock>,
        IViewClassBlockContent
    {
        public TestContentBlock()
        {
            InitializeComponent();
        }
    }
}
