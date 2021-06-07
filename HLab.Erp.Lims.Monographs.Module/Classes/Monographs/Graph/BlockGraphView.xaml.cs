using System.Windows;
using System.Windows.Controls;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    /// <summary>
    /// Logique d'interaction pour ConsommableView.xaml
    /// </summary>
    /// 

    public partial class BlockGraphView : UserControl
    {
        public BlockGraphView()
        {
            InitializeComponent();
        }

        public FrameworkElement IconPlaceHolder => (FrameworkElement)GetTemplateChild("IconPlaceHolder");
    }
}