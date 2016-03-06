using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Networking
{
    [TestFixture]
    public class CommandFactoryTests
    {
        [Test]
        public void TestCommandFactoryCreate()
        {
            // Check if it returns the proper Type.
            var buyBuildingCommand = CommandFactory.Create(500);
            Assert.AreEqual(buyBuildingCommand.GetType(), typeof(BuyBuildingCommand));

            // Check if it returns throws NotSupportedException for unknown commands.
            Assert.Throws<NotSupportedException>(() => CommandFactory.Create(1337));

            // Check if it returns new instances.
            var instance1 = CommandFactory.Create(500);
            var instance2 = CommandFactory.Create(500);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void TestCommandFactoryTryCreate()
        {
            // Check if it returns true if it succeeded.
            var buyBuildingCommand = (Command)null;
            Assert.True(CommandFactory.TryCreate(500, out buyBuildingCommand));

            // Check if it returns false if it failed.
            var unknownCommand = (Command)null;
            Assert.False(CommandFactory.TryCreate(1337, out unknownCommand));

            // Check if it returns the proper Type.
            Assert.AreEqual(buyBuildingCommand.GetType(), typeof(BuyBuildingCommand));

            var instance1 = (Command)null;
            var instance2 = (Command)null;

            // Check if it returns new instances.
            CommandFactory.TryCreate(500, out instance1);
            CommandFactory.TryCreate(500, out instance2);

            Assert.AreNotSame(instance1, instance2);
        }
    }
}
