using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameField
    {
        private int[,] field;
        public int Rows { get; }
        public int Cols { get; }

        public GameField(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            field = new int[rows, cols];
        }
        // Индексатор для доступа к элементам поля
        public int this[int row, int col]
        {
            get { return field[row, col]; }
            set { field[row, col] = value; }
        }
        // Метод для проверки и удаления заполненных рядов
        public void ClearFullRows()
        {
            for (int i = 0; i < Rows; i++)
            {
                bool isRowFull = true;
                for (int j = 0; j < Cols; j++)
                {
                    if (field[i, j] == 0)
                    {
                        isRowFull = false;
                        break;
                    }
                }

                if (isRowFull)
                {
                    // Удаление заполненной строки и сдвиг остальных вниз
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < Cols; j++)
                        {
                            field[k, j] = field[k - 1, j];
                        }
                    }

                    // Очистка верхней строки
                    for (int j = 0; j < Cols; j++)
                    {
                        field[0, j] = 0;
                    }

                    // Проверить текущую строку снова, так как она сдвинулась
                    i--;
                }
            }
        }
        public bool IsCellOccupied(int row, int col) => field[row, col] != 0;

        public void SetCell(int row, int col, int value)
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Cols)
                field[row, col] = value;
        }

        public bool CanPlaceTetromino(Tetromino tetromino, int row, int col)
        {
            for (int i = 0; i < tetromino.Rows; i++)
                for (int j = 0; j < tetromino.Cols; j++)
                    if (tetromino.Shape[i, j] == 1 &&
                        (row + i >= Rows || col + j < 0 || col + j >= Cols || IsCellOccupied(row + i, col + j)))
                        return false;
            return true;
        }

        public void PlaceTetromino(Tetromino tetromino, int row, int col)
        {
            for (int i = 0; i < tetromino.Rows; i++)
                for (int j = 0; j < tetromino.Cols; j++)
                    if (tetromino.Shape[i, j] == 1)
                        SetCell(row + i, col + j, 1);
        }

        public void Draw()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(field[i, j] == 0 ? '.' : '#');
                }
            }
        }


        public bool IsGameOver()
        {
            for (int col = 0; col < Cols; col++)
                if (field[0, col] != 0)
                    return true;
            return false;
        }
    }

}
