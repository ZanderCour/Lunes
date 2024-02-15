using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog System/Dialog")]
public class DialogSO : ScriptableObject
{
    public string characterName;
    public string[] dialogText;

    public bool addQuest;
    public int AddQuestIndex;
    [Space(15)]
    public bool NpcCancompleteQuest;
    public int QuestIndex;
}