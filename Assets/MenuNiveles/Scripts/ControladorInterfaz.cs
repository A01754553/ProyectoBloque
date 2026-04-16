using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ControladorInterfaz : MonoBehaviour
{
    private VisualElement root;
    private Button botonSalir;

    [SerializeField] private int totalNiveles = 8;

    // url base de la api
    private string urlBase = "https://ampi8wp2ei.execute-api.us-east-1.amazonaws.com";

    // estructuras de datos
    public struct RespuestaProgreso
    {
        public int id_partida;
        public int[] niveles_completados;
    }

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        botonSalir = root.Q<Button>("BotonSalir");

        botonSalir.clicked += Salir;

        // bloquear botones mientras carga
        BloquearTodos();

        // obtener progreso de la bd
        int idAlumno = PlayerPrefs.GetInt("idAlumno", 0);
        StartCoroutine(ObtenerProgreso(idAlumno));
    }

    void OnDisable()
    {
        botonSalir.clicked -= Salir;
    }

    private void BloquearTodos()
    {
        for (int i = 1; i <= totalNiveles; i++)
        {
            Button btn = root.Q<Button>("btn-" + i);
            if (btn != null)
            {
                btn.SetEnabled(false);
                btn.text = "";
            }
        }
    }

    private IEnumerator ObtenerProgreso(int idAlumno)
    {
        UnityWebRequest request = UnityWebRequest.Get(urlBase + "/progreso/" + idAlumno);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success || request.responseCode == 401)
        {
            RespuestaProgreso respuesta = JsonUtility.FromJson<RespuestaProgreso>(request.downloadHandler.text);

            // actualizar id_partida
            PlayerPrefs.SetInt("idPartida", respuesta.id_partida);
            PlayerPrefs.Save();

            ConfigurarBotones(respuesta.niveles_completados);
        }
        else
        {
            // si falla habilitar solo el nivel 1
            ConfigurarBotones(new int[0]);
        }

        request.Dispose();
    }

    private void ConfigurarBotones(int[] nivelesCompletados)
    {
        // nivel mas alto habilitado
        int nivelHabilitado = nivelesCompletados.Length > 0
            ? System.Linq.Enumerable.Max(nivelesCompletados) + 1
            : 1;

        // configurar cada boton
        for (int i = 1; i <= totalNiveles; i++)
        {
            Button btn = root.Q<Button>("btn-" + i);
            if (btn != null)
            {
                if (i <= nivelHabilitado)
                {
                    btn.SetEnabled(true);
                    btn.text = i.ToString();

                    int numeroCarga = i;
                    btn.clicked += () => CargarNivel(numeroCarga);
                }
                else
                {
                    btn.SetEnabled(false);
                    btn.text = "";
                }
            }
        }

        // boton play al nivel mas alto disponible
        Button btnPlay = root.Q<Button>("btn-play");
        if (btnPlay != null)
            btnPlay.clicked += () => CargarNivel(nivelHabilitado);
    }

    private void CargarNivel(int numero)
    {
        // guardar nivel actual y cargar escena
        PlayerPrefs.SetInt("nivelActual", numero);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Nivel_" + numero.ToString("D2"));
    }

    private void Salir()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}