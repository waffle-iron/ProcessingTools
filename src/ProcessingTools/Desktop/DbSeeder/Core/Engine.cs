﻿namespace ProcessingTools.DbSeeder.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.DbSeeder.Contracts.Core;

    public class Engine : IEngine
    {
        private readonly ICommandRunner commandRunner;
        private readonly ISandbox sandbox;
        private readonly IHelpProvider helpProvider;

        public Engine(ICommandRunner commandRunner, ISandbox sandbox, IHelpProvider helpProvider)
        {
            if (commandRunner == null)
            {
                throw new ArgumentNullException(nameof(commandRunner));
            }

            if (sandbox == null)
            {
                throw new ArgumentNullException(nameof(sandbox));
            }

            this.commandRunner = commandRunner;
            this.sandbox = sandbox;
            this.helpProvider = helpProvider;
        }

        public async Task Run(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                await this.helpProvider?.GetHelp();
                return;
            }

            var tasks = new ConcurrentQueue<Task>();
            foreach (var arg in args)
            {
                tasks.Enqueue(this.sandbox.Run(action: () => this.commandRunner.Run(arg).Wait()));
            }

            await Task.WhenAll(tasks);
        }
    }
}
