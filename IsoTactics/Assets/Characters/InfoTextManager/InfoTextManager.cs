using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics.InfoTextManager
{
    public class InfoTextManager : MonoBehaviour
    {
        public GameObject damageTextPrefab;
        private Character _character;

        private void Start()
        {
            _character = gameObject.transform.parent.GetComponent<Character>();
        }

        private void InstantiateDamageText(string damage)
        {
            if (damage != null)
            {
                var damageTextInstance = Instantiate(damageTextPrefab, transform);
                damageTextInstance.GetComponent<TMP_Text>().text = damage;
            }
        }

        public void ShowDamageText(Component sender, object data)
        {
            if (data is string damage && sender.GetComponent<Character>() == _character)
            {
                InstantiateDamageText(damage);
            }
        }
    } 
}

