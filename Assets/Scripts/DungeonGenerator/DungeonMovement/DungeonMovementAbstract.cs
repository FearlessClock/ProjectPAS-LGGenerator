using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonMovement
{
    public abstract class DungeonMovementAbstract : ScriptableObject
    {
        public abstract void DungeonStep(Transform transform);
    }
}
