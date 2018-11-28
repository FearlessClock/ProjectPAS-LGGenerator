using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonRemovalRules
{
    [CreateAssetMenu(fileName = "New dig rule", menuName = "Dungeon Removal/Dig spaces")]
    
    public class DigSpaceRule : ScriptableObject
    {
        public int sizeX = 1;
        public int sizeY = 1;

        private int createdXSize = 0;
        private int createdYSize = 0;

        public TileBase replacementTile;

        public bool[] dug = new bool[0]; 

        public DigSpaceRule()
        {
            //if(dug == null)
            //{
            //    Debug.Log("DigSpaceRule Constructor Array rebuild");
            //    SetArray(sizeX, sizeY);
            //}
        }

        public bool CheckIfSizeHasToChange()
        {
            if(createdXSize != sizeX || createdYSize != sizeY)
            {
                return true;
            }
            return false;
        }

        public void SetArray(int sX, int sY)
        {
            dug = new bool[sY * sX];
            createdYSize = sY;
            createdXSize = sX;

        }

        public bool GetAtPosition(int x, int y)
        {
            return dug[x + y * sizeX];
        }
        public void SetAtPosition(int x, int y, bool value)
        {
            dug[x + y * sizeX] = value;
        }

    }
}
