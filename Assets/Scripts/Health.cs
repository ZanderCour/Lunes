using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            NpcController npc = GetComponent<NpcController>();
            npc.Die();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }
}
