using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenteXP.Models
{
    public abstract class Player
    {
        public string Name { get; set; }
        public Colors PlayerColor { get; set; }
        public int Captures { get; set; }

        public abstract Turn TakeTurn();

        public override string ToString()
        {
            return $"{Name} - {PlayerColor}";
        }
    }
}