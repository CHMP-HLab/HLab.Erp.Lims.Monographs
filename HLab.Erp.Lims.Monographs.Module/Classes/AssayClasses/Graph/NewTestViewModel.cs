using System.Collections.Generic;
using System.Linq;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Graph
{
    internal class NewTestViewModel : NotifierBase
    {
        readonly IDataService _db;

        public NewTestViewModel(IDataService db)
        {
            _db = db;
        }

        public List<TestClass> TestList => _testList.Get();

        readonly IProperty<List<TestClass>> _testList = H<NewTestViewModel>.Property<List<TestClass>>( c => c
            .Set(async e => await e._db.FetchAsync<TestClass>().ToListAsync())
        );
    }
}
