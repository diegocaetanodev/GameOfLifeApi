using GameOfLife.Controllers;
using GameOfLife.Models;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameOfLifeApi.Tests.Controllers
{
    public class GameOfLifeControllerTests
    {
        private readonly Mock<GameOfLifeService> _mockService;
        private readonly GameOfLifeController _controller;

        public GameOfLifeControllerTests()
        {
            _mockService = new Mock<GameOfLifeService>(null);
            _controller = new GameOfLifeController(_mockService.Object);
        }

        [Fact]
        public void AddBoard_ValidBoard_ReturnsCreatedResult()
        {
            // Arrange
            var board = new Board
            {
                Id = Guid.NewGuid(),
                Rows = 3,
                Columns = 3,
                State = new List<List<int>> { new List<int> { 0, 1, 0 }, new List<int> { 1, 0, 1 }, new List<int> { 0, 1, 0 } }
            };
            _mockService.Setup(s => s.AddBoard(board)).Returns(board.Id);

            // Act
            var result = _controller.AddBoard(board);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(board.Id, ((dynamic)createdResult.Value).BoardId);
        }

        [Fact]
        public void GetBoardById_ValidId_ReturnsOkResult()
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
            _mockService.Setup(s => s.GetBoard(boardId)).Returns(board);

            // Act
            var result = _controller.GetBoardById(boardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(board, okResult.Value);
        }

        [Fact]
        public void GetBoardById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            _mockService.Setup(s => s.GetBoard(boardId)).Throws(new KeyNotFoundException());

            // Act
            var result = _controller.GetBoardById(boardId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
