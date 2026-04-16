using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Registro : MonoBehaviour
{
    // componentes
    private UIDocument menu;
    private Button botonListo;
    private Label labelMensaje;
    private TextField[] pinFields;

    // pines validos hardcodeados (luego se reemplaza con BD)
    private string[] pinesValidos = { "1111", "2222", "3333" };

    void OnEnable()
    {
        // recuperar elementos del hud
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonListo = root.Q<Button>("BotonListo");
        labelMensaje = root.Q<Label>("LabelMensaje");

        pinFields = new TextField[4];
        pinFields[0] = root.Q<TextField>("Pin1");
        pinFields[1] = root.Q<TextField>("Pin2");
        pinFields[2] = root.Q<TextField>("Pin3");
        pinFields[3] = root.Q<TextField>("Pin4");

        // configurar campos
        for (int i = 0; i < pinFields.Length; i++)
        {
            pinFields[i].maxLength = 1;
            int index = i;
            pinFields[i].RegisterValueChangedCallback(evt => OnPinChanged(index, evt.newValue));
        }

        botonListo.RegisterCallback<ClickEvent>(ValidarPin);
    }

    private void OnPinChanged(int index, string value)
    {
        // avanzar al siguiente campo automaticamente
        if (!string.IsNullOrEmpty(value) && index < pinFields.Length - 1)
        {
            pinFields[index + 1].Focus();
        }
    }

    private void ValidarPin(ClickEvent evt)
    {
        // armar el pin completo
        string pinCompleto = "";
        foreach (var field in pinFields)
            pinCompleto += field.value;

        // buscar el pin en el array
        bool pinEncontrado = false;
        int indicAlumno = -1;

        for (int i = 0; i < pinesValidos.Length; i++)
        {
            if (pinCompleto == pinesValidos[i])
            {
                pinEncontrado = true;
                indicAlumno = i + 1;
                break;
            }
        }

        if (pinEncontrado)
        {
            // si el alumno que entro es diferente al anterior, resetear progreso
            int alumnoAnterior = PlayerPrefs.GetInt("idAlumnoAnterior", -1);
            if (alumnoAnterior != indicAlumno)
            {
                PlayerPrefs.DeleteKey("nivelCompletado");
                PlayerPrefs.SetInt("idAlumnoAnterior", indicAlumno);
                PlayerPrefs.Save();
            }

            // guardar id del alumno para usarlo despues en la BD
            PlayerPrefs.SetInt("idAlumno", indicAlumno);
            PlayerPrefs.Save();

            SceneManager.LoadScene("MenuNiveles");
        }
        else
        {
            // mostrar mensaje de error
            labelMensaje.text = "PIN incorrecto, intentalo de nuevo";
        }
    }
}