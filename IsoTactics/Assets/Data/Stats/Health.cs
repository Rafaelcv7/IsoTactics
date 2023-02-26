using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using Character = IsoTactics.Character;
using Random = System.Random;

namespace IsoTactics.Stats
{
    public class Health : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        public AudioClip hurtFX;
        public GameEvents onDamageReceived;
        public GameEvents onDeath;
        private BaseCharacter _character;

        private void Start()
        {
            _character = gameObject.GetComponent<BaseCharacter>();
            maxHealth = _character.Stats.vitality.baseStat * 3;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage, float inflictorAccuracy)
        {
            damage = (int) (damage - (_character.Stats.vitality.statValue / 100f) * 20f);
            if (!ChanceOfEvade((int)((_character.Stats.agility.statValue / inflictorAccuracy) * 10f)))
            {
                if (onDamageReceived) { onDamageReceived.Raise(this, damage.ToString()); }
                currentHealth -= damage;
                _character.GetComponent<Reactions>().Shake(0.1f, 0.1f);
            }
            else
            {
                onDamageReceived.Raise(this, "Evaded!");
                _character.GetComponent<Reactions>().Evade(0.3f,0.2f);
                Debug.Log($"Attack Evaded by {_character.name}");
            }
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
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
        
        private bool ChanceOfEvade(int percentage)
        {
            Random rand = new Random();
            int randNum = rand.Next(100);

            return randNum < percentage;
        }
    }
}

