using HLab.Erp.Data;
using HLab.Mvvm.Application;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<Supplier>;
    public partial class Supplier : Entity, IListableModel
    {
        public Supplier() => H.Initialize(this);

        public string Name
        {
            get => _name.Get();
            set => _name.Set(value);
        }

        private readonly IProperty<string> _name = H.Property<string>(c => c.Default(""));


        public string Url
        {
            get => _url.Get();
            set => _url.Set(value);
        }

        private readonly IProperty<string> _url = H.Property<string>(c => c.Default(""));

        public string SearchUrl
        {
            get => _searchUrl.Get();
            set => _searchUrl.Set(value);
        }

        private readonly IProperty<string> _searchUrl = H.Property<string>(c => c.Default(""));


        public string ReferenceUrl
        {
            get => _referenceUrl.Get();
            set => _referenceUrl.Set(value);
        }

        private readonly IProperty<string> _referenceUrl = H.Property<string>(c => c.Default(""));


        public string PriceRetriever
        {
            get => _priceRetriever.Get();
            set => _priceRetriever.Set(value);
        }

        private readonly IProperty<string> _priceRetriever = H.Property<string>(c => c.Default(""));


        public float Tax
        {
            get => _tax.Get();
            set => _tax.Set(value);
        }

        private readonly IProperty<float> _tax = H.Property<float>(c => c.Default(0.0f));

    }
}
