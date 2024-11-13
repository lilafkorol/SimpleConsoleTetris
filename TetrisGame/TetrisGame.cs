using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class TetrisGame
    {
        private GameField field;
        private Tetromino currentTetromino;
        private int currentRow;
        private int currentCol;
        private DateTime lastFallTime;
        private TimeSpan fallInterval = TimeSpan.FromMilliseconds(500); // Интервал падения фигур

        public TetrisGame(int rows, int cols)
        {
            field = new GameField(rows, cols);
            StartNewTetromino();
            lastFallTime = DateTime.Now;
        }

        private void StartNewTetromino()
        {
            currentTetromino = TetrominoFactory.GetRandomTetromino();
            currentRow = 0; // Начальная позиция на самом верху поля
            currentCol = field.Cols / 2 - currentTetromino.Cols / 2;
        }

        public void Run()
        {
            while (!field.IsGameOver())
            {
                if (DateTime.Now - lastFallTime > fallInterval)
                {
                    if (field.CanPlaceTetromino(currentTetromino, currentRow + 1, currentCol))
                    {
                        currentRow++;
                    }
                    else
                    {
                        // Фиксация фигуры на поле
                        field.PlaceTetromino(currentTetromino, currentRow, currentCol);

                        // Проверка и удаление заполненных рядов
                        field.ClearFullRows();

                        // Создание новой фигуры
                        StartNewTetromino();
                    }
                    lastFallTime = DateTime.Now;
                }

                HandleInput();
                Draw();
                System.Threading.Thread.Sleep(50);
            }

            Console.SetCursorPosition(0, field.Rows + 2);
            Console.WriteLine("Game Over! Press any key to restart.");
            Console.ReadKey();
            Run();
        }


        private void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (field.CanPlaceTetromino(currentTetromino, currentRow, currentCol - 1))
                            currentCol--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (field.CanPlaceTetromino(currentTetromino, currentRow, currentCol + 1))
                            currentCol++;
                        break;
                    case ConsoleKey.UpArrow:
                        currentTetromino.RotateClockwise();
                        if (!field.CanPlaceTetromino(currentTetromino, currentRow, currentCol))
                            currentTetromino.RotateClockwise(); // Отмена поворота при невозможности размещения
                        break;
                    case ConsoleKey.DownArrow:
                        if (field.CanPlaceTetromino(currentTetromino, currentRow + 1, currentCol))
                            currentRow++;
                        break;
                }
            }
        }
        private void Draw()
        {
            // Сначала отрисовка статичного поля
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < field.Rows; i++)
            {
                for (int j = 0; j < field.Cols; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(field[i, j] == 0 ? '.' : '#');
                }
            }

            // Затем отрисовка текущей падающей фигуры
            for (int i = 0; i < currentTetromino.Rows; i++)
            {
                for (int j = 0; j < currentTetromino.Cols; j++)
                {
                    if (currentTetromino.Shape[i, j] == 1) // Если блок текущей фигуры заполнен
                    {
                        int drawRow = currentRow + i;
                        int drawCol = currentCol + j;

                        // Убедимся, что координаты в пределах поля
                        if (drawRow >= 0 && drawRow < field.Rows && drawCol >= 0 && drawCol < field.Cols)
                        {
                            Console.SetCursorPosition(drawCol, drawRow);
                            Console.Write('#'); // Символ для отображения текущей фигуры
                        }
                    }
                }
            }
        }
    }
}
