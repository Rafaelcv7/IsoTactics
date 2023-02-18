using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IsoTactics;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    private ButtonEventHandler[] _buttons;

    private Character _activeCharacter;
    // Start is called before the first frame update
    void Start()
    {
        _buttons = gameObject.GetComponentsInChildren<ButtonEventHandler>(); //TODO: Change to AbilityButtonClass.
    }

    // Update is called once per frame
    void Update()
    {
        if (_activeCharacter != null)
        {
            var abilities = _buttons.Where(x => x.name.Contains("Ability")).ToList();
            for (var i = 0; i < abilities.Count; i++)
            {
                if (_activeCharacter.abilities[i])
                {
                    abilities[i].ability = _activeCharacter.abilities[i];
                    var abilityIcon = abilities[i].GetComponentsInChildren<Image>().First(x => x.name.Contains("Icon"));
                    abilityIcon.sprite = _activeCharacter.abilities[i].abilityIcon;
                    abilityIcon.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    abilities[i].ability = null;
                    var abilityIcon = abilities[i].GetComponentsInChildren<Image>().First(x => x.name.Contains("Icon"));
                    abilityIcon.sprite = null;
                    abilityIcon.color = new Color(1, 1, 1, 0);
                }
            } 
        }
    }

    public void SetActiveCharacter(Component sender, object data)
    {
        if (data is Character character)
        {
            _activeCharacter = character;
        }
    }
}
