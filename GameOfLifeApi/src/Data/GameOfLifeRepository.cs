using GameOfLife.Models;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace GameOfLife.Data
{
    public class GameOfLifeRepository
    {
        private readonly IDatabase _database;

        public GameOfLifeRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public void AddBoard(Board board)
        {
            var json = JsonSerializer.Serialize(board);

            _database.StringSet(board.Id.ToString(), json);
        }

        public Board GetBoard(Guid boardId)
        {
            var json = _database.StringGet(boardId.ToString());

            if (string.IsNullOrEmpty(json))
                throw new KeyNotFoundException("Board not found in Redis.");

            return JsonSerializer.Deserialize<Board>(json);
        }
    }
}
