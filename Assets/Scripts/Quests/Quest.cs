using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public Vector3 goalLocation;
    public Sprite questIndicator; // For image indicator
    // Other relevant quest data or methods
}