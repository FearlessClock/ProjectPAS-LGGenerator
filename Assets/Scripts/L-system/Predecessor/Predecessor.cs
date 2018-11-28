using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Predecessors
{
    public abstract class Predecessor : ScriptableObject
    {
        public abstract void SetPredecessor(char value);

        public abstract char GetPredecessor();

        public abstract bool CanRuleApply(char predecessor, char preLetter = ' ', char postLetter = ' ');
    }
}
