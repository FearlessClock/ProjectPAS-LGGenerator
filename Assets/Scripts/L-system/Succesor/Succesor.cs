using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Succesors
{
    public abstract class Successor : ScriptableObject
    {
        public abstract void SetSuccesor(string value);
        public abstract String GetSuccesor();
    }
}
