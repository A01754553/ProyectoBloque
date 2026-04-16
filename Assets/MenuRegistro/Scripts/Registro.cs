using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Registro : MonoBehaviour
{
    private UIDocument menu;
    private Button botonListo;
    private TextField[] pinFields;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonListo = root.Q<Button>("BotonListo");
        botonListo.RegisterCallback<ClickEvent>(AbrirMenuNiveles);

        pinFields = new TextField[4];
        pinFields[0] = root.Q<TextField>("Pin1");
        pinFields[1] = root.Q<TextField>("Pin2");
        pinFields[2] = root.Q<TextField>("Pin3");
        pinFields[3] = root.Q<TextField>("Pin4");

        for (int i = 0; i < pinFields.Length; i++)
        {
            pinFields[i].maxLength = 1;
            int index = i;
            pinFields[i].RegisterValueChangedCallback(evt => OnPinChanged(index, evt.newValue));
        }
    }

    private void OnPinChanged(int index, string value)
    {
        if (!string.IsNullOrEmpty(value) && index < pinFields.Length - 1)
        {
            pinFields[index + 1].Focus();
        }
    }

    private void AbrirMenuNiveles(ClickEvent evt)
    {
        string pinCompleto = "";
        foreach (var field in pinFields)
        {
            pinCompleto += field.value;
        }

        Debug.Log("PIN ingresado: " + pinCompleto);

        SceneManager.LoadScene("MenuNiveles");
    }
}