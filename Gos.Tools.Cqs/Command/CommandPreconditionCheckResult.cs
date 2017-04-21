using System;
using System.Collections.Generic;
using System.Linq;

namespace Gos.Tools.Cqs.Command
{
    public class CommandPreconditionCheckResult
    {
        public CommandPreconditionCheckResult()
        {
            Exceptions = new List<Exception>();
        }

        public bool IsValid => Exceptions.Any();

        public IList<Exception> Exceptions { get; set; }
    }
}