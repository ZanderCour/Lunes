using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectManager : MonoBehaviour
{
    [SerializeField] private QuestObject[] quests;

    private void OnValidate() {
        if (quests.Length == 0) 
            return;

        for(int i = 0; i < quests.Length; i++) {
            quests[i].gameObject.name = "QuestObject : ID == " + "[" + i + "]";
            quests[i].questID = i;
        }
    }
}
