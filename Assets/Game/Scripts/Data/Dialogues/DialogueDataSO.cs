using UnityEngine;
using System;

[Serializable]
public struct DialogueLine
{
    public string speaker;
    [TextArea(3, 5)]
    public string line;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueDataSO", order = 1)]
public class DialogueDataSO : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}