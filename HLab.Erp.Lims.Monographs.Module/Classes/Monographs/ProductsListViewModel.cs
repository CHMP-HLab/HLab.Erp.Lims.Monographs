﻿using HLab.Erp.Core.Wpf.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Module.SampleTests;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Module.Products
{
    public class MonographListViewModel : Core.EntityLists.EntityListViewModel<Monograph>, IMvvmContextProvider
    {
        public MonographListViewModel(Injector i) : base(i, c => c
                //.AddAllowed()
                //.DeleteAllowed()
                .Column("Name")
                .Header("{Name}")
                .Width(100)
                .Link(e => e.Caption)
                    .Filter()
                        
                .Column("Pharmacopoeia")
                .Header("{Pharmacopoeia}")
                .Width(300)
                    .Link(e => e.Pharmacopoeia)
                    .Filter()
                        .IconPath("Icons/Entities/Products/Dose")

                .Column("Inn")
                    .Header("{Inn}")
                    .Width(200)
                    .Link(e => e.Inn)
                        .Filter()
                        .IconPath("Icons/Entities/Products/Inn")

                .FormColumn(e => e.Form)
        )

        {
        }

        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
        }
    }
}