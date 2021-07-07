﻿using HLab.Erp.Core.Wpf.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Module.SampleTests;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Module.Products
{
    public class MonographListViewModel : EntityListViewModel<Monograph>, IMvvmContextProvider
    {
        public MonographListViewModel() : base(c => c
                //.AddAllowed()
                //.DeleteAllowed()
                .Column()
                .Header("{Name}")
                .Width(100)
                .Link(e => e.Caption)
                    .Filter()
                        
                .Column()
                .Header("{Pharmacopoeia}")
                .Width(300)
                    .Link(e => e.Pharmacopoeia)
                    .Filter()
                        .IconPath("Icons/Entities/Products/Dose")

                .Column()
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