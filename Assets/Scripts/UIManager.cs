using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    public GameObject[] Menus;
    public bool PauseMenuOpen;
    public bool DialogMenuOpen;
    [SerializeField] private QuestManager questManager;

    public PlayerController player;

    public CinemachineBrain camController;

    private void Start()
    {
        questManager = GetComponent<QuestManager>();
    }

    private void Update()
    {
        if (DialogMenuOpen || !player.isGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!PauseMenuOpen) {
                PauseMenuOpen = true;
                Menus[0].SetActive(true);
            }
            else {
                PauseMenuOpen = false;
                CloseMenus();

            }
        }

        Cursor.visible = true ? PauseMenuOpen : Cursor.visible = false;

        if (PauseMenuOpen) {
            Cursor.lockState = CursorLockMode.None;
            camController.enabled = false;
            player.CanControll = false;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            player.CanControll = true;
            camController.enabled = true;
        }
    }

    public void OpenMenuSingle(GameObject menu)
    {
        for(int i = 0; i < Menus.Length; i++)
        {
            Menus[i].SetActive(false);
        }

        menu.SetActive(true);
        if (menu.name == "QuestMenu") 
        {
            Debug.Log("Quest menu opend");
            questManager.UpdateUI();
        }
    }

    public void OpenMenuOverlay(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void CloseMenus()
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            Menus[i].SetActive(false);
            PauseMenuOpen = false;
        }
    }
}
