using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonRemovalRules
{
    public abstract class AbstractDigRules : ScriptableObject
    {
        public abstract char GetReplacementLetter();
        public abstract DigSpaceRule GetReplacementSpaceRule();
        public abstract DungeonMovement.DungeonMovementAbstract GetDungeonMovement();
    }
}
