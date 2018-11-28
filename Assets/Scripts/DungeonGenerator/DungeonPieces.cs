using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DungeonMovement;

namespace DungeonGenerator
{
    public abstract class DungeonPieces : ScriptableObject
    {
        public GameObject room;

        public DungeonMovementAbstract movement;

        [Tooltip("Letter that has to be changed")]
        [SerializeField]
        private char replacementLetter;
        public char ReplacementLetter { get { return replacementLetter;  } set { replacementLetter = value; } }

        //[Tooltip("Letter that has to exist before the rule letter")]
        //[SerializeField]
        //private char preLetter;
        //[Tooltip("Letter that has to be changed")]
        //[SerializeField]
        //private char replacementLetter;
        //[Tooltip("Letter that has to exist after the rule letter")]
        //[SerializeField]
        //private char postLetter;

        public abstract bool CanApplyRule(char current, char preLetter, char postLetter);
        //{
        //    if(replacementLetter.Equals(current) && this.preLetter.Equals(preLetter) && this.postLetter.Equals(postLetter))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

    }
}
