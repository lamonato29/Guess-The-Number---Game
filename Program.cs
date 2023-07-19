using Guess_The_Number___Game;
using System.Runtime.Versioning;

Console.BackgroundColor = ConsoleColor.White;
Console.ForegroundColor = ConsoleColor.Black;
SetConsoleBufferSize(Console.WindowWidth, Console.WindowHeight);

GameManager gameManager = new GameManager();
gameManager.StartGame();

void SetConsoleBufferSize(int width, int height)
{
    if (IsWindows())
    {
#pragma warning disable CA1416 // Validate platform compatibility
        Console.SetWindowSize(width, height);
        Console.SetBufferSize(width, height);
#pragma warning restore CA1416 // Validate platform compatibility
    }
}
bool IsWindows()
{
    PlatformID platform = Environment.OSVersion.Platform;
    return platform == PlatformID.Win32NT || platform == PlatformID.Win32Windows;
}