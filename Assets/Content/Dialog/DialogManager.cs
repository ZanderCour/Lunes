// DialogManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    private UIManager _UIManager;
    private QuestManager questManager;
    private bool DialogActive;
    public GameObject dialogMenu;
    private DialogSO Dialog;
    public int page;

    [Header("Interaction")]
    [SerializeField] private float reachDistance;
    public LayerMask NPC;
    public Camera cam;

    private void Start()
    {
        _UIManager = GetComponentInChildren<UIManager>();
        questManager = GetComponent<QuestManager>();
    }

    private void Update()
    {
        if (_UIManager.PauseMenuOpen)
            return;

        HandleInteraction();

        if (Input.GetKeyDown(KeyCode.F) && DialogActive) {
            DisplayNextPage();
            page = page + 1;
        }

        _UIManager.DialogMenuOpen = DialogActive;
    }

    private void HandleInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, reachDistance, NPC))
        {
            NpcController NPC = hit.collider.GetComponent<NpcController>();

            if (Input.GetKeyDown(KeyCode.E) && !DialogActive)
            {
                Dialog = NPC.NpcDialog;
                StartDialog(Dialog);
            }
        }

        Vector3 forward = cam.transform.TransformDirection(Vector3.forward) * reachDistance;
        Debug.DrawRay(cam.transform.position, forward, Color.green);
    }

    public void StartDialog(DialogSO dialog)
    {
        page = 0;
        DialogActive = true;
        _UIManager.OpenMenuSingle(dialogMenu);

        DisplayNextPage();
    }

    public void DisplayNextPage()
    {
        if (Dialog.dialogText.Length > 0 && page != Dialog.dialogText.Length)
        {
            string currentPage = Dialog.dialogText[page]; 
            dialogText.text = currentPage;
        }
        else
        {
            Debug.Log("No dialog to download");
        }

        if(page == Dialog.dialogText.Length)
        {
            EndDialog();
        }
    }

    private void EndDialog()
    {
        Debug.Log("Dialog Ended");
        _UIManager.CloseMenus();
        DialogActive = false;

        if (Dialog.addQuest)
        {
            try
            {
                Quest quest = questManager.QuestDatabase[Dialog.QuestIndex];
                questManager.quests.Add(quest);
                questManager.ChangeQuest(quest.QuestID);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }

        }
        
        page = 0;
    }
}
