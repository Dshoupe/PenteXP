using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenteXP.Models
{
    [Serializable]
    public class gameModel
    {
        public List<String> boardpieces = new List<String>();
        public List<Player> players = new List<Player>();
        public int lastTurn = 0;
    }
}
