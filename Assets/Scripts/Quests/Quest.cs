using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public Vector3 goalLocation;
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