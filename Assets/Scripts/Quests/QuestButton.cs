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
    [SerializeField] private int ButtonID;

    private void Awake() {
        questManager = FindObjectOfType<QuestManager>();
    }

    public void SetButton(int id) {
        try
        {
            ButtonID = id;
            Debug.Log("Setup Triggered");
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => ChangeQuest(ButtonID));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void ChangeQuest(int index) {
        questManager.ChangeQuest(index);
        Debug.Log("Quest changed " + index);
    }

}
