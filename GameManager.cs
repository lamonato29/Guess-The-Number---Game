using Guess_The_Number___Game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guess_The_Number___Game
{
    public class GameManager
    {
        public int? PlayerQuantity { get; set; }
        public int? Number { get; set; }
        public List<Player>? Players { get; set; }
        private DateTime? StartDateTime { get; set; }
        private DateTime? EndDateTime { get; set; }
        private TimeSpan? Duration { get; set; }
        public string? WinnerName { get; set; }

        private bool GameOver = false;
        private int CurrentPlayerIndex;
        public GameManager() 
        {
        }
        /// <summary>
        /// Start the game.
        /// </summary>
        public void StartGame()
        {
            GameOver = false;
            CurrentPlayerIndex = 0;
            if(Players != null)
            Players.Clear();

            string introMessage = """
    Welcome to Guess The Number game.

    This game was developed by Bruno Lamonato, a student of production engineering, tech lover and programmer from Brazil!

    My name is Bill and I'm the Game Manager.

    To start playing, tell me how many players will play or enter "RECORDS" to check the biggest winners.
    """;




            Console.WriteLine(introMessage);
            var read = Console.ReadLine();
            if(read != null)
            {
                if(read.ToLower() == "records" || read.ToLower() == "record")
                {
                    WriteRecords();
                    RestartGame();
                }
                else
                {
                    RegisterPlayers(read);

                    var dif = new Difficulty(ChooseDifficulty());
                    Number = GetRandomNumber(dif.Id);
                    StartDateTime = DateTime.Now;
                    while (!GameOver)
                    {
                        Player currentPlayer = Players.ElementAt(CurrentPlayerIndex);
                        int guess = currentPlayer.TakeTurn(dif);
                        bool result = EvaluateGuess(guess);
                        if (result)
                        {
                            GameOver = true;
                            EndDateTime = DateTime.Now;
                            RegisterRecord(currentPlayer, currentPlayer.TriesInMatch, GetGameDuration(), dif.Name);
                            RestartGame();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong!");
                            Console.ForegroundColor = ConsoleColor.Black;
                            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
                        }

                    }
                }
            }

      
        }

        private void RestartGame()
        {
            Console.WriteLine("Play again? Type y/n");
            var c = Console.ReadLine();
            if (c == "y" | c == "yes")
            {
                StartGame();
            } 
        }

        private void RegisterRecord(Player currentPlayer, int tries, TimeSpan timeSpan, string difficultyName)
        {
            Console.WriteLine($"Congratulations, {currentPlayer.Name}! You won the game!");
            Record rec = new Record(currentPlayer.Name, tries, timeSpan, difficultyName);
            using(GameDbContext dbContext = new GameDbContext())
            {
                dbContext.Records.Add(rec);
                dbContext.SaveChanges();
            }
        }

        private void RegisterPlayers(string _numberOfPlayers)
        {
            if(Players != null)
            {
                Players.Clear();
            }
            else
            {
                Players = new List<Player>();
            }

            bool canPass = false;
            int numberOfPlayers = 0;
            while (!canPass)
            {
                bool check = int.TryParse(_numberOfPlayers, out numberOfPlayers);
                if (check)
                {
                    canPass = true;
                }
                else
                {
                    Console.WriteLine("Choose a valid option. Write the number of players or write \"records\" to see the records.");
                    _numberOfPlayers = String.Empty;
                    break;
                }

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    var id = i + 1;
                    Console.WriteLine($"Tell me the name of Player {id}");
                    var chosenName = Console.ReadLine();
                    while (string.IsNullOrEmpty(chosenName))
                    {
                        Console.WriteLine($"This name is invalid. Tell me the name of Player {i + 1}");
                        chosenName = Console.ReadLine();
                    }

                    Player player = new Player(id, chosenName);
                    Players.Add(player);
                }
            }
        }

        private bool EvaluateGuess(int guess)
        {
            return guess == Number;
        }
        /// <summary>
        /// This method write in Console the Highscores.
        /// </summary>
        private void WriteRecords()
        {
            using(GameDbContext db = new GameDbContext())
            {
                var records = db.Records.ToList();
                var recordsByDifficulty = records.GroupBy(r => r.DifficultyName);
                foreach (var difficultyGroup in recordsByDifficulty)
                {
                    Console.WriteLine($"Difficulty: {difficultyGroup.Key}\n");

                    // Sort records by number of tries and duration within each difficulty group
                    var sortedRecords = difficultyGroup.OrderBy(r => r.Tries).ThenBy(r => r.Duration);
                    int count = 1;
                    foreach (var record in sortedRecords.Take(10))
                    {
                        Console.WriteLine($"#{count++}: Player: {record.PlayerName}, Tries: {record.Tries}, Duration: {record.Duration}");
                    }

                    Console.WriteLine();
                }
            }
        }
        private TimeSpan GetGameDuration()
        {
            return (EndDateTime.GetValueOrDefault() - StartDateTime.GetValueOrDefault());
        }
        private int GetRandomNumber(int dif)
        {
            Difficulty difficulty = new(dif);
            Random random = new Random();
            int number = random.Next(0, difficulty.toRange + 1);
            return number;
        }

        public int ChooseDifficulty()
        {
            Console.Write("╔════════════════════════════════════════╗\n");
            Console.Write("║          ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Choose a Difficulty");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("           ║\n");
            Console.Write("║                                        ║\n");
            Console.Write("║   ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("1. Oracle     - Number range: 0-25");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("   ║\n");
            Console.Write("║   ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("2. Seer       - Number range: 0-100");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("  ║\n");
            Console.Write("║   ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("3. Nostradamus - Number range: 0-1000");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("║\n");
            Console.Write("╚════════════════════════════════════════╝\n\n\n");

            int option = -1;
            while (option != 1 && option != 2 && option != 3)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out option))
                {
                    if (option != 1 && option != 2 && option != 3)
                    {
                        Console.WriteLine("Choose one difficulty: 1, 2, or 3.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Choose one difficulty: 1, 2, or 3.");
                }
            }
            return option;
        }

    }
}
