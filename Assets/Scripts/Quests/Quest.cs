using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public Vector3 goalLocation;
    public Sprite questIndicator;
    public bool Complete = false;

    public enum QuestType
    {
        story,
        main,
        extra,
        tutorial
    };

    public QuestType type = new QuestType();
}