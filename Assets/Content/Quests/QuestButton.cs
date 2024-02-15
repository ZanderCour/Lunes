using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class QuestButton : MonoBehaviour
{
    private QuestManager questManager;
    public int ButtonID;
    public TextMeshProUGUI[] buttonText;
    int viewedQuest;
    public Button button;

    private void Awake() {
        questManager = FindObjectOfType<QuestManager>();
    }

    public void SetButton(int id) {
        try
        {
            ButtonID = id;
            Button button = GetComponent<Button>();
            Button TraceButton = GameObject.FindWithTag("Trace").GetComponent<Button>();

            //Update info -->
            button.onClick.AddListener(() => UpdateInfo(ButtonID));

            //Change quest -->
            TraceButton.onClick.AddListener(() => ChangeQuest());

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }



    public void ChangeQuest() {
        questManager.ChangeQuest(questManager.viewedQuest);
    }



    public void UpdateInfo(int index) {
        questManager.UpdateQuestInformation(index);
        viewedQuest = index;
        questManager.viewedQuest = viewedQuest;
        Debug.Log(viewedQuest);
    }

}
