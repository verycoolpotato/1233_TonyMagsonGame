using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSelectGrid))]
public class LevelGridBuilder : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelSelectGrid grid = (LevelSelectGrid)target;

        if (GUILayout.Button("Generate Level Buttons"))
        {
            grid.GenerateButtons();
        }
    }
}

