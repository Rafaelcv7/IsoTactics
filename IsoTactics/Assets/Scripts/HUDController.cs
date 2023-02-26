using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IsoTactics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Character _character;
    private Image _portrait;
    private TMP_Text _characterName;
    private TMP_Text _characterAP;
    private TMP_Text _characterMP;
    private TMP_Text _characterHP;
    private Animator _lifeLineHeart;
    // Start is called before the first frame update
    void Start()
    {
        _portrait = gameObject.GetComponentsInChildren<Image>().First(x => x.name == "Portrait");
        _characterName = gameObject.GetComponentsInChildren<TMP_Text>().First(x => x.name == "NameText");
        _characterAP = gameObject.GetComponentsInChildren<TMP_Text>().First(x => x.name == "ApText");
        _characterMP = gameObject.GetComponentsInChildren<TMP_Text>().First(x => x.name == "MpText");
        _characterHP = gameObject.GetComponentsInChildren<TMP_Text>().First(x => x.name == "HealthValue");
        _lifeLineHeart = gameObject.GetComponentsInChildren<Animator>().First(x => x.name == "LifelineHeart");
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_character) return;
        UpdatePortraitInfo(_character);
        UpdateLifeline(_character);
    }

    public void ShowHudInfo(Component sender, object data)
    {
        if(!GamePhases.CurrentPhase.Equals("Turn")) return;
        if (data is Character activeCharacter)
        {
            _character = activeCharacter;
            gameObject.SetActive(true);
        }
        else if (data is OverlayTile tile)
        {
            if (tile.activeCharacter)
            {
                _character = tile.activeCharacter;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePortraitInfo(Character character)
    {
        _portrait.sprite = character.portrait;
        _characterName.text = character.Name;
        _characterAP.text = $"AP  {character.Stats.actionPoints.statValue.ToString()}";
        _characterMP.text = $"MP  {character.Stats.movementPoints.statValue.ToString()}";
    }

    private void UpdateLifeline(Character character)
    {
        switch ((int)((character.HP.currentHealth/(float)character.HP.maxHealth)*100f))
        {
            case > 80: {_lifeLineHeart.Play("Stable"); break;}
            case > 35: {_lifeLineHeart.Play("Fine"); break;}
            case > 0: {_lifeLineHeart.Play("Low"); break;}
            case <= 0: {{_lifeLineHeart.Play("Death"); break;}}
        }
        _characterHP.text = $"{character.HP.currentHealth}/{character.HP.maxHealth}";
    }
}
