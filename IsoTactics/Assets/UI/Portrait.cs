using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    private Image _portraitSprite;
    private TMP_Text _characterName;
    void Start()
    {
        _portraitSprite = gameObject.GetComponentsInChildren<Image>()[1];
        _characterName = gameObject.GetComponentInChildren<TMP_Text>();
    }

    public void ChangeSprite(Component sender, object data)
    {
        if (data is Character activeCharacter)
        {
            _portraitSprite.sprite = activeCharacter.portrait;
            _portraitSprite.color = new Color(1, 1, 1, 1);
            _characterName.text = activeCharacter.Name;
        }
            
    }
    
}
