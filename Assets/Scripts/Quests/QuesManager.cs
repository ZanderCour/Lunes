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
    [SerializeField] private int ActiveQuestID;
    [SerializeField] private float distance;
    public GameObject questButtonPrefab;
    public Transform questUIRect;
    

    private void Start()
    {
        AddQuest("Quest 1", 0, Quest.QuestType.story);
        AddQuest("Quest 2", 1, Quest.QuestType.main);
        AddQuest("Quest 3", 2, Quest.QuestType.extra);
        AddQuest("Quest 4", 3, Quest.QuestType.tutorial);
        AddQuest("Quest 5", 4, Quest.QuestType.story);
        AddQuest("Quest 6", 5, Quest.QuestType.main);
        AddQuest("Quest 7", 6, Quest.QuestType.extra);

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
    }

    void CheckPlayerPosition(Transform player) {
        distance = Vector3.Distance(questLocation, player.position);
        if(distance < 1) {
            if (quests.Count > 0) {
                if (quests[ActiveQuestID].Complete == false)
                {
                    CompleteQuest("QUEST COMPLETED! " + ActiveQuestID);
                }
                else
                {
                    Debug.Log("Quest Failed This quest has allready been completed");
                }
            }
        }
    }

    void CompleteQuest(string message) {
        quests[ActiveQuestID].Complete = true; 
        CheckQuest(ActiveQuestID);
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
                //Instantiate button
                var ButtonPrefab = Instantiate(questButtonPrefab, questUIRect.position, Quaternion.identity);
                ButtonPrefab.transform.parent = questUIRect.transform;
                ButtonPrefab.transform.localScale = Vector3.one;
                QuestButton UIButton = ButtonPrefab.GetComponent<QuestButton>();



                //Find quest name text
                TextMeshProUGUI questText = UIButton.buttonText[0];
                questText.text = quests[i].questName;

                //Find quest type text
                TextMeshProUGUI questTypeText = UIButton.buttonText[1];
                string type = quests[i].type.ToString();
                questTypeText.text = "" + type + " quest";



                //Set the quest id of the button via listener
                UIButton.SetButton(i);
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
            try
            {
                QuestObject[] Quests = FindObjectsOfType<QuestObject>();
                ActiveQuestID = index;
                questLocation = Quests[index].GetComponent<Transform>().position;
                Debug.Log("Active quest changed to : " + index);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            Debug.Log("Failed");
        }
    }

    public void AddQuest(string questName, int questID, Quest.QuestType type)
    {
        ActiveQuestID = questID;
        questLocation = Vector3.zero;
        QuestObject[] Quests = FindObjectsOfType<QuestObject>();
        for(int i = 0; i < Quests.Length; i++) {
            if (Quests[i].questID == questID)
            {
                questLocation = Quests[i].GetComponent<Transform>().position;
            }
        }

        //Setup information
        try
        {
            Quest newQuest = ScriptableObject.CreateInstance<Quest>();
            quests.Add(newQuest);
            newQuest.questName = questName;
            newQuest.goalLocation = questLocation;
            newQuest.type = type;
            
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


    }

    public void CompleteQuest()
    {
        if(quests.Count > 0) {
            if(quests[ActiveQuestID] != null) {
                quests[ActiveQuestID].Complete = true;
            }
        }
    }
}