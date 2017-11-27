using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenteXP.Models
{
    [Serializable]
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name)
        {
            if (id == 1)
            {
                PlayerColor = Colors.Black;
            }
            else
            {
                PlayerColor = Colors.White;
            }
            Name = name;
        }
    }
}