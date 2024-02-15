using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestWaypoint : MonoBehaviour
{
    public GameObject container;
    public Transform player;
    public Image img;
    public Transform questTarget;
    public TextMeshProUGUI metersText;
    public Vector3 offset;

    private void Update()
    {
        container.SetActive(questTarget != null);

        if(questTarget != null)
        {

            float minX = img.GetPixelAdjustedRect().width / 10;
            float maxX = Screen.width - minX;

            float minY = img.GetPixelAdjustedRect().height / 10;
            float maxY = Screen.height - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(questTarget.position + offset);

            if (Vector3.Dot((questTarget.position - transform.position), transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            img.transform.position = pos;
            metersText.text = ((int)Vector3.Distance(questTarget.position, player.transform.position)).ToString() + "m";

            if(Vector3.Distance(questTarget.position, player.transform.position) < 1) {
                questTarget = null;
            }
        }
    }
}
