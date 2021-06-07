using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Graph
{
    /// <summary>
    /// Logique d'interaction pour TestTypeContainer.xaml
    /// </summary>
    public partial class TestClassContainer : UserControl ,
        IView<ViewModeDefault,TestClassGraphViewModel>, IViewClassGraphContainer
    {
        public TestClassContainer()
        {
            InitializeComponent();
        }
    }
}
