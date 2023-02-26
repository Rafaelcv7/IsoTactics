using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoDestroyText : MonoBehaviour
{
    public void DestroyOnAnimation()
    {
        var damageText = gameObject.GetComponent<TMP_Text>();
        Destroy(damageText.gameObject);
    }
}
