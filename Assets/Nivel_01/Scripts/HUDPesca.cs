using UnityEngine;
using UnityEngine.UIElements;

public class HUDPesca : MonoBehaviour
{
    public static HUDPesca instance;

    private Label labelSilaba;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        labelSilaba = root.Q<Label>("LabelSilaba");
    }

    public void ActualizarSilaba(string silaba)
    {
        labelSilaba.text = "¡Pesca al pez con " + silaba + "!";
    }
}