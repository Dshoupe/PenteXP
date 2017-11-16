using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenteXP.Models
{
    public class AIPlayer : Player
    {
        protected AIPlayer()
        {
        }

        public override Turn TakeTurn()
        {
            throw new NotImplementedException();
        }
    }
}