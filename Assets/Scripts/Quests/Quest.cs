using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public Vector3 goalLocation;
    public Sprite questIndicator;
    public bool Complete = false;
}