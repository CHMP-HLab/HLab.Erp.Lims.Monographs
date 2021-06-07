using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<GradientLine>;
    public partial class GradientLine : Entity
    {
        public GradientLine() => H.Initialize(this);

        [Column]
        public string Comment
        {
            get => _comment.Get();
            set => _comment.Set(value);
        }

        private readonly IProperty<string> _comment = H.Property<string>(c => c.Default(""));




        [Column]
        public int? GradientId
        {
            get => _gradientId.Get();
            set => _gradientId.Set(value);
        }

        private readonly IProperty<int?> _gradientId = H.Property<int?>();

        [NotMapped]
        public Gradient Gradient
        {
            get => _gradient.Get();
            set => GradientId = value.Id;
        }

        private readonly IProperty<Gradient> _gradient = H.Property<Gradient>(c => c.Foreign(e => e.GradientId));


        [Column]
        public double? Time
        {
            get => _time.Get();
            set => _time.Set(value);
        }

        private readonly IProperty<double?> _time = H.Property<double?>(c => c.Default((double?)default));


        [Column]
        public double? Ratio
        {
            get => _ratio.Get();
            set => _ratio.Set(value);
        }

        private readonly IProperty<double?> _ratio = H.Property<double?>(c => c.Default((double?)default));
    }
}