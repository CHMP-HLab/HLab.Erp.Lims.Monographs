using System.ComponentModel;
using HLab.Erp.Core.ViewModelStates;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    public interface IGraphViewModel : INotifyPropertyChanged
    {
        MonographGraphViewModel Root { get; set; }
        bool IsConnectable(string thisAnchorClass, IGraphViewModel model, string anchorClass);
        void ConnectTo(string srcClass, IGraphViewModel dstViewModel, string dstClass);
        State State { get; }
        void SetColor();
        int Order { get; }

        double Right { get; }
        double Width { get; }
    }
}