using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    private Image _portrait;
    private Image _portraitSprite;
    private TMP_Text _characterName;
    void Start()
    {
        _portrait = gameObject.GetComponent<Image>();
        _portraitSprite = gameObject.GetComponentsInChildren<Image>()[1];
        _characterName = gameObject.GetComponentInChildren<TMP_Text>();
        _portrait.color = new Color(1, 1, 1, 0);
    }

    public void ChangeSprite(Component sender, object data)
    {
        if (data is Character activeCharacter)
        {
            _portraitSprite.sprite = activeCharacter.portrait;
            _portrait.color = new Color(1, 1, 1, 1);
            _portraitSprite.color = new Color(1, 1, 1, 1);
            _characterName.text = activeCharacter.Name;
        }
        else if (data is OverlayTile tile)
        {
            if (tile.activeCharacter)
            {
                _portraitSprite.sprite = tile.activeCharacter.portrait;
                _portrait.color = new Color(1, 1, 1, 1);
                _portraitSprite.color = new Color(1, 1, 1, 1);
                _characterName.text = tile.activeCharacter.Name;
            }
            else
            {
                _portrait.color = new Color(1, 1, 1, 0);
                _portraitSprite.color = new Color(1, 1, 1, 0);
                _characterName.text = "";
            }
        }
            
    }
    
}
