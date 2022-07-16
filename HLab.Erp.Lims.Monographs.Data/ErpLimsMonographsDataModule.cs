using HLab.Erp.Base.Data;
using HLab.Erp.Data;

namespace HLab.Erp.Lims.Monographs.Data
{
    public class ErpLimsMonographsDataModule : DataUpdaterModule
    {
        public ErpLimsMonographsDataModule(IDataService data) : base(data)
        { }

        protected override ISqlBuilder GetSqlUpdater(string version, ISqlBuilder builder)
        {
            switch (version)
            {
                case "0.0.0.0":
                    builder
                        .Table<Unit>().Create()

                        .Table<Currency>().Create()
                        .Table<ConsumableType>().Create()
                        .Table<Supplier>().Create()

                        .Table<SupplierPrice>().Create()
                        .Table<Consumable>().Create()
                        .Version("1.0.0.0")
                        ;
                    break;
            }
            return base.GetSqlUpdater(version, builder);
        }
    }
}
