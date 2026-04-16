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

        // Configurar cada campo
        for (int i = 0; i < pinFields.Length; i++)
        {
            pinFields[i].maxLength = 1; // Solo un dígito
            int index = i; // Capturar índice para el callback
            pinFields[i].RegisterValueChangedCallback(evt => OnPinChanged(index, evt.newValue));
        }
    }

    private void OnPinChanged(int index, string value)
    {
        // Si se escribió un número, pasa al siguiente campo
        if (!string.IsNullOrEmpty(value) && index < pinFields.Length - 1)
        {
            pinFields[index + 1].Focus();
        }
    }

    private void AbrirMenuNiveles(ClickEvent evt)
    {
        SceneManager.LoadScene("MenuNiveles");
    }
}