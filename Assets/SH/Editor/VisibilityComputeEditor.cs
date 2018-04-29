using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisibilityCompute))]
public class VisibilityComputeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Compute static children"))
        {
            (target as VisibilityCompute).Compute();
        }
    }
}
