using System;

namespace GameOfLife.Models
{
    public class Board
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<List<int>> State { get; set; } // Matriz representando o estado do tabuleiro
    }
}
