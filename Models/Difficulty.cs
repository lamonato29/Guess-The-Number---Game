using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guess_The_Number___Game.Models
{
    public class Difficulty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int fromRange = 0;
        public int toRange { get; set; }

        public Difficulty(int id)
        {
            Id = id;
            switch (id)
            {
                case 1:
                    Name = "Oracle";
                    toRange = 25;
                    break;
                case 2:
                    Name = "Seer";
                    toRange = 100;
                    break;
                case 3:
                    Name = "Nostradamus";
                    toRange = 1000;
                    break;
                default:
                    return;
            }
        }
    }
    
}
