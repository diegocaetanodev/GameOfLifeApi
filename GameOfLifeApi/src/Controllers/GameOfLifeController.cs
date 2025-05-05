using GameOfLife.Models;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GameOfLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameOfLifeController : ControllerBase
    {
        private readonly GameOfLifeService _service;

        public GameOfLifeController(GameOfLifeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add a new board.
        /// </summary>
        /// <param name="board">The board object.</param>
        /// <returns>The ID of the created board.</returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult AddBoard([FromBody] Board board)
        {
            try
            {
                if (board == null || board.Rows <= 0 || board.Columns <= 0 || board.State == null)
                {
                    return BadRequest("Invalid board data. Ensure all fields are properly filled.");
                }

                var id = _service.AddBoard(board);
                return Created("", new { BoardId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the board: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a board by ID.
        /// </summary>
        /// <param name="boardId">The ID of the board.</param>
        /// <returns>The corresponding board.</returns>
        [HttpGet("{boardId}")]
        [ProducesResponseType(typeof(Board), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetBoardById(Guid boardId)
        {
            try
            {
                var board = _service.GetBoard(boardId);
                return Ok(board);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Calculate the next state of the board.
        /// </summary>
        /// <param name="boardId">The ID of the board.</param>
        /// <returns>The next state of the board.</returns>
        [HttpGet("{boardId}/next")]
        [ProducesResponseType(typeof(Board), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetNextState(Guid boardId)
        {
            try
            {
                var board = _service.GetNextState(boardId);
                return Ok(board);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Calculate the state of the board after a specific number of steps.
        /// </summary>
        /// <param name="boardId">The ID of the board.</param>
        /// <param name="steps">The number of steps.</param>
        /// <returns>The state of the board after the steps.</returns>
        [HttpGet("{boardId}/steps/{steps}")]
        [ProducesResponseType(typeof(Board), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetStateAfterXSteps(Guid boardId, int steps)
        {
            try
            {
                var board = _service.GetStateAfterXSteps(boardId, steps);
                return Ok(board);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Calculates the final state of the board after a maximum number of attempts.
        /// </summary>
        /// <param name="boardId">The ID of the board.</param>
        /// <param name="maxAttempts">The maximum number of attempts.</param>
        /// <returns>The final state of the board.</returns>
        [HttpGet("{boardId}/final/{maxAttempts}")]
        [ProducesResponseType(typeof(Board), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetFinalState(Guid boardId, int maxAttempts)
        {
            try
            {
                var board = _service.GetFinalState(boardId, maxAttempts);
                return Ok(board);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}