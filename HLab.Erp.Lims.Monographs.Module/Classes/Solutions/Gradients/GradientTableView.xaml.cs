using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Gradients
{
    /// <summary>
    /// Logique d'interaction pour GradientTableView.xaml
    /// </summary>
    public partial class GradientTableView : UserControl
    {
        public GradientTableView()
        {
            InitializeComponent();
            var view = CollectionViewSource.GetDefaultView(ListView);
            view?.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Ascending));
        }
    }
}
