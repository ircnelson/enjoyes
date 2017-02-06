using EnjoyES.Defaults.Stores;
using EnjoyES.Exceptions;
using EnjoyES.Stores;
using FluentAssertions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EnjoyES.UnitTests
{
    public class InMemoryStreamStoreTests
    {
        [Fact]
        public Task Should_create_an_instance_of_InMemoryStreamStore()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();

            // Act
            var instance = factory.Create();

            // Assert
            instance.GetType().Should().Be(typeof(InMemoryStreamStore));

            return Task.CompletedTask;
        }

        [Fact]
        public async Task Should_append_an_item_into_stream()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();

            var streamId = Guid.NewGuid();
            var content = Encoding.ASCII.GetBytes("Hello World");
            
            // Act
            using (var streamStore = factory.Create())
            {
                await streamStore.AppendStreamAsync(streamId, "ItemCreated", -1, content);
            }

            // Assert
            using (var streamStore = factory.Create())
            {
                var streams = await streamStore.GetStreamsAsync(streamId);

                streams.Count().Should().Be(1);

                streams.First().Content.SequenceEqual(content);
            }
        }

        [Fact]
        public async Task Should_throw_exception_When_expected_version_is_different()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();

            var streamId = Guid.NewGuid();
            var content = Encoding.ASCII.GetBytes("Hello World");

            using (var streamStore = factory.Create())
            {
                await streamStore.AppendStreamAsync(streamId, "ItemCreated", ExpectedVersion.NoStream, content);
                await streamStore.AppendStreamAsync(streamId, "ItemCreated", ExpectedVersion.Any, content);
                await streamStore.AppendStreamAsync(streamId, "ItemCreated", ExpectedVersion.Any, content);
            }

            using (var streamStore = factory.Create())
            {
                // Act
                Func<Task> func = async () => await streamStore.AppendStreamAsync(streamId, "ItemCreated", 1, content);

                // Assert
                var assertion = func.ShouldThrowExactly<WrongExpectedVersionException>();
                assertion.And.CurrentVersion.Should().Be(2);
                assertion.And.ExpectedVersion.Should().Be(1);
            }
        }

        [Fact]
        public async Task Should_append_itens_with_correct_version()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();
             
            var streamId = Guid.NewGuid();
            var content = Encoding.ASCII.GetBytes("Hello World");

            // Act
            using (var streamStore = factory.Create())
            {
                await streamStore.AppendStreamAsync(streamId, "ItemCreated", ExpectedVersion.NoStream, content);
                await streamStore.AppendStreamAsync(streamId, "ItemCreated", ExpectedVersion.Any, content);
                await streamStore.AppendStreamAsync(streamId, "ItemCreated", ExpectedVersion.Any, content);
            }

            // Assert
            using (var streamStore = factory.Create())
            {
                var streams = (await streamStore.GetStreamsAsync(streamId)).ToList();

                streams[0].Version.Should().Be(0);
                streams[1].Version.Should().Be(1);
                streams[2].Version.Should().Be(2);
            }
        }
    }
}
