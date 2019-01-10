using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FitnessTextController : MonoBehaviour {
    public TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        WalkerCentraliser.OnValueUpdated += UpdateText;
    }

    public void UpdateText(int amount)
    {
        textMeshProUGUI.text = amount.ToString();
    }
}
