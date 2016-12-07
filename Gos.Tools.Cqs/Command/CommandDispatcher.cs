using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gos.Tools.Cqs.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public CommandDispatcher(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            _logger = loggerFactory.CreateLogger<CommandDispatcher>();
            _serviceProvider = serviceProvider;
        }

        [DebuggerStepThrough]
        public void DispatchCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            _logger.LogDebug("Dispatching command {0}", command);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            CheckAllPreconditions(command);

            var handlers = _serviceProvider.GetServices<ICommandHandler<TCommand>>();
            foreach (var handler in handlers)
            {
                handler.Handle(command);
            }

            stopwatch.Stop();
            _logger.LogInformation("Execution time for dispatching command {0}: {1}", command, stopwatch.Elapsed.ToString("g"));
        }

        [DebuggerStepThrough]
        public async Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            _logger.LogDebug("Async dispatching command {0}", command);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            CheckAllPreconditions(command);

            var handlers = _serviceProvider.GetServices<IAsyncCommandHandler<TCommand>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(command).ConfigureAwait(false);
            }

            stopwatch.Stop();
            _logger.LogInformation("Async execution time for dispatching command {0}: {1}", command, stopwatch.Elapsed.ToString("g"));
        }

        private void CheckAllPreconditions<TCommand>(TCommand command) where TCommand : ICommand
        {
            var results = new Collection<CommandPreconditionCheckResult>();
            foreach (var condition in _serviceProvider.GetServices<ICommandPrecondition<TCommand>>())
            {
                results.Add(condition.Check(command));
            }

            if (results.Any(t => !t.IsValid))
            {
                throw new CommandPreconditionCheckException(results.Where(t=> !t.IsValid));
            }
        }
    }
}