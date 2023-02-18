using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character = IsoTactics.Character;

namespace IsoTactics.Stats
{
    public class Health : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        public AudioClip hurtFX;
        public GameEvents onDeath;

        private void Start()
        {
            maxHealth = gameObject.GetComponent<Character>().vitality * 3;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            gameObject.GetComponentInChildren<CharacterAudioController>().audioController.clip = hurtFX;
            gameObject.GetComponentInChildren<CharacterAudioController>().audioController.Play();
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    
        private void Die()
        {
            var deathChar = gameObject.GetComponent<Character>();
            deathChar.State.ToDeathState();
            onDeath.Raise(this, deathChar);
        }
    }
}

