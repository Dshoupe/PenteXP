using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenteXP.Models
{
    public abstract class Player
    {
        public static int id = 1;
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    name = $"Player {id++}";
                }
                else
                {
                    name = value;
                    id++;
                }
            }
        }

        public Colors PlayerColor { get; set; }
        public int Captures { get; set; }

        public abstract Turn TakeTurn();

        public override string ToString()
        {
            return $"{Name} - {PlayerColor}\nCaptures: {Captures}";
        }
    }
}