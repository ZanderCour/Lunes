using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public List<Quest> QuestDatabase = new List<Quest>();

    public List<Quest> quests = new List<Quest>();
    [SerializeField] private Transform player;
    Vector3 questLocation;
    [SerializeField] private int ActiveQuestID;
    [SerializeField] private float distance;
    public GameObject questButtonPrefab;
    public Transform questUIRect;
    public TextMeshProUGUI QuestInfo;
<<<<<<< HEAD:Assets/Scripts/Quests/QuestManager.cs
    public List<Button> questButtons = new List<Button>();
    [SerializeField] private bool IsClose;
    [HideInInspector] bool calledFirstTime;
    public int viewedQuest;

=======
    
>>>>>>> parent of 1783e75 (Character og oprydet i filer):Assets/Scripts/Quests/QuesManager.cs

    private void Start()
    {
        AddQuest(0);
        AddQuest(1);
<<<<<<< HEAD:Assets/Scripts/Quests/QuestManager.cs
        StartCoroutine(Setup());
    }

    private IEnumerator Setup() {
        yield return new WaitForSeconds(1f);
        UpdateUI();
=======

        if (quests.Count > 0)
        {
            HandleUI();
        }
>>>>>>> parent of 1783e75 (Character og oprydet i filer):Assets/Scripts/Quests/QuesManager.cs
    }

    private void Update()
    {
        if (quests.Count > 0)
        {
            CheckPlayerPosition(player);
            HandleButtons();

        }
    }

    public void UpdateUI()
    {
        HandleUI();
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

    private void HandleButtons()
    {
        try
        {
            Button[] buttons = GameObject.FindWithTag("QButton").GetComponents<Button>();
            for (int i = 0; i < quests.Count; i++)
            {
                if (quests[i].Complete == true)
                {
                    buttons[i].interactable = false;
                }
                else
                {
                    buttons[i].interactable = true;
                }
            }
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
        ClearUI();
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

    public void UpdateQuestInformation(int id)
    {
        QuestInfo.text = quests[id].QuestInformation;
    }

    public void ChangeQuest(int index)
    {
<<<<<<< HEAD:Assets/Scripts/Quests/QuestManager.cs
        try
=======
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
>>>>>>> parent of 1783e75 (Character og oprydet i filer):Assets/Scripts/Quests/QuesManager.cs
        {
            ActiveQuestID = index;

            Debug.Log("Active quest changed to : " + index);
            QuestObject[] Quests = FindObjectsOfType<QuestObject>();
            questLocation = Quests[index].GetComponent<Transform>().position;
            calledFirstTime = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void AddQuest(int questID)
    {
        try
        {
            for(int i = 0; i < QuestDatabase.Count; i++)
            {
                if(QuestDatabase[i].QuestID == questID)
                {
                    quests.Add(QuestDatabase[i]);
<<<<<<< HEAD:Assets/Scripts/Quests/QuestManager.cs
                    quests[i].Complete = false;
                    ChangeQuest(i);
=======
>>>>>>> parent of 1783e75 (Character og oprydet i filer):Assets/Scripts/Quests/QuesManager.cs
                    ActiveQuestID = i;
                }
            }

            //QuestLocation
            QuestObject[] Quests = FindObjectsOfType<QuestObject>();
            for (int i = 0; i < Quests.Length; i++)
            {
                if (Quests[i].questID == questID)
                {
                    quests[questID].goalLocation = Quests[i].GetComponent<Transform>().position;
                }
            }
        }
        catch(Exception e)
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