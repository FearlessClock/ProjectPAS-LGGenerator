using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonMovement
{
    [CreateAssetMenu(fileName = "New Dungeon movement", menuName = "Dungeon Generator/Dungeon movement/Forward")]
    class DungeonMoveForward : DungeonMovementAbstract
    {
        [Tooltip("How far forward the generator should go")]
        public float distance = 1;

        public override void DungeonStep(Transform transform)
        {
            transform.position += transform.up * distance;
        }
    }
}
