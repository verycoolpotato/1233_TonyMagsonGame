using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectGrid : MonoBehaviour
{
    [SerializeField] private GameObject ButtonPrefab;
    [SerializeField] private Transform GridParent;
    [SerializeField] private List<string> LevelNames = new();

#if UNITY_EDITOR
    public void GenerateButtons()
    {
        if (!ButtonPrefab || !GridParent) return;

        // Clear existing buttons
        for (int i = GridParent.childCount - 1; i >= 0; i--)
        {
            
            Object.DestroyImmediate(GridParent.GetChild(i).gameObject);
        }

        // Create new buttons
        for (int i = 0; i < LevelNames.Count; i++)
        {
            GameObject button = UnityEditor.PrefabUtility.InstantiatePrefab(ButtonPrefab) as GameObject;
            if (button != null)
            {
                button.transform.SetParent(GridParent, false);
                button.name = $"Level {i + 1}";

                button.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();

                // Support Unity UI 
                Text text = button.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.text = LevelNames[i];
                }

                
            }
        }

        //tell unity to save changes made by this script
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }
#endif
}

