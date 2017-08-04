﻿namespace ProcessingTools.DbSeeder.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ProcessingTools.Contracts;
    using ProcessingTools.DbSeeder.Contracts.Providers;

    internal class CommandNamesProvider : ICommandNamesProvider
    {
        private const string CommandInterfacePattern = @"I(?<commandName>\w+)DbSeeder";

        private readonly ITypesProvider typesProvider;

        public CommandNamesProvider(ITypesProvider typesProvider)
        {
            this.typesProvider = typesProvider ?? throw new ArgumentNullException(nameof(typesProvider));
        }

        public IEnumerable<string> CommandNames
        {
            get
            {
                var re = new Regex(CommandInterfacePattern);
                var names = this.typesProvider.GetTypes()
                    .Select(t => re.Match(t.Name).Groups["commandName"].Value);

                return new HashSet<string>(names);
            }
        }
    }
}
