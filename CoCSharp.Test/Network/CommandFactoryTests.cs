using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Network
{
    [TestFixture]
    public class CommandFactoryTests
    {
        [Test]
        public void Constructor_Initialize()
        {
            CommandFactory.Initialize();
        }

        [Test]
        public void Create_UnknownCommand_Exception()
        {
            // Check if it returns throws NotSupportedException for unknown commands.
            Assert.Throws<NotSupportedException>(() => CommandFactory.Create(1337));
        }

        [Test]
        public void Create_KnownCommand_Type()
        {
            // Check if it returns the proper Type.
            var buyBuildingCommand = CommandFactory.Create(500);
            Assert.AreEqual(buyBuildingCommand.GetType(), typeof(BuyBuildingCommand));
        }

        [Test]
        public void Create_KnownCommand_NewInstances()
        {
            // Check if it returns a new instance for a known commands.
            var instance1 = CommandFactory.Create(500);
            var instance2 = CommandFactory.Create(500);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void TryCreate_UnknownCommand_ReturnFalse()
        {
            // Check if it returns false if it failed.
            var unknownCommand = (Command)null;
            Assert.False(CommandFactory.TryCreate(1337, out unknownCommand));
            Assert.AreEqual(default(Command), unknownCommand);
        }

        [Test]
        public void TryCreate_KnownCommand_ReturnTrue()
        {
            // Check if it returns true if it succeeded.
            var buyBuildingCommand = (Command)null;
            Assert.True(CommandFactory.TryCreate(500, out buyBuildingCommand));
            Assert.AreEqual(buyBuildingCommand.GetType(), typeof(BuyBuildingCommand));
        }

        [Test]
        public void TryCreate_KnownCommand_NewInstances()
        {
            // Check if it returns new instances.
            var instance1 = (Command)null;
            var instance2 = (Command)null;

            CommandFactory.TryCreate(500, out instance1);
            CommandFactory.TryCreate(500, out instance2);

            Assert.AreNotSame(instance1, instance2);
        }
    }
}
