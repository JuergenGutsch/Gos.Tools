using System;
using System.Collections.Generic;
using System.Linq;
using Gos.Tools.Cqs.Command;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;

namespace Gos.Tools.Cqs.Tests
{
    [TestClass]
    public class CommandDispatcherTest
    {
        [TestMethod]
        public void CreateTheCommandDispatcher()
        {

        }

        [TestMethod]
        public void SimpleDispatching()
        {
            var commandHandlers = new List<ICommandHandler<ChangeUsersNameCommand>>{
                new CommandHandler()
            };
            var commandPreconditions = new List<ICommandPrecondition<ChangeUsersNameCommand>>{
                new CommandPrecondition()
            };

            var logger = new Mock<ILogger>();
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory
                .Setup(x => x.CreateLogger(It.IsAny<String>()))
                .Returns(logger.Object);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t == typeof(ICommandHandler<ChangeUsersNameCommand>))))
                .Returns(commandHandlers.Select(x => x));
            serviceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t == typeof(ICommandPrecondition<ChangeUsersNameCommand>))))
                .Returns(commandPreconditions.Select(x => x));

            var sut = new CommandDispatcher(loggerFactory.Object, serviceProvider.Object);
            sut.DispatchCommand(new ChangeUsersNameCommand());
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
