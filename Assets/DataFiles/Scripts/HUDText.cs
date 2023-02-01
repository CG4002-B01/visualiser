using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ShieldText;
    [SerializeField] TextMeshProUGUI AmmoText;
    [SerializeField] TextMeshProUGUI GrenadeText; 
    // [SerializeField] TextMeshProUGUI ShieldTimer;
    [SerializeField] TextMeshProUGUI ShieldTimerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShieldText(string text)
    {
        ShieldText.text = text;
    }

    public void SetShieldTimerText(string text)
    {
        ShieldTimerText.text = text;
    }

    public void SetGrenadeText(string text)
    {
        GrenadeText.text = text;
    }

    public void SetAmmoText(string text)
    {
        AmmoText.text = text;
    }
}
