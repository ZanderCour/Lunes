using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    private DebugConsole Console;

    public List<Quest> QuestDatabase = new List<Quest>();

    public List<Quest> quests = new List<Quest>();
    [SerializeField] private Transform player;
    Vector3 questLocation;
    public int ActiveQuestID;
    private float distance;
    public GameObject questButtonPrefab;
    public Transform questUIRect;
    public TextMeshProUGUI QuestInfo;
    [SerializeField] private List<Button> questButtons = new List<Button>();
    private bool IsClose;
    private bool calledFirstTime;
    public int viewedQuest;


    private void Start()
    {
        Console = GetComponent<DebugConsole>();
        AddQuest(0);
    }

    private void Update()
    {
        if (quests.Count > 0)
        {
            CheckPlayerPosition(player);

        }
    }

    public void UpdateUI()
    {
        ClearUI();

        HandleUI();
        HandleButtons();
    }

    void CheckPlayerPosition(Transform player) {
        distance = Vector3.Distance(questLocation, player.position);

        if (quests[ActiveQuestID].LocationControlled)
        {
            if (distance < 1)
            {
                SendQuestCompletionRequest(ActiveQuestID);
            }
        }
    }

    public void SendQuestCompletionRequest(int questIndex)
    {
        if (quests.Count > 0)
        {
            if (!quests[questIndex].Complete)
            {
                CompleteQuest(questIndex);
            }
            else
            {
                Console.WriteMessage("Quest Failed This quest has allready been completed");
            }
        }
        else
        {
            Console.WriteMessage("Quest Failed This quest has not been loaded to the current active quests");
        }
    }

    private void CompleteQuest(int id) {
        quests[id].Complete = true;
        string message = "Quest completed " + id;
        Console.WriteMessage(message);
        QuestWaypoint waypointSystem = FindObjectOfType<QuestWaypoint>();
        waypointSystem.questTarget = null;
        //Add rewards
    }


    private void HandleButtons()
    {
        try
        {

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void ClearUI()
    {
        questButtons.Clear();
        QuestButton[] QuestButtonsScripts = FindObjectsOfType<QuestButton>();
        for (int i = 0; i < QuestButtonsScripts.Length; i++)
        {
            Destroy(QuestButtonsScripts[i].gameObject);

        }

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
                questButtons.Add(UIButton.button);

                //Find quest name text
                TextMeshProUGUI questText = UIButton.buttonText[0];
                questText.text = quests[i].questName;

                //Find quest type text
                TextMeshProUGUI questTypeText = UIButton.buttonText[1];
                string type = quests[i].type.ToString();
                questTypeText.text = "" + type + " quest";

                //Set the quest id of the button via listener
                UIButton.SetButton(i);

                if (quests[i].Complete)
                {
                    questButtons[i].interactable = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }



    }

    public void HandleUINotifications()
    {

    }

    public void UpdateQuestInformation(int id)
    {
        QuestInfo.text = quests[id].QuestInformation;
    }

    public void ChangeQuest(int index)
    {
        if (index <= quests.Count)
        {
            try
            {
                List<QuestObject> Quests = new List<QuestObject>();
                Quests.Clear();

                QuestObjectManager questObjectManager = FindObjectOfType<QuestObjectManager>();

                for(int i = 0; i < questObjectManager.quests.Count; i++) {
                    Quests.Add(questObjectManager.quests[i]);
                }

                ActiveQuestID = index;
                questLocation = Quests[ActiveQuestID].GetComponent<Transform>().position;
                Console.WriteMessage("Active quest changed to : " + index);



                //Set waypoint
                QuestWaypoint waypointSystem = FindObjectOfType<QuestWaypoint>();
                for(int i = 0; i < Quests.Count; i++)
                {
                    if(index == Quests[i].questID)
                    {
                        waypointSystem.questTarget = Quests[i].transform;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
       

    public void AddQuest(int questID)
    {
        try
        {
            for(int i = 0; i <= QuestDatabase.Count; i++)
            {
                if(QuestDatabase[i].QuestID == questID)
                {
                    quests.Add(QuestDatabase[i]);
                    //ChangeQuest(i);
                    quests[i].Complete = false;
                    Console.WriteMessage("Added quest " + ActiveQuestID);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}