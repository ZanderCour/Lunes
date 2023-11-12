using UnityEngine;
using UnityEngine.UI;

public class QuestObject : MonoBehaviour
{
    private Transform player;
    public int questID;

    private void Update()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
}