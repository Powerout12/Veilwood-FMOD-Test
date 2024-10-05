using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/New Dialogue Container")]
public class DialogueText : ScriptableObject
{
    public string speakerName;
    public DialoguePath defaultPath;
    public DialoguePath[] paths;
    public int currentPath = -1; //-1 means default path

}

public enum Emotion
{
    //To dictate which audio is played alongside the dialogue
    Neutral,
    Happy,
    Sad,
    Angry,
    Confused,
    Shocked
}

[System.Serializable]
public class DialoguePath
{
    [TextArea(5,10)]
    public string[] paragraphs;
    public Emotion[] emotions;
}
