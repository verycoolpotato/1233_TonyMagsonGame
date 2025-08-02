
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UnityEditor.Events;

using UnityEngine.EventSystems;


[ExecuteInEditMode]
public class AssignEvents : MonoBehaviour
{
    private GameObject _loadGame;
    
    [SerializeField] Button ThisButton;
    [SerializeField] EventTrigger OnHover;
    public string LevelNumber;


    private void Awake()
    {
        //Automatically fills out the events on the ThisButton

        _loadGame = GameObject.Find("LoadGame");
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerEnter;
        OnHover.triggers.Add(entry);
        UnityEventTools.AddVoidPersistentListener(entry.callback, _loadGame.GetComponents<AudioSource>()[2].Play);

        Invoke("lateAssign", 0.1f);
        

    }
    //wait until the script has recieved its number by the level select grid script
    private void lateAssign()
    {
        MenuLoadFunction StartGameFunction = _loadGame.GetComponent<MenuLoadFunction>();
        UnityEventTools.AddStringPersistentListener(ThisButton.onClick, StartGameFunction.StartGame, "Level" + LevelNumber);

        //tell unity to save changes made by this script
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.EditorUtility.SetDirty(ThisButton);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }

}
#endif
