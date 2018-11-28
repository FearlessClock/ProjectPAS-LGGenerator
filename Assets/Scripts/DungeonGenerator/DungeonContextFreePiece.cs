using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New Dungeon Piece", menuName = "Dungeon Generator/Dungeon Piece")]
    class DungeonContextFreePiece : DungeonPieces
    { 
        public override bool CanApplyRule(char current, char preLetter, char postLetter)
        {
            if (ReplacementLetter.Equals(current))
            {
                return true;
            }
            return false;
        }
    }
}
