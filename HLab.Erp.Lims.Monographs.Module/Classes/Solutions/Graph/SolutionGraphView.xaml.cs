using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph
{
    /// <summary>
    /// Logique d'interaction pour SolutionMonographieView.xaml
    /// </summary>
    public partial class SolutionFlowchartView : UserControl
        , IView<ViewModeDefault, SolutionGraphViewModel>
        , IViewClassFlowchart
    {
        public SolutionFlowchartView()
        {
            InitializeComponent();

            var icon = Block.IconPlaceHolder;

            if (icon != null)
            {
                icon.AllowDrop = true;
                icon.Drop += UIElement_OnDrop;
            }

            Block.AllowDrop = true;
            Block.Drop += UIElement_OnDrop;

            SizeChanged += SolutionGraphView_SizeChanged;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (DataContext is SolutionGraphViewModel vm) vm.Width = this.ActualWidth;
        }

        private void SolutionGraphView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is SolutionGraphViewModel vm) vm.Width = this.ActualWidth;
        }

        private void TextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(sender is TextBox t) t.Background=new SolidColorBrush(Colors.White);
        }

        private void TextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox t) t.Background = new SolidColorBrush(Colors.Transparent);
        }

        public SolutionGraphViewModel ViewModel => DataContext as SolutionGraphViewModel;

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            var formats = e.Data.GetFormats();

            var text = e.Data.GetData("UnicodeText") as string;

            ViewModel.Link();

            e.Handled = true;
        }


    }
}
