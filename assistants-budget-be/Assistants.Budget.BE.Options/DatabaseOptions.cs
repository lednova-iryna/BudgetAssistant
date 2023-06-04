using System;
using Assistants.Extensions.Options;

namespace Assistants.Budget.BE.Options
{
    public class DatabaseOptions : BaseOptions
    {
        public override string SectionName => "Database";

        public string ConnectionString { get; set; }
        public string Name { get; set; }
    }
}

