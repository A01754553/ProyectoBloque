using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Registro : MonoBehaviour
{
    private UIDocument menu;
    private Button botonListo;
    private Label labelMensaje;
    private TextField[] pinFields;

    // url base de la api
    private string urlBase = "https://ampi8wp2ei.execute-api.us-east-1.amazonaws.com";

    // estructuras de datos
    public struct PinLogin
    {
        public string pin;
    }

    public struct RespuestaLogin
    {
        public bool exito;
        public string mensaje;
        public int id_alumno;
        public string nombre;
        public int id_partida;
        public int ultimo_nivel;
    }

    public struct DatosPartida
    {
        public int id_alumno;
    }

    public struct RespuestaPartida
    {
        public bool exito;
        public int id_partida;
    }

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

        // configurar campos y callbacks
        for (int i = 0; i < pinFields.Length; i++)
        {
            pinFields[i].maxLength = 1;
            int index = i;
            pinFields[i].RegisterValueChangedCallback(evt => OnPinChanged(index, evt.newValue));
        }

        botonListo.RegisterCallback<ClickEvent>(ValidarPin);
    }

    void OnDisable()
    {
        botonListo.UnregisterCallback<ClickEvent>(ValidarPin);
    }

    private void OnPinChanged(int index, string value)
    {
        // avanzar al siguiente campo automaticamente
        if (!string.IsNullOrEmpty(value) && index < pinFields.Length - 1)
            pinFields[index + 1].Focus();
    }

    private void ValidarPin(ClickEvent evt)
    {
        // armar pin y enviar
        string pinCompleto = "";
        foreach (var field in pinFields)
            pinCompleto += field.value;

        labelMensaje.text = "Validando...";
        StartCoroutine(EnviarLogin(pinCompleto));
    }

    private IEnumerator EnviarLogin(string pin)
    {
        PinLogin datos = new PinLogin { pin = pin };
        string json = JsonUtility.ToJson(datos);

        UnityWebRequest request = UnityWebRequest.Post(urlBase + "/login", json, "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success || request.responseCode == 401)
        {
            RespuestaLogin respuesta = JsonUtility.FromJson<RespuestaLogin>(request.downloadHandler.text);

            if (respuesta.exito)
            {
                // guardar datos del alumno
                PlayerPrefs.SetInt("idAlumno", respuesta.id_alumno);
                PlayerPrefs.SetString("nombreAlumno", respuesta.nombre);
                PlayerPrefs.Save();

                if (respuesta.id_partida == 0)
                    StartCoroutine(CrearPartida(respuesta.id_alumno));
                else
                {
                    PlayerPrefs.SetInt("idPartida", respuesta.id_partida);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("MenuNiveles");
                }
            }
            else
            {
                labelMensaje.text = respuesta.mensaje;
            }
        }
        else
        {
            labelMensaje.text = "Error de conexion: " + request.error;
        }

        request.Dispose();
    }

    private IEnumerator CrearPartida(int idAlumno)
    {
        DatosPartida datos = new DatosPartida { id_alumno = idAlumno };
        string json = JsonUtility.ToJson(datos);

        UnityWebRequest request = UnityWebRequest.Post(urlBase + "/partida", json, "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success || request.responseCode == 401)
        {
            RespuestaPartida respuesta = JsonUtility.FromJson<RespuestaPartida>(request.downloadHandler.text);

            if (respuesta.exito)
            {
                PlayerPrefs.SetInt("idPartida", respuesta.id_partida);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MenuNiveles");
            }
        }
        else
        {
            labelMensaje.text = "Error al crear partida: " + request.error;
        }

        request.Dispose();
    }
}