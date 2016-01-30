﻿namespace ProcessingTools.MainProgram.Tests.UnitTests
{
    using System;
    using System.Xml;

    using Controllers;
    using Moq;
    using NUnit.Framework;
    using ProcessingTools.Contracts;

    [TestFixture]
    public class TagReferencesControllerTests
    {
        private const string CallShouldThrowSystemAggregateExceptionMessage = "Call should throw System.AggregateException.";
        private const string InnerExceptionShouldBeArgumentNullExceptionMessage = "InnerException should be System.ArgumentNullException.";
        private const string ContentShouldBeUnchangedMessage = "Content should be unchaged.";

        private XmlDocument document;
        private XmlNamespaceManager namespaceManager;
        private ProgramSettings settings;
        private ILogger logger;

        [SetUp]
        public void Init()
        {
            this.document = new XmlDocument();
            this.document.LoadXml("<root />");

            this.namespaceManager = new XmlNamespaceManager(this.document.NameTable);
            this.settings = new ProgramSettings();

            var loggerMock = new Mock<ILogger>();
            this.logger = loggerMock.Object;
        }

        [Test]
        public void TagReferencesController_WithDefaultCnstructor_ShouldReturnValidObject()
        {
            var controller = new TagReferencesController();

            Assert.IsNotNull(controller, "Controller should not be null.");
        }

        [Test]
        public void TagReferencesController_RunWithValidParameters_ShouldWork()
        {
            var controller = new TagReferencesController();

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, this.logger).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagReferencesController();

            try
            {
                controller.Run(null, this.namespaceManager, this.settings, this.logger).Wait();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType(), CallShouldThrowSystemAggregateExceptionMessage);

                Exception innerException = e.InnerException;
                Assert.AreEqual(typeof(ArgumentNullException), innerException.GetType(), InnerExceptionShouldBeArgumentNullExceptionMessage);

                Assert.AreEqual("context", ((ArgumentNullException)innerException).ParamName, @"ParamName should be ""context"".");
            }
        }

        [Test]
        public void TagReferencesController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullNamespaceManagerAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagReferencesController();

            try
            {
                controller.Run(this.document.DocumentElement, null, this.settings, this.logger).Wait();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType(), CallShouldThrowSystemAggregateExceptionMessage);

                Exception innerException = e.InnerException;
                Assert.AreEqual(typeof(ArgumentNullException), innerException.GetType(), InnerExceptionShouldBeArgumentNullExceptionMessage);

                Assert.AreEqual("namespaceManager", ((ArgumentNullException)innerException).ParamName, @"ParamName should be ""namespaceManager"".");
            }
        }

        [Test]
        public void TagReferencesController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagReferencesController();

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagReferencesController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagReferencesController();

            try
            {
                controller.Run(this.document.DocumentElement, this.namespaceManager, null, this.logger).Wait();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType(), CallShouldThrowSystemAggregateExceptionMessage);

                Exception innerException = e.InnerException;
                Assert.AreEqual(typeof(ArgumentNullException), innerException.GetType(), InnerExceptionShouldBeArgumentNullExceptionMessage);

                Assert.AreEqual("settings", ((ArgumentNullException)innerException).ParamName, @"ParamName should be ""settings"".");
            }
        }

        [Test]
        public void TagReferencesController_RunWithNullLoggerAndValidOtherParameters_ShouldWork()
        {
            var controller = new TagReferencesController();

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, null).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }
    }
}