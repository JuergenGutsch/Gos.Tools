using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using Gos.Tools.Cqs.Command;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Gos.Tools.Cqs.Tests
{
    public class CommandDispatcherTest
    {
        [Fact]
        public void CreateTheCommandDispatcher()
        {

        }

        [Fact]
        public void SimpleDispatching()
        {
            var commandHandlers = new List<ICommandHandler<ChangeUsersNameCommand>>{
                 new CommandHandler()
             };
            var commandPreconditions = new List<ICommandPrecondition<ChangeUsersNameCommand>>{
                 new CommandPrecondition()
             };

            var services = new ServiceCollection();
            services.AddScoped<ICommandHandler<ChangeUsersNameCommand>, CommandHandler>();
            services.AddScoped<ICommandPrecondition<ChangeUsersNameCommand>, CommandPrecondition>();

            var serviceProvider = services.BuildServiceProvider();

            var logger = new Mock<ILogger>();
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory
                .Setup(x => x.CreateLogger(It.IsAny<String>()))
                .Returns(logger.Object);

            var sut = new CommandDispatcher(loggerFactory.Object, serviceProvider);
            sut.DispatchCommand(new ChangeUsersNameCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Juergen"
            });
        }
    }

    public class ChangeUsersNameCommand : ICommand
    {
        public string Name { get; set; }
        public Guid UserId { get; internal set; }
    }

    public class CommandHandler : ICommandHandler<ChangeUsersNameCommand>
    {
        public void Handle(ChangeUsersNameCommand command) { }
    }

    public class CommandPrecondition : ICommandPrecondition<ChangeUsersNameCommand>
    {
        public void Check(ChangeUsersNameCommand command)
        {
            if (command.UserId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty");
            }
            if (String.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentNullException("Name cannot be null");
            }
        }
    }
}
