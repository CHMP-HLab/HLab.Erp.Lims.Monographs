using System.Windows;
using HLab.Erp.Core;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.EditorTool
{
    /// <summary>
    /// Logique d'interaction pour MonographieEditor.binary
    /// </summary>
    public partial class MonographEditorView : IViewClassAnchorable, 
        IView<ViewModeDefault,MonographEditorViewModel>
    {
        public MonographEditorView()
        {
            InitializeComponent();

            DataContextChanged += MonographEditorView_DataContextChanged;
        }

        private void MonographEditorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is MonographEditorViewModel vm)
            {
                vm.Reader = FlowDocumentReader;
                vm.SourceEditor = TextEditor;
                vm.Editor = RichTextBox;
            }
        }

        public string ContentId => GetType().Name;
    }


}
