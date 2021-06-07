using System.Windows.Controls;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Detail;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Detail
{
    /// <summary>
    /// Logique d'interaction pour TestTypeView.xaml
    /// </summary>
    public partial class TestTypeView : UserControl,
        IView<ViewModeDefault,TestClassViewModel>, IViewClassDetail
    {
        public TestTypeView()
        {
            InitializeComponent();
        }
    }
}
