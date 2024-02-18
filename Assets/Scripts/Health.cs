using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private DebugConsole Console;

    public int maxHealth = 100;
    public int currentHealth;


    private void Start()
    {
        Console = FindObjectOfType<DebugConsole>();
        currentHealth = maxHealth;
    }

    public void Heal()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(currentHealth <= 0 && currentHealth != -999)
        {
            NpcController npc = GetComponent<NpcController>();
            npc.Die();
            Console.WriteMessage(this.gameObject.name + " Died");
            currentHealth = -999;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Console.WriteMessage(this.gameObject.name + " Took damage: " + amount);
    }
}
