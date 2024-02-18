using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public GameObject MessagePrefab;
    public Transform ConsoleRect;
    public ScrollRect scrollRect;


    private void Start()
    {
        WriteMessage("Hello world");
    }


    public void WriteMessage(string message)
    {
        var ButtonPrefab = Instantiate(MessagePrefab, ConsoleRect.position, Quaternion.identity);
        ButtonPrefab.transform.SetParent(ConsoleRect.transform);
        ButtonPrefab.transform.localScale = Vector3.one;

        TextMeshProUGUI MessageText = ButtonPrefab.GetComponent<TextMeshProUGUI>();
        System.DateTime realTime = System.DateTime.Now;
        string formattedTime = realTime.ToString("HH:mm:ss");

        MessageText.text = "[" + formattedTime + "]" + " : " + message;
        Debug.Log(message);
        scrollRect.verticalNormalizedPosition = 0;
    }

}
