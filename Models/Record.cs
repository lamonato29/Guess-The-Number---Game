using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guess_The_Number___Game.Models
{
    public class Record
    {
        public int RecordId { get; set; }
        public string PlayerName { get; set; }
        public int Tries { get; set; }
        public TimeSpan Duration { get; set; }
        public string DifficultyName { get; set; }

        public Record(string playerName, int tries, TimeSpan duration, string difficultyName)
        {
            PlayerName = playerName;
            Tries = tries;
            Duration = duration;
            DifficultyName = difficultyName;
        }


    }
}
