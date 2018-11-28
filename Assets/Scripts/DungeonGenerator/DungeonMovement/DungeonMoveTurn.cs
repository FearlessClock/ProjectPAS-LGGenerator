using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonMovement
{
    [CreateAssetMenu(fileName = "New Dungeon movement", menuName = "Dungeon Generator/Dungeon movement/Turn")]

    class DungeonMoveTurn : DungeonMovementAbstract
    {
        [Tooltip("How much the generator should turn")]
        public float angle = 90;
        [Tooltip("How far forward the generator should go in the first direction")]
        public float forwardDistance = 1;
        [Tooltip("How far forward the generator should go in the second direction")]
        public float sideDistance = 1;
        public override void DungeonStep(Transform transform)
        {
            transform.position += transform.up * forwardDistance;
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle);
            transform.position += transform.up * sideDistance;
        }
    }
}
