using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guess_The_Number___Game.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TriesInMatch { get; set; }

        public Player(int id, string name, int tries = 0)
        {
            Id = id;
            Name = name;
            TriesInMatch = tries;
        }
        /// <summary>
        /// Start the turn and get the player's guess
        /// </summary>
        /// <returns>returns int value of the player's guess</returns>
        public int TakeTurn(Difficulty difficulty)
        {
            Console.WriteLine($"{Name}, it's your turn. What number from {difficulty.fromRange} to {difficulty.toRange} am I thinking of?");
            bool canPass = false;
            while(!canPass)
            {
                int guess = -1;
                string input = Console.ReadLine();
                if (!String.IsNullOrEmpty(input) && !(guess > difficulty.toRange && guess < difficulty.fromRange))
                {
                    int.TryParse(input, out guess);
                    canPass = true;
                    RegisterNewTry();
                    return guess;
                }
                else
                {
                    Console.WriteLine("Wrong format. Please, try to guess the number I'm thinking of. (Only Integer between 0 and 100 is accepted)");
                }
            }
            return -1;
        }
        /// <summary>
        /// Increase the player's tries in the match by 1.
        /// </summary>
        public void RegisterNewTry()
        {
            TriesInMatch = TriesInMatch + 1;
        }

    }
}
