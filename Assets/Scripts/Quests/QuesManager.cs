using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

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
        AddQuest("Tutorial_0", 0);
        AddQuest("Tutorial_1", 1);
        

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

    private void HandleButtonId()
    {
        for(int i = 0; i < quests.Count; i++) {
            QuestButton[] buttons = FindObjectsOfType<QuestButton>();

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
                var ButtonPrefab = Instantiate(questButtonPrefab, questUIRect.position, Quaternion.identity);
                ButtonPrefab.transform.parent = questUIRect.transform;
                ButtonPrefab.transform.localScale = Vector3.one;
                TextMeshProUGUI questText = ButtonPrefab.GetComponentInChildren<TextMeshProUGUI>();
                QuestButton UIButton = ButtonPrefab.GetComponent<QuestButton>();
                UIButton.SetButton(i);
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