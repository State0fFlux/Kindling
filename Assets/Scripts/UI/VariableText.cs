using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextVariableBinding : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        string output = text.text;

        // Replace any recognized placeholders
        output = output.Replace("{time}", NightManager.Instance.GetTime());
        // add any other placeholders

        text.text = output;
    }
}
