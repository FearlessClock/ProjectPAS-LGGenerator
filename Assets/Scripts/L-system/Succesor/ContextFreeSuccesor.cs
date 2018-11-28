using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Succesors
{
    [CreateAssetMenu(fileName = "New Sucessor", menuName = "L-system/Sucessor/Single Sucessor")]
    class ContextFreeSuccesor: Successor
    {
        public string successor = "";

        public override string GetSuccesor()
        {
            return successor;
        }

        public override void SetSuccesor(string value)
        {
            successor = value;
        }

        public override string ToString()
        {
            return "Single successor> " + successor;
        }
    }
}

