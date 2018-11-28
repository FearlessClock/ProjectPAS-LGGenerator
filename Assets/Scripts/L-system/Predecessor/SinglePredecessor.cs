using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Predecessors
{
    [CreateAssetMenu(fileName = "New Single predecessor", menuName = "L-system/Predecessor/Single Predecessor")]
    class SinglePredecessor : Predecessor
    {
        [SerializeField]
        private char predecessor;

        /// <summary>
        /// Does not check if the predecessor is correct, returns true always.
        /// </summary>
        /// <param name="predecessor">Letter that the l-system compares to to launch the rule </param>
        /// <param name="preLetter">Not used</param>
        /// <param name="postLetter">Not used</param>
        /// <returns></returns>
        public override bool CanRuleApply(char predecessor, char preLetter = ' ', char postLetter = ' ')
        {
            return true;
        }

        public override char GetPredecessor()
        {
            return predecessor;
        }

        public override void SetPredecessor(char value)
        {
            predecessor = value;
        }
    }
}
