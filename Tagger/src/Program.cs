﻿namespace ProcessingTools.MainProgram
{
    using System;
    using System.Diagnostics;
    using Globals.Loggers;

    public class Program
    {
        public const int NumberOfExpandingIterations = 1;

        private static ILogger logger = new TextWriterLogger();

        public static void Main(string[] args)
        {
            Stopwatch mainTimer = new Stopwatch();
            mainTimer.Start();

            try
            {
                var settingsBuilder = new ProgramSettingsBuilder(logger, args);
                var settings = settingsBuilder.Settings;

                var singleFileProcessor = new SingleFileProcessor(settings, logger);

                singleFileProcessor.Run().Wait();
            }
            catch (Exception e)
            {
                logger.Log(e, string.Empty);
            }

            logger.Log(LogType.Info, "Main timer {0}.", mainTimer.Elapsed);
        }
    }
}