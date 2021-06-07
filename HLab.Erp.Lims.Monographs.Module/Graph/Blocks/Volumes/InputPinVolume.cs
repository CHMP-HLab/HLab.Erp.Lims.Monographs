using System.Windows.Input;
using HLab.Mvvm;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Volumes
{
    using H = H<InputPinVolumesViewModel>;
    public class InputPinVolumesViewModel : ViewModel<InputPinVolumes>
    {
        public InputPinVolumesViewModel() => H.Initialize(this);

        public ICommand PlusCommand { get; } = H.Command(c => c
            .Action(
                (e, n) => e.Model.Volumes++)
        );


        public ICommand MinusCommand { get; } = H.Command(c => c
            .CanExecute(e => e.Model.Volumes >= 1.0)
            .Action(
                (e, n) => e.Model.Volumes--
                )
            .On(e => e.Model.Volumes).CheckCanExecute()
        );

        
    }
}