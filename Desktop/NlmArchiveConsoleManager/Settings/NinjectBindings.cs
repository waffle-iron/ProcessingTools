﻿namespace ProcessingTools.NlmArchiveConsoleManager.Settings
{
    using System.Reflection;
    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Factory;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Modules;
    using ProcessingTools.Interceptors;
    using ProcessingTools.Loggers.Loggers;
    using ProcessingTools.NlmArchiveConsoleManager.Contracts.Factories;
    using ProcessingTools.Services.Data.Services.Files;

    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind(b =>
            {
                b.From(Assembly.GetExecutingAssembly())
                .SelectAllClasses()
                .BindDefaultInterface();
            });

            this.Bind(b =>
            {
                b.FromAssembliesMatching(
                    $"{nameof(ProcessingTools)}.*.dll")
                .SelectAllClasses()
                .BindDefaultInterface();
            });

            this.Bind<ProcessingTools.Contracts.IDocumentFactory>()
                .To<ProcessingTools.DocumentProvider.Factories.TaxPubDocumentFactory>()
                .InSingletonScope();

            this.Bind<ProcessingTools.Contracts.ILogger>()
                .To<ConsoleLogger>()
                .InSingletonScope();

            this.Bind<ProcessingTools.Contracts.Files.IO.IXmlFileReader>()
                .To<ProcessingTools.FileSystem.IO.XmlFileReader>()
                .WhenInjectedInto<XmlFileContentDataService>();

            this.Bind<ProcessingTools.Contracts.Files.IO.IXmlFileWriter>()
                .To<ProcessingTools.FileSystem.IO.XmlFileWriter>()
                .WhenInjectedInto<XmlFileContentDataService>()
                .Intercept()
                .With<FileExistsRaiseWarningInterceptor>();

            this.Bind<ProcessingTools.Contracts.IDeserializer>()
                .To<ProcessingTools.Serialization.Serializers.DataContractJsonDeserializer>()
                .InSingletonScope();

            this.Bind<IDirectoryProcessorFactory>()
                .ToFactory()
                .InSingletonScope();

            this.Bind<IFileProcessorFactory>()
                .ToFactory()
                .InSingletonScope();

            this.Bind<IModelFactory>()
                .ToFactory()
                .InSingletonScope();
        }
    }
}
