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
    public Transform Point;



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
        }

        _UIManager.DialogMenuOpen = DialogActive;
    }

    private void HandleInteraction()
    {
        Vector3 forward = Point.TransformDirection(Vector3.forward) * reachDistance;
        RaycastHit hit;
        if (Physics.Raycast(Point.position, Point.forward, out hit, reachDistance, NPC))
        {
            NpcController NPC = hit.collider.GetComponent<NpcController>();

            if (Input.GetKeyDown(KeyCode.E) && !DialogActive)
            {
                Dialog = NPC.NpcDialog;
                StartDialog(Dialog);
            }
        }
        
        Debug.DrawRay(Point.position, forward, Color.green);
    }

    public void StartDialog(DialogSO dialog)
    {
        page = 1;
        DialogActive = true;
        _UIManager.OpenMenuSingle(dialogMenu);
    }

    public void DisplayNextPage()
    {
        if (Dialog.dialogText.Length > 0)
        {
            page += 1;

            if (page == Dialog.dialogText.Length)
            {
                EndDialog();
            }
            else
            {
                string currentPage = Dialog.dialogText[page];
                dialogText.text = currentPage;
            }
        }
        else
        {
            Debug.Log("No dialog to download");
        }

    }

    private void EndDialog()
    {
        Debug.Log("Dialog Ended");
        _UIManager.CloseMenus();

        if (Dialog.addQuest && Dialog.QuestIndex == questManager.ActiveQuestID)
        {
            try
            {
                questManager.AddQuest(Dialog.AddQuestIndex);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }

        }

        if (Dialog.NpcCancompleteQuest && Dialog.QuestIndex == questManager.ActiveQuestID)
        {
            try
            {
                questManager.SendQuestCompletionRequest(Dialog.QuestIndex);
                Debug.Log("Npc request send");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


        DialogActive = false;
        page = 0;
    }
}
