using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectManager : MonoBehaviour
{
    public List<QuestObject> quests = new List<QuestObject>();

    private void OnValidate() {
        if (quests.Count == 0)
            return;

        for(int i = 0; i < quests.Count; i++) {
            quests[i].gameObject.name = "QuestObject : ID == " + "[" + i + "]";
            quests[i].questID = i;
        }  
    }
}
