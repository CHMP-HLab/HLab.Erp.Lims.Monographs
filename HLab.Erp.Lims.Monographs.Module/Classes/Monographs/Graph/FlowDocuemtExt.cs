using System.Windows.Documents;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    public static class FlowDocuemtExt
    {
        public static bool InDocument(this FlowDocument doc, string text)
        {
            bool r = false;

            doc.Dispatcher.Invoke(() =>
            {
                var content = new TextRange(doc.ContentStart, doc.ContentEnd);
                r = content.Text.Contains(text);
            });

            return r;
        }

    }
}