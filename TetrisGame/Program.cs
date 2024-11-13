using Tetris;

class Program
{
    static void Main(string[] args)
    {
        TetrisGame game = new TetrisGame(20, 10); // Размер игрового поля
        game.Run();
    }
}
