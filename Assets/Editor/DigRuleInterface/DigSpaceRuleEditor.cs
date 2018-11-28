using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DungeonRemovalRules;
using UnityEditor;

[CustomEditor(typeof(DigSpaceRule))]
[CanEditMultipleObjects]
class DigSpaceRuleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DigSpaceRule digSpaceRule = (DigSpaceRule)target;
        if (GUI.changed && digSpaceRule.CheckIfSizeHasToChange())
        {
            Debug.Log("Reset");
            digSpaceRule.SetArray(digSpaceRule.sizeX, digSpaceRule.sizeY);
        }
        for (int i = 0; i < digSpaceRule.sizeY; i++)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(digSpaceRule.sizeX * 5));
            for(int j = 0; j < digSpaceRule.sizeX; j++)
            {
                digSpaceRule.SetAtPosition(j, i, EditorGUILayout.Toggle(digSpaceRule.GetAtPosition(j, i)));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Set dirty"))
        {
            SetAsDirty();
            Debug.Log("Dirty");
        }
    }

    public void SetAsDirty()
    {
        EditorUtility.SetDirty(target);
    }
    
}
