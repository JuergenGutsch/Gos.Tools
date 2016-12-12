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

        private void CheckAllPreconditions<TCommand>(TCommand command) where TCommand : ICommand
        {
            var results = new Collection<Exception>();
            foreach (var condition in _serviceProvider.GetServices<ICommandPrecondition<TCommand>>())
            {
                try
                {
                    condition.Check(command);
                }
                catch (Exception ex)
                {

                    results.Add(ex);
                }
            }

            if (results.Any())
            {
                throw new AggregateException(results);
            }
        }

        [DebuggerStepThrough]
        public async Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            _logger.LogDebug("Async dispatching command {0}", command);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await CheckAllPreconditionsAsync(command);

            var handlers = _serviceProvider.GetServices<IAsyncCommandHandler<TCommand>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(command).ConfigureAwait(false);
            }

            stopwatch.Stop();
            _logger.LogInformation("Async execution time for dispatching command {0}: {1}", command, stopwatch.Elapsed.ToString("g"));
        }

        private async Task CheckAllPreconditionsAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var results = new Collection<Exception>();
            foreach (var condition in _serviceProvider.GetServices<IAsyncCommandPrecondition<TCommand>>())
            {
                try
                {
                    await condition.CheckAsync(command);
                }
                catch (Exception ex)
                {
                    results.Add(ex);
                }
            }

            if (results.Any())
            {
                throw new AggregateException(results);
            }
        }
    }
}