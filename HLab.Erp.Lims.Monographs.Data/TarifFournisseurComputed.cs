using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<SupplierPrice>;

    public partial class SupplierPrice
    {
        [NotMapped][Ignore]
        public double UnitPrice => _unitPrice.Get();

        readonly IProperty<double> _unitPrice = H.Property<double>(c => c
            .On(e => e.Cost)
            .On(e => e.Quantity)
            .On(e => e.Currency.Rate)
            .On(e => e.Unit.Abs)
            .Set(e => (double) e.Cost / (e.Quantity * e.Unit?.Abs ?? 1) *
                      (double) (e.Currency?.Rate ?? 1)));

    }
}
