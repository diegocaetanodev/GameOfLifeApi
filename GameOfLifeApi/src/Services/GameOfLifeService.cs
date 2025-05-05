using GameOfLife.Data;
using GameOfLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Services
{
    public class GameOfLifeService
    {
        private readonly Dictionary<Guid, Board> _boards = new();
        private readonly GameOfLifeRepository _repository;

        public GameOfLifeService(GameOfLifeRepository repository)
        {
            _repository = repository;
        }

        public Guid AddBoard(Board board)
        {
            _boards[board.Id] = board;

            _repository.AddBoard(board);

            return board.Id;
        }

        public Board GetBoard(Guid boardId)
        {
            return _repository.GetBoard(boardId);
        }

        public Board GetNextState(Guid boardId)
        {
            var currentBoard = _repository.GetBoard(boardId);
            if (currentBoard == null)
                throw new KeyNotFoundException("Board not found in the database.");

            var nextState = CalculateNextState(currentBoard.State);

            var newBoard = new Board
            {
                Id = currentBoard.Id,
                Rows = currentBoard.Rows,
                Columns = currentBoard.Columns,
                State = nextState
            };

            return newBoard;
        }

        public Board GetStateAfterXSteps(Guid boardId, int steps)
        {
            var currentBoard = _repository.GetBoard(boardId);
            if (currentBoard == null)
                throw new KeyNotFoundException("Board not found in the database.");

            var state = currentBoard.State;

            for (int i = 0; i < steps; i++)
            {
                state = CalculateNextState(state);
            }

            return new Board
            {
                Id = currentBoard.Id,
                Rows = currentBoard.Rows,
                Columns = currentBoard.Columns,
                State = state
            };
        }

        public Board GetFinalState(Guid boardId, int maxAttempts)
        {
            var currentBoard = _repository.GetBoard(boardId);
            if (currentBoard == null)
                throw new KeyNotFoundException("Board not found in the database.");

            var state = currentBoard.State;

            for (int i = 0; i < maxAttempts; i++)
            {
                var nextState = CalculateNextState(state);
                if (AreStatesEqual(state, nextState))
                {
                    return new Board
                    {
                        Id = currentBoard.Id,
                        Rows = currentBoard.Rows,
                        Columns = currentBoard.Columns,
                        State = nextState
                    };
                }

                state = nextState;
            }

            throw new InvalidOperationException("Board did not reach a final state within the maximum attempts.");
        }

        private List<List<int>> CalculateNextState(List<List<int>> state)
        {
            int rows = state.Count;
            int cols = state[0].Count;
            var nextState = CreateEmptyState(rows, cols);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int liveNeighbors = CountLiveNeighbors(state, row, col);
                    if (state[row][col] == 1)
                    {
                        nextState[row][col] = (liveNeighbors == 2 || liveNeighbors == 3) ? 1 : 0;
                    }
                    else
                    {
                        nextState[row][col] = (liveNeighbors == 3) ? 1 : 0;
                    }
                }
            }

            return nextState;
        }

        private int CountLiveNeighbors(List<List<int>> state, int row, int col)
        {
            int rows = state.Count;
            int cols = state[0].Count;
            int liveNeighbors = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int neighborRow = row + i;
                    int neighborCol = col + j;

                    if (neighborRow >= 0 && neighborRow < rows && neighborCol >= 0 && neighborCol < cols)
                    {
                        liveNeighbors += state[neighborRow][neighborCol];
                    }
                }
            }

            return liveNeighbors;
        }

        private bool AreStatesEqual(List<List<int>> state1, List<List<int>> state2)
        {
            int rows = state1.Count;
            int cols = state1[0].Count;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (state1[row][col] != state2[row][col])
                        return false;
                }
            }

            return true;
        }

        private List<List<int>> CreateEmptyState(int rows, int cols)
        {
            var state = new List<List<int>>();
            for (int i = 0; i < rows; i++)
            {
                state.Add(Enumerable.Repeat(0, cols).ToList());
            }
            return state;
        }
    }
}
