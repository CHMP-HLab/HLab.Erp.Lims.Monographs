using System.Collections.ObjectModel;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;
using HLab.Mvvm.Flowchart.Models;
using HLab.Mvvm.Flowchart.ViewModel;

namespace HLab.Erp.Lims.Monographs.Module.Graph
{
    public class MonographGraphEditorViewModel : ViewModel<MonographGraph>, IGraphViewModel, IMvvmContextProvider
    {
        public IMvvmService MvvmService { get; }
        public IGraphService GraphService { get; }

        public MonographGraphEditorViewModel(IMvvmService mvvmService, IGraphService graphService)
        {
            MvvmService = mvvmService;
            GraphService = graphService;
        }

        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
            ctx.AddCreator<IBlockViewModel>(vm => vm.GraphViewModel = this);
        }

        public ObservableCollection<IGraphBlock> Blocks => Model.Blocks;

        public void Select(IGraphBlock s)
        {
            var svm = MvvmContext.GetLinked<ViewModeDefault>(s) as BlockViewModel;

            foreach (var block in Blocks)
            {
                var vm = MvvmContext.GetLinked<ViewModeDefault>(block) as BlockViewModel;
                if(!ReferenceEquals(vm,svm)) vm.Selected = false;
            }

            svm.Selected = true;
        }

    }
}
