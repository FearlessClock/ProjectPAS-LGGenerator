using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Tilemaps;
using DungeonMovement;
using UnityEngine;

namespace DungeonRemovalRules
{
    [CreateAssetMenu(fileName = "New dig rule", menuName = "Dungeon Removal/Dig rules/Single dig rule")]
    class SingleDigRule : AbstractDigRules
    {
        //Letter that needs to be validated for the dig to be used
        public char replacementLetter;

        public DigSpaceRule replacementTile;

        public DungeonMovementAbstract movement;

        public override char GetReplacementLetter()
        {
            return replacementLetter;
        }

        public override DigSpaceRule GetReplacementSpaceRule()
        {
            return replacementTile;
        }

        public override DungeonMovementAbstract GetDungeonMovement()
        {
            return movement;
        }
    }
}
