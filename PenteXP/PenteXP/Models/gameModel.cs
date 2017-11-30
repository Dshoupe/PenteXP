using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenteXP.Models
{
    [Serializable]
    public class GameModel
    {
        public List<String> boardpieces = new List<String>();
        public List<Player> players = new List<Player>();
        public int lastTurn = 0;
        public int Player1LastPiece { get; set; }
        public int Player1SecondToLastPiece { get; set; }
        public List<int> Player2LastPiece { get; set; }
    }
}
