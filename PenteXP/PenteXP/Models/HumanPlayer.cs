﻿using System;
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
            Name = name;
        }
    }
}