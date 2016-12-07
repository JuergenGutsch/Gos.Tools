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
            var commandHandlers = new List<ICommandHandler<TestCommand>>{
                new CommandHandler()
            };
            var commandPreconditions = new List<ICommandPrecondition<TestCommand>>{
                new CommandPrecondition()
            };

            var logger = new Mock<ILogger>();
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory
                .Setup(x => x.CreateLogger(It.IsAny<String>()))
                .Returns(logger.Object);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t == typeof(ICommandHandler<TestCommand>))))
                .Returns(commandHandlers.Select(x => x));
            serviceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t == typeof(ICommandPrecondition<TestCommand>))))
                .Returns(commandPreconditions.Select(x => x));

            var sut = new CommandDispatcher(loggerFactory.Object, serviceProvider.Object);
            sut.DispatchCommand(new TestCommand());
        }
    }

    public class TestCommand : ICommand
    {
    }

    public class CommandHandler : ICommandHandler<TestCommand>
    {
        public void Handle(TestCommand command) { }
    }

    public class CommandPrecondition : ICommandPrecondition<TestCommand>
    {
        public CommandPreconditionCheckResult Check(TestCommand command)
        {
            return new CommandPreconditionCheckResult();
        }
    }
}
