using GameOfLife.Data;
using GameOfLife.Models;
using GameOfLife.Services;
using Moq;

namespace GameOfLifeApi.Tests.Services
{
    public class GameOfLifeServiceTests
    {
        private readonly Mock<GameOfLifeRepository> _mockRepository;
        private readonly GameOfLifeService _service;

        public GameOfLifeServiceTests()
        {
            _mockRepository = new Mock<GameOfLifeRepository>(null);
            _service = new GameOfLifeService(_mockRepository.Object);
        }

        [Fact]
        public void AddBoard_ValidBoard_ReturnsBoardId()
        {
            // Arrange
            var board = new Board
            {
                Id = Guid.NewGuid(),
                Rows = 3,
                Columns = 3,
                State = new List<List<int>> { new List<int> { 0, 1, 0 }, new List<int> { 1, 0, 1 }, new List<int> { 0, 1, 0 } }
            };

            // Act
            var result = _service.AddBoard(board);

            // Assert
            Assert.Equal(board.Id, result);
        }

        [Fact]
        public void GetBoard_ValidId_ReturnsBoard()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            var board = new Board
            {
                Id = boardId,
                Rows = 3,
                Columns = 3,
                State = new List<List<int>> { new List<int> { 0, 1, 0 }, new List<int> { 1, 0, 1 }, new List<int> { 0, 1, 0 } }
            };
            _mockRepository.Setup(r => r.GetBoard(boardId)).Returns(board);

            // Act
            var result = _service.GetBoard(boardId);

            // Assert
            Assert.Equal(board, result);
        }

        [Fact]
        public void GetBoard_InvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetBoard(boardId)).Throws(new KeyNotFoundException());

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _service.GetBoard(boardId));
        }
    }
}
