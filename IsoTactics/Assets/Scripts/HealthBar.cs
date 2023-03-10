using System;
using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    private Slider _healthBarController;
    private TMP_Text _healthText;

    private Character _activeCharacter;

    private void Start()
    {
        _healthBarController = gameObject.GetComponent<Slider>();
        _healthText = gameObject.GetComponentInChildren<TMP_Text>();

    }

    void Update()
    {
        if (_activeCharacter)
        {
            _healthBarController.maxValue = _activeCharacter.HP.maxHealth;
            _healthBarController.value = _activeCharacter.HP.currentHealth;
            _healthText.text = $"{_activeCharacter.HP.currentHealth}/{_activeCharacter.HP.maxHealth}";
        }
    }
    
    public void SetActiveCharacter(Component sender, object data)
    {
        if (data is Character newCharacter)
        {
            _activeCharacter = newCharacter;
        }
    }
    
}
