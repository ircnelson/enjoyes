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
        public void Should_create_an_instance_of_InMemoryStreamStore()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();

            // Act
            var instance = factory.Create();

            // Assert
            instance.GetType().Should().Be(typeof(InMemoryStreamStore));
        }

        [Fact]
        public async Task Should_append_an_item_into_stream()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();

            var streamId = Guid.NewGuid().ToString();
            var content = Encoding.ASCII.GetBytes("Hello World");
            
            // Act
            using (var streamStore = factory.Create())
            {
                await streamStore.AppendStreamAsync(streamId, -1, CreateRecord("ItemCreated", content));
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

            var streamId = Guid.NewGuid().ToString();
            var content = Encoding.ASCII.GetBytes("Hello World");

            using (var streamStore = factory.Create())
            {
                await streamStore.AppendStreamAsync(streamId, ExpectedVersion.NoStream, CreateRecord("ItemCreated"));
                await streamStore.AppendStreamAsync(streamId, ExpectedVersion.Any, CreateRecord("ItemCreated"));
                await streamStore.AppendStreamAsync(streamId, ExpectedVersion.Any, CreateRecord("ItemCreated"));
            }

            using (var streamStore = factory.Create())
            {
                // Act
                Func<Task> func = async () => await streamStore.AppendStreamAsync(streamId, 1, CreateRecord("ItemCreated"));

                // Assert
                var assertion = func.ShouldThrowExactly<ConcurrencyException>();
                assertion.And.CurrentVersion.Should().Be(2);
                assertion.And.ExpectedVersion.Should().Be(1);
            }
        }
        
        [Fact]
        public async Task Should_append_itens_with_correct_version()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();
             
            var streamId = Guid.NewGuid().ToString();

            // Act
            using (var streamStore = factory.Create())
            {
                await streamStore.AppendStreamAsync(streamId, ExpectedVersion.NoStream, CreateRecord("ItemCreated"));
                await streamStore.AppendStreamAsync(streamId, ExpectedVersion.Any, CreateRecord("ItemChanged"));
                await streamStore.AppendStreamAsync(streamId, ExpectedVersion.Any, CreateRecord("ItemRemoved"));
            }

            // Assert
            using (var streamStore = factory.Create())
            {
                var streams = (await streamStore.GetStreamsAsync(streamId)).ToList();

                streams[0].Type.Should().Be("ItemCreated");
                streams[1].Type.Should().Be("ItemChanged");
                streams[2].Type.Should().Be("ItemRemoved");
            }
        }

        [Fact]
        public void When_concurrency_operation_Then_throws_exception()
        {
            // Arrange
            var factory = new InMemoryStreamStoreFactory();

            var streamId = Guid.NewGuid().ToString();
            var streamStore = factory.Create();

            // Act
            Action act = () => RunConcurrencyOperations(streamId, streamStore);

            // Assert
            var assertion = act.ShouldThrowExactly<ConcurrencyException>();

            assertion.And.CurrentVersion.Should().Be(0);
            assertion.And.ExpectedVersion.Should().Be(-1);
        }

        private static void RunConcurrencyOperations(string streamId, InMemoryStreamStore streamStore)
        {
            var t1 = streamStore.AppendStreamAsync(streamId, ExpectedVersion.NoStream, CreateRecord("ItemCreated"));
            var t2 = streamStore.AppendStreamAsync(streamId, ExpectedVersion.NoStream, CreateRecord("ItemCreated"));

            Task.WaitAll(t1, t2);
        }

        private static InMemoryStreamRecord CreateRecord(string type, byte[] content = null)
        {
            content = content ?? Encoding.ASCII.GetBytes("Hello World");

            return new InMemoryStreamRecord(Guid.NewGuid(), type, content);
        }
    }
}
