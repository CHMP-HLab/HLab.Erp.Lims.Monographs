using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Models;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Solution
{
    /// <summary>
    /// Logique d'interaction pour SoluteValueView.xaml
    /// </summary>
    public partial class SoluteValueView : UserControl, 
        IView<ViewModeEdit, IInputPin>, IViewClassPinContent
    {
        public SoluteValueView()
        {
            InitializeComponent();
        }
    }
}
