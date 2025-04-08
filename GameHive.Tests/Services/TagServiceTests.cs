using GameHive.Core.Services;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameHive.Tests.Services
{
    [TestFixture]
    public class TagServiceTests
    {
        private Mock<ITagRepository> _mockTagRepository;
        private Mock<IGameTagRepository> _mockGameTagRepository;
        private TagService _service;

        [SetUp]
        public void Setup()
        {
            _mockTagRepository = new Mock<ITagRepository>();
            _mockGameTagRepository = new Mock<IGameTagRepository>();
            _service = new TagService(_mockTagRepository.Object, _mockGameTagRepository.Object);
        }

        [Test]
        public async Task AddAsync_CallsTagRepositoryAddAsync()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Action" };

            // Act
            await _service.AddAsync(tag);

            // Assert
            _mockTagRepository.Verify(r => r.AddAsync(tag), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_DeletesGameTagsFirst_ThenDeletesTag()
        {
            // Arrange
            int tagId = 1;
            var sequence = new MockSequence();

            // We want to verify that DeleteByTagIdAsync is called before DeleteAsync
            _mockGameTagRepository
                .InSequence(sequence)
                .Setup(r => r.DeleteByTagIdAsync(tagId))
                .Returns(Task.CompletedTask);

            _mockTagRepository
                .InSequence(sequence)
                .Setup(r => r.DeleteAsync(tagId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(tagId);

            // Assert
            _mockGameTagRepository.Verify(r => r.DeleteByTagIdAsync(tagId), Times.Once);
            _mockTagRepository.Verify(r => r.DeleteAsync(tagId), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ReturnsTagsFromRepository()
        {
            // Arrange
            var expectedTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Action" },
                new Tag { Id = 2, Name = "Adventure" }
            };
            _mockTagRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedTags);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedTags));
            _mockTagRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ReturnsEmptyList_WhenRepositoryReturnsNull()
        {
            // Arrange
            _mockTagRepository.Setup(r => r.GetAllAsync()).ReturnsAsync((List<Tag>)null);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
            _mockTagRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsTagFromRepository()
        {
            // Arrange
            int tagId = 1;
            var expectedTag = new Tag { Id = tagId, Name = "Action" };
            _mockTagRepository.Setup(r => r.GetByIdAsync(tagId)).ReturnsAsync(expectedTag);

            // Act
            var result = await _service.GetByIdAsync(tagId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedTag));
            _mockTagRepository.Verify(r => r.GetByIdAsync(tagId), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsTagRepositoryUpdateAsync()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Action" };

            // Act
            await _service.UpdateAsync(tag);

            // Assert
            _mockTagRepository.Verify(r => r.UpdateAsync(tag), Times.Once);
        }

        [Test]
        public async Task GetTagsByGameIdAsync_ReturnsTagsFromGameTagRepository()
        {
            // Arrange
            int gameId = 1;
            var expectedTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Action" },
                new Tag { Id = 3, Name = "RPG" }
            };
            _mockGameTagRepository.Setup(r => r.GetTagsByGameIdAsync(gameId)).ReturnsAsync(expectedTags);

            // Act
            var result = await _service.GetTagsByGameIdAsync(gameId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedTags));
            _mockGameTagRepository.Verify(r => r.GetTagsByGameIdAsync(gameId), Times.Once);
        }
    }
}