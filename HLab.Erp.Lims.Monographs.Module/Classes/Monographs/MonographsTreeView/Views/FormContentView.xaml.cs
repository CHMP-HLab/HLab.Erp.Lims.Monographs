using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour FormeContentView.xaml
    /// </summary>
    public partial class FormContentView : UserControl,
        IView<ViewModeContent, FormTreeElementViewModel>
    {
        public FormContentView()
        {
            InitializeComponent();
        }
    }
}
