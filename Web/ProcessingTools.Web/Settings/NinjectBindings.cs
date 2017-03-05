﻿namespace ProcessingTools.Web.Settings
{
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using ProcessingTools.Constants;

    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind(configure =>
            {
                configure.FromAssembliesMatching(
                    $"{nameof(ProcessingTools)}.*.{FileConstants.DllFileExtension}")
                .SelectAllClasses()
                .BindDefaultInterface();
            });

            this.Bind<ProcessingTools.History.Data.Entity.Contracts.IHistoryDbContext>()
                .To<ProcessingTools.History.Data.Entity.HistoryDbContext>()
                .WhenInjectedInto<ProcessingTools.History.Data.Entity.Repositories.EntityHistoryRepository>()
                .InRequestScope();
        }
    }
}