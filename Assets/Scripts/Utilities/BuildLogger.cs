using UnityEngine;
using TMPro;

public class BuildLogger : MonoBehaviour
{
    public static BuildLogger Instance;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        Instance = this;

        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Log(string message)
    {
        _text.text = message;
    }
}
