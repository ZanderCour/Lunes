using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    [SerializeField] private Transform player;
    Vector3 questLocation;
    int QuestID;
    [SerializeField] private float distance;

    private void Start()
    {
        AddQuest("Tutorial", 0);
    }

    private void Update()
    {
        if (quests.Count >= 1)
            CheckPlayerPosition(player);
    }

    void CheckPlayerPosition(Transform player) {
        distance = Vector3.Distance(questLocation, player.position);
        if(distance < 1) {
            CompleteQuest("QUEST COMPLETED!");
        }
    }

    void CompleteQuest(string message) {
        Debug.Log(message);
        if(quests.Count >= 1) {
            RemoveQuest(QuestID);
        }
        
    }

    public void AddQuest(string questName, int questID)
    {
        QuestID = questID;
        questLocation = Vector3.zero;
        QuestObject[] Quests = FindObjectsOfType<QuestObject>();
        for(int i = 0; i < Quests.Length; i++) {
            if (Quests[i].questID == questID)
            {
                questLocation = Quests[i].GetComponent<Transform>().position;
            }
        }

        Quest newQuest = ScriptableObject.CreateInstance<Quest>();
        quests.Add(newQuest);
        newQuest.questName = questName;
        newQuest.goalLocation = questLocation;
    }

    public void RemoveQuest(int id)
    {
        quests.Remove(quests[id]);
    }
}