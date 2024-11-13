using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Tetromino
    {
        public int[,] Shape { get; private set; }
        public int Rows => Shape.GetLength(0);
        public int Cols => Shape.GetLength(1);

        public Tetromino(int[,] shape)
        {
            Shape = shape;
        }

        public void RotateClockwise()
        {
            int[,] rotated = new int[Cols, Rows];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    rotated[j, Rows - i - 1] = Shape[i, j];
            Shape = rotated;
        }
    }

    public static class TetrominoFactory
    {
        private static readonly List<int[,]> Shapes = new List<int[,]>
    {
        new int[,] {{1, 1, 1, 1}}, // I
        new int[,] {{1, 1}, {1, 1}}, // O
        new int[,] {{0, 1, 0}, {1, 1, 1}}, // T
        new int[,] {{1, 1, 0}, {0, 1, 1}}, // S
        new int[,] {{0, 1, 1}, {1, 1, 0}}, // Z
        new int[,] {{1, 0, 0}, {1, 1, 1}}, // L
        new int[,] {{0, 0, 1}, {1, 1, 1}} // J
    };

        public static Tetromino GetRandomTetromino()
        {
            Random random = new Random();
            int index = random.Next(Shapes.Count);
            return new Tetromino(Shapes[index]);
        }
    }
}
