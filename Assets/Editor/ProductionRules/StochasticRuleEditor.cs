using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Succesors;
using UnityEngine;

[CustomEditor(typeof(StochasticSuccesor))]
[CanEditMultipleObjects]
public class StochasticRuleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StochasticSuccesor stochasticSuccesor = (StochasticSuccesor)target;
        base.OnInspectorGUI();
        string response = "";
        EditorGUILayout.LabelField("Amoint of choices for the rule: " + stochasticSuccesor.choices.Count);
        if (GUILayout.Button("Store values"))
        {
            if (!stochasticSuccesor.CalculateChances())
            {
                response = "There was an error, check the console";
            }
            else
            {
                response = "Done";
            }
            Debug.Log(response);
        }
    }
}