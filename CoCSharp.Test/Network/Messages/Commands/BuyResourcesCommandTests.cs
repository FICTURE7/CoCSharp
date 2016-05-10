using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using NUnit.Framework;
using System.IO;

namespace CoCSharp.Test.Network.Messages.Commands
{
    [TestFixture]
    public class BuyResourcesCommandTests
    {
        [Test]
        public void TestBuyResourcesCommandDepth()
        {
            var command = CreateCommand(5000);

            var commandBytes = (byte[])null;
            using (var writer = new MessageWriter(new MemoryStream()))
            {
                command.WriteCommand(writer);
                commandBytes = ((MemoryStream)writer.BaseStream).ToArray();
            }

            var rCommand = new BuyResourcesCommand();
            using (var reader = new MessageReader(new MemoryStream(commandBytes)))
            {
                Assert.Throws<CommandException>(() => rCommand.ReadCommand(reader));
            }
        }

        public BuyResourcesCommand CreateCommand(int depth)
        {
            if (depth == 0)
                return null;

            depth--;
            if (depth != 0)
            {
                return new BuyResourcesCommand()
                {
                    EmbedCommand = true,
                    Command = CreateCommand(depth)
                };
            }
            else
            {
                return new BuyResourcesCommand()
                {
                    EmbedCommand = false,
                    Command = null
                };
            }
        }
    }
}
