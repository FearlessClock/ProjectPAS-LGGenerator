using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Succesors;
using Predecessors;

[CreateAssetMenu()]
public class ProductionRule : ScriptableObject
{
    public Predecessor predecessorObject;
    public Successor succesorObject;

    public string GetSuccesor()
    {
        return succesorObject.GetSuccesor();
    }

    public char GetPredecessor()
    {
        return predecessorObject.GetPredecessor();
    }

    public void SetSuccesor(Successor successor)
    {
        succesorObject = successor;
    }

    public void SetPredecessor(Predecessor predecessor)
    {
        predecessorObject = predecessor;
    }

    public string ApplyRule(char preDec, char preLetter = ' ', char postLetter = ' ')
    {
        if(predecessorObject.CanRuleApply(preDec, preLetter, postLetter))
        {
            return succesorObject.GetSuccesor();
        }
        else
        {
            return null;
        }
    }
}

