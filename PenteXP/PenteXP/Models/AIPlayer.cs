﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenteXP.Models
{
    [Serializable]
    public class AIPlayer : Player
    {
        public AIPlayer()
        {
            Name = "Computer";
            PlayerColor = Colors.White;
        }
    }
}