using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Health : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string id;

    [ContextMenu("Generate unique id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField]
    private int currentHealth = 100;

    public event Action<Health> OnHealthChange;
    public event Action OnDeath;

    public int MaxHealth = 100;
    public int CurrentHealth
    {
        get => currentHealth;
    }

    private bool isDead = false;
   

    private void Start()
    {
        OnHealthChange?.Invoke(this);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChange?.Invoke(this);

        if (CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Kill()
    {
        TakeDamage(CurrentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth = amount + CurrentHealth < MaxHealth ? CurrentHealth + amount : MaxHealth;
        OnHealthChange?.Invoke(this);
    }

    public void Save(GameData gameData)
    {
        gameData.Health[id] = CurrentHealth;
    }

    public void Load(GameData gameData)
    {
        if (gameData.Health.TryGetValue(id, out int health))
        {
            currentHealth = health;
        }
    }

}
