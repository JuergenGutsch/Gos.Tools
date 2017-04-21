using System;
using System.Collections.Generic;

namespace Gos.Tools.Cqs.Command
{
    public class CommandPreconditionCheckException : AggregateException
    {
        public CommandPreconditionCheckException(IEnumerable<CommandPreconditionCheckResult> results)
            :base(UnionExceptions(results))
        {
        }

        public static IEnumerable<Exception> UnionExceptions(IEnumerable<CommandPreconditionCheckResult> results)
        {
            var exceptions = new List<Exception>();
            foreach (var result in results)
            {
                exceptions.AddRange(result.Exceptions);
            }
            return exceptions;
        } 
    }
}