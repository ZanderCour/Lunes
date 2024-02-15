using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;

    [Space(5)]

    [Header("NPC controlled:")]
    [Header("It will be completed once you have talked to the NPC")]
    public bool NpcControlled;

    [Space(10)]

    [Header("LocationControlled:")]
    [Header("It will be completed once you reach its location")]
    public bool LocationControlled;

    [Space(10)]

    [Header("Quest rewards")]
    public bool recieveRewards;

    [Space(10)]

    //public Sprite questIndicator;
    public bool Complete = false;
    public string QuestInformation;
    public int QuestID;

    public enum QuestType
    {
        story,
        main,
        extra,
        tutorial
    };

    public QuestType type = new QuestType();
}