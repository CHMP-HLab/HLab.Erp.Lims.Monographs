using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using HLab.Core.Annotations;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using ICSharpCode.AvalonEdit;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.EditorTool
{
    using H = H<MonographEditorViewModel>;

    public class MonographEditorViewModel : ViewModel
    {
        private readonly IMessageBus _msg;

        public MonographEditorViewModel(IMessageBus messageBus)
        {
            _msg = messageBus;
            H.Initialize(this);
        }

        //TODO

        //public NotifyCommand OpenCommand =>  N.Get(() => new NotifyCommand(
        //    m => _app.OpenDocument(new MonographEditorViewModel
        //    {
        //        Monograph = m as Monograph
        //    }),
        //    () => true
        //));

        public Monograph Monograph
        {
            get => _monograph.Get();
            set => _monograph.Set(value);
        }

        private readonly IProperty<Monograph> _monograph = H.Property<Monograph>();


        public FlowDocument Document
        {
            get => _document.Get();
            set => _document.Set(value);
        }

        private readonly IProperty<FlowDocument> _document = H.Property<FlowDocument>();



        private bool _loadFromBinary = true;
        private bool _saveChanges = true;

        public void SaveDocument(FlowDocument doc)
        {
            using (var tempstream = new MemoryStream())
            {
                XamlWriter.Save(doc, tempstream);

                tempstream.Seek(0, SeekOrigin.Begin);

                var doc2 = (FlowDocument)XamlReader.Load(tempstream);

                ClearHighlight(doc2);
                RemoveImages(doc2);

                using (var stream = new MemoryStream())
                {
                    XamlWriter.Save(doc2, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    _loadFromBinary = false;
                    Monograph.Document = stream.ToArray();
                    _loadFromBinary = true;
                }
            }
        }

       public Visibility ReaderVisibility => _readerVisibility.Get();
       private readonly IProperty<Visibility> _readerVisibility = H.Property<Visibility>(c => c
           .On(e => e.EditMode)
           .On(e => e.SourceMode)
           .Set(e => (e.EditMode||e.SourceMode)?Visibility.Hidden : Visibility.Visible));

       public Visibility EditorVisibility => _editorVisibility.Get();
        private readonly IProperty<Visibility> _editorVisibility = H.Property<Visibility>(c => c
           .On(e => e.EditMode)
           .On(e => e.SourceMode)
           .Set(e => (e.EditMode||!e.SourceMode)?Visibility.Visible : Visibility.Hidden));


        public Visibility SourceEditorVisibility => _sourceEditorVisibility.Get();
        private readonly IProperty<Visibility> _sourceEditorVisibility = H.Property<Visibility>(c => c
            .On(e => e.SourceMode)
            .Set(e => e.SourceMode ? Visibility.Visible : Visibility.Hidden)
        );

        public bool EditEnabled
        {
            get => _editEnabled.Get();
            set => _editEnabled.Set(value);
        }

        private readonly IProperty<bool> _editEnabled = H.Property<bool>();


        public bool EditMode
        {
            get => _editMode.Get();
            set => _editMode.Set(value);
        }

        private readonly IProperty<bool> _editMode = H.Property<bool>();


         public bool SourceMode
        {
            get => _sourceMode.Get();
            set => _sourceMode.Set(value);
        }

        private readonly IProperty<bool> _sourceMode = H.Property<bool>();


        [TriggerOn(nameof(Monograph))]
        [TriggerOn(nameof(SourceMode))]
        [TriggerOn(nameof(EditMode))]
        public void Update()
        {
             LoadDocument();
        }

        public void LoadDocument()
        {
            if (!_loadFromBinary) return;

            //if (!IsDocumentLinked) return;

            _saveChanges = false;

            SetDocumentFromBinary(Monograph.Document);

            _msg.Publish(this);

            _saveChanges = SourceMode || EditMode;

            EditEnabled = true;
        }

        public void SaveSource(string s)
        {
            _loadFromBinary = false;
            Monograph.Document = Encoding.ASCII.GetBytes(s);
            _loadFromBinary = true;
        }


        private static void ClearHighlight(FlowDocument doc)
        {
            foreach (var p in doc.Blocks.OfType<Paragraph>())
            {
                foreach (var span in p.Inlines.OfType<Span>())
                {
                        span.ClearValue(TextElement.BackgroundProperty);
                }                
            }
        }

        private static IEnumerable<InlineUIContainer> FindImages(FlowDocument doc)
        {
            var blocks = doc.Blocks;
            for (var position = blocks.FirstBlock.ElementStart; position != null && position.CompareTo(blocks.LastBlock.ElementEnd) != 1; position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                var element = position.Parent as InlineUIContainer;
                if (element?.Child is Image)
                {
                    yield return element;
                }
            }
        }

        private static void RemoveImages(FlowDocument doc)
        {
            foreach (var image in FindImages(doc))
            {
                var inlines = image.SiblingInlines;

                inlines?.Remove(image);
            }
        }
        public FlowDocumentReader Reader
        {
            get => _reader.Get();
            set => _reader.Set(value);
        }
        private readonly IProperty<FlowDocumentReader> _reader = H.Property<FlowDocumentReader>();

        public RichTextBox Editor
        {
            get => _editor.Get();
            set => _editor.Set(value);
        }

        private readonly IProperty<RichTextBox> _editor = H.Property<RichTextBox>();


        public TextEditor SourceEditor
        {
            get => _sourceEditor.Get();
            set => _sourceEditor.Set(value);
        }

        private readonly IProperty<TextEditor> _sourceEditor = H.Property<TextEditor>();


        private void SetDocumentFromBinary(FlowDocument doc, byte[] binary, string format)
        {
            try
            {
                var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                var stream = new MemoryStream(binary);

                range.Load(stream, format);
            }
            catch (System.Windows.Markup.XamlParseException)
            {
                //range = new Paragraph{Inlines = { ex.Message}}
            }
            catch (Exception)
            {
                //return new FlowDocument();
            }
        }
        private FlowDocument GetDocumentFromBinary(byte[] binary)
        {
            try
            {
                if (binary == null)
                    return new FlowDocument();

                var stream = new MemoryStream(binary);
                return (FlowDocument)XamlReader.Load(stream);
            }

            catch (XamlParseException)
            { }
            catch (Exception)
            { }
            return new FlowDocument();
        }

        private static string GetSourceFromBinary(byte[] binary)
        {
            if (binary == null)
                return "";

            try
            {
                var stream = new MemoryStream(binary);

                var xml = new XmlDocument();
                xml.Load(stream);
                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };
                using (var writer = XmlWriter.Create(sb, settings))
                {
                    xml.Save(writer);
                }
                return sb.ToString();

            }
            catch (Exception)
            {
                using (var stream = new MemoryStream(binary))
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }


        }
        private void SetDocumentFromBinary(byte[] binary)
        {
            if(SourceEditor!=null)
                SourceEditor.Text = GetSourceFromBinary(binary);

            Document = GetDocumentFromBinary(binary);

            if(Editor!=null)
                Editor.Document = GetDocumentFromBinary(binary);
        }

        public bool Unlink(string anchorName, bool save = true)
        {
            var done = false;
            foreach (var span in FindSpans(Document, anchorName))
            {
                BindingOperations.ClearBinding(span, TextElement.BackgroundProperty);
                span.ClearValue(FrameworkContentElement.NameProperty);
                span.ClearValue(FrameworkContentElement.TagProperty);
                done = true;
            }

            if (!save || !done) return false;

            SaveDocument(Document);
            return true;
        }

        private static IEnumerable<Span> FindSpans(FlowDocument doc)
        {
            var blocks = doc.Blocks;

            if (blocks.Count <= 0) yield break;

            for (var position = blocks.FirstBlock.ElementStart;
                position != null && position.CompareTo(blocks.LastBlock.ElementEnd) != 1;
                position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.Parent is Span element)
                {
                    yield return element;
                }
            }
        }

        private static IEnumerable<Span> FindSpans(FlowDocument doc, string anchorId)
        {
            return FindSpans(doc).Where(s => s.Name == anchorId);
        }

        public Span Bind(string anchorId, object source, string path)
        {
            Span span = null;

            Reader?.Dispatcher.Invoke(() =>
            {
                foreach (var s in FindSpans(Document, anchorId))
                {
                    span = Bind(s, source, path);
                }
            });

            return span;
        }

        public Span Bind(Span span, object source, string path)
        {
            if (span == null) return null;

            Reader.Dispatcher.Invoke(() =>
            {
                var b = new Binding(path) { Source = source };
                span.SetBinding(TextElement.BackgroundProperty, b);
            });
            return span;
        }

        public Span Link(string anchorName, object source/*=null*/, string path/*="Background"*/)
        {
            var selection = Reader?.Selection;
            if (selection == null || selection.IsEmpty) return null;

            Unlink(anchorName, false);

            var span = new Span(selection.Start, selection.End)
            {
                Name = anchorName,
                Tag = "anchor"
            };

            Bind(span, source, path);

            SaveDocument(Document);

            return span;
        }

        private void RevoveSpan(Inline span)
        {
            var others = span.SiblingInlines;

            var parent = span.PreviousInline;

            var start = span.ContentStart;
            var end = span.ContentEnd;

            span?.SiblingInlines?.Remove(span);
        }

    }
}
