//using System.ComponentModel.DataAnnotations.Schema;
//using HLab.DependencyInjection.Annotations;
//using HLab.Erp.Data;
//using HLab.Notify.Annotations;
//using HLab.Notify.PropertyChanged;

//namespace HLab.Erp.Lims.Monographs.Data
//{
//    public partial class MonographSolution
//    {
//        [Import]
//        private readonly IDataService _db;

//        [NotMapped]
//        public Gradient Gradient
//        {
//            get => _gradient.Get();
//            set => _gradient.Set(value);
//        }

//        private readonly IProperty<Gradient> _gradient = H.Property<Gradient>(c => c
//            .On(e => e.QtyMode)
//            .Set(e => e.GetGradient(e.Gradient)));


//        private Gradient GetGradient(Gradient old)
//        {
//            if (old?.MonographSolutionId == Id) return old;
//            if (QtyMode != "g") return null;
//            if (Id < 0) return null;

//                var g = _db.FetchOne<Gradient>(e => e.MonographSolutionId == Id);

//                if (g == null)
//                {
//                    _db.Add<Gradient>(h =>
//                    {
//                        h.MonographSolutionId = Id;
//                    });

//                    _db.Add<GradientLine>(h =>
//                    {
//                        h.GradientId = g.Id;
//                        h.Ratio = 1;
//                        h.Time = 0;
//                    });

//                    _db.Add<GradientLine>(h =>
//                    {
//                        h.GradientId = g.Id;
//                        h.Ratio = 1;
//                        h.Time = 30;
//                    });

//                }
//                return g;
//            }
        
//    }
//}