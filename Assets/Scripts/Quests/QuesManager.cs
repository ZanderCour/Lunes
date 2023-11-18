using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    [SerializeField] private Transform player;
    Vector3 questLocation;
    [SerializeField] private int QuestID;
    [SerializeField] private float distance;
    [SerializeField] private int newActiveQuest;
    public GameObject questButtonPrefab;
    public Transform questUIRect;
    

    private void Start()
    {
        AddQuest("Tutorial", 0);
        AddQuest("haha lol", 1);
        AddQuest("soi dora", 3);
        AddQuest("spider boi", 4);
        AddQuest("bruh", 5);
        AddQuest("pepega", 6);



        if (quests.Count > 0)
        {
            HandleUI();
        }
    }

    private void Update()
    {
        if (quests.Count > 0)
        {
            CheckPlayerPosition(player);
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeQuest(newActiveQuest);
            Debug.Log("Tried to change quest");
        }
    }

    void CheckPlayerPosition(Transform player) {
        distance = Vector3.Distance(questLocation, player.position);
        if(distance < 1) {
            if (quests.Count > 0) {
                if (quests[QuestID].Complete == false)
                {
                    CompleteQuest("QUEST COMPLETED! " + QuestID);
                }
                else
                {
                    Debug.Log("Quest Failed This quest has allready been completed");
                }
            }
        }
    }

    void CompleteQuest(string message) {
        quests[QuestID].Complete = true; 
        CheckQuest(QuestID);
        Debug.Log(message);
    }



    void CheckQuest(int id)
    {
        if(id == 0)
        {

        }

        if(id == 1)
        {

        }

        Debug.Log("Quests checked sucsesfully");
    }


    public void HandleUI()
    {
        for(int i = 0; i < quests.Count; i++)
        {
            try 
            {
                var Button = Instantiate(questButtonPrefab, questUIRect.position, Quaternion.identity);
                Button.transform.parent = questUIRect.transform;
                Button.transform.localScale = Vector3.one;
                TextMeshProUGUI questText = Button.GetComponentInChildren<TextMeshProUGUI>();
                
                questText.text = quests[i].questName;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }
    }

    public void ChangeQuest(int index)
    {
        if(index < quests.Count) {
            QuestID = index;
        }
        else
        {
            Debug.Log("Failed");
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

    public void CompleteQuest()
    {
        if(quests.Count > 0) {
            if(quests[QuestID] != null) {
                quests[QuestID].Complete = true;
            }
        }
    }
}