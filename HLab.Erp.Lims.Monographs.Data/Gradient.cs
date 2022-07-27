using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<Gradient>;
    public partial class Gradient : Entity
    {
        public Gradient() => H.Initialize(this);
 
        [Column]
        public double? FlowRate
        {
            get => _flowRate.Get();
            set => _flowRate.Set(value);
        }

        readonly IProperty<double?> _flowRate = H.Property<double?>(c => c.Default((double?)default));


        [Column]
        public double? LostVolume
        {
            get => _lostVolume.Get();
            set => _lostVolume.Set(value);
        }

        readonly IProperty<double?> _lostVolume = H.Property<double?>(c => c.Default((double?)default));


        [Column]
        public int? NbInjectionFirst
        {
            get => _nbInjectionFirst.Get();
            set => _nbInjectionFirst.Set(value);
        }

        readonly IProperty<int?> _nbInjectionFirst = H.Property<int?>(c => c.Default((int?)default));


        [Column]
        public int? NbInjectionNext
        {
            get => _nbInjectionNext.Get();
            set => _nbInjectionNext.Set(value);
        }

        readonly IProperty<int?> _nbInjectionNext = H.Property<int?>(c => c.Default((int?)default));



        [Column("MonographieSolutionId")]
        public int? MonographSolutionId
        {
            get => _monographSolutionId.Get();
            set => _monographSolutionId.Set(value);
        }

        readonly IProperty<int?> _monographSolutionId = H.Property<int?>();

        //[NotMapped]
        //public MonographSolution MonographSolution
        //{
        //    get => _monographSolution.Get();
        //    set => MonographSolutionId = value.Id;
        //}

        //private readonly IProperty<MonographSolution> _monographSolution = H.Property<MonographSolution>(c => c.Foreign(e => e.MonographSolutionId));

    }
}
