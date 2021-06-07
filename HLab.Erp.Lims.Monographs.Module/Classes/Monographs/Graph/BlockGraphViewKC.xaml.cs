using System;
using System.Windows;
using System.Windows.Controls;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    /// <summary>
    /// Logique d'interaction pour BlockFournisseurView.xaml
    /// </summary>
    public partial class BlockGraphViewKC : UserControl
    {
        public event EventHandler SelectedChanged;
        public BlockGraphViewKC()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(nameof(Selected),
            typeof(bool), typeof(BlockGraphViewKC), new FrameworkPropertyMetadata(OnSelectedChanged));

        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register(nameof(Container),
            typeof(FrameworkElement), typeof(BlockGraphViewKC), new FrameworkPropertyMetadata());

        public FrameworkElement Container
        {
            get => (FrameworkElement)GetValue(ContainerProperty); set => SetValue(ContainerProperty, value);
        }

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty); set => SetValue(SelectedProperty, value);
        }

        private static void OnSelectedChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            (source as BlockGraphViewKC)?.SelectedChanged(source, new EventArgs());
        }
    }
}
