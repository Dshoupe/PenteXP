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

        private Colors playerColor;

        public Colors PlayerColor
        {
            get { return playerColor; }
            set
            {
                if (id == 1)
                {
                    playerColor = Colors.Black;
                }
                else
                {
                    playerColor = Colors.White;
                }
            }
        }

        private int captures;

        public int Captures
        {
            get { return captures; }
            set
            {
                captures += value;
            }
        }


        public override string ToString()
        {
            return $"{Name} - {PlayerColor}\nCaptures: {Captures}";
        }
    }
}