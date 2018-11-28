using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Predecessors
{
    [CreateAssetMenu(fileName = "New Context sensitive predecessor", menuName = "L-system/Predecessor/Context sensitive Predecessor")]
    class ContextSensitivePredecessor : Predecessor
    {
        [Tooltip("Letters that have to exist before the rule letter")]
        [SerializeField]
        private string preLetter;
        [Tooltip("Letter that has to be changed")]
        [SerializeField]
        private char predecessor;
        [Tooltip("letters that have to exist after the rule letter")]
        [SerializeField]
        private string postLetter;

        public override char GetPredecessor()
        {
            return predecessor;
            
        }

        public override bool CanRuleApply(char predec, char preLet = ' ', char postLet = ' ')
        {
            bool preCharFound = false;
            bool postCharFound = false;

            //Compare all the preLetters (Defined in inspector) with the current Pre letter
            foreach(char preChar in preLetter)
            {
                if(preChar.Equals(preLet))
                {
                    preCharFound = true;
                }
            }

            //Compare all the postLetters (Defined in inspector) with the current postLetter
            foreach (char postChar in postLetter)
            {
                if (postChar.Equals(postLet))
                {
                    postCharFound = true;
                }
            }

            //Check for wildcards in the letters 
            if ((preLetter.Equals("*") || preCharFound) 
                && (postLetter.Equals("*") || postCharFound) 
                && this.predecessor.Equals(predec))
            {
                return true; 
            }
            else
            {
                return false;
            }
        }

        public override void SetPredecessor(char value)
        {
            predecessor = value;
        }

        public void SetLetters(string pre, string post)
        {
            preLetter = pre;
            postLetter = post;
        }

        public override string ToString()
        {
            return "Context sen: " + preLetter + " < " + predecessor + " < " + postLetter;
        }
    }
}
