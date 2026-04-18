using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private string[] silabas = { "A", "E", "I", "O", "U" };
    [SerializeField] private int[] idPreguntas = { 1, 2, 3, 4, 5 };
    [SerializeField] private AudioClip[] audiosSilabas;
    [SerializeField] private AudioClip audioCorrecto;
    [SerializeField] private AudioClip audioIncorrecto;

    public int aciertos = 0;
    public int fallos = 0;
    private int silabaTurno = 0;
    private AudioSource audioSource;
    private int[] desaciertosPorPregunta;

    // url base de la api
    private string urlBase = "https://ampi8wp2ei.execute-api.us-east-1.amazonaws.com";

    // estructuras de datos
    [System.Serializable]
    public struct ResultadoPregunta
    {
        public int id_pregunta;
        public int desaciertos;
    }

    [System.Serializable]
    public struct DatosResultado
    {
        public int id_partida;
        public int id_nivel;
        public ResultadoPregunta[] resultados;
    }

    public struct DatosFinalizarPartida
    {
        public int id_partida;
    }

    void Awake()
    {
        // inicializar instancia
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        desaciertosPorPregunta = new int[silabas.Length];
        MostrarSilaba();
    }

    public void MostrarSilaba()
    {
        // actualizar hud y reproducir audio de la silaba
        HUDPesca.instance.ActualizarSilaba(silabas[silabaTurno]);
        if (audiosSilabas[silabaTurno] != null)
            audioSource.PlayOneShot(audiosSilabas[silabaTurno]);
    }

    public void ResponderPez(string silabaPez, Pez pez)
    {
        if (silabaPez == silabas[silabaTurno])
        {
            // respuesta correcta
            aciertos++;
            if (audioCorrecto != null)
                audioSource.PlayOneShot(audioCorrecto);
            StartCoroutine(EsperarYMostrar());
        }
        else
        {
            // respuesta incorrecta
            fallos++;
            desaciertosPorPregunta[silabaTurno]++;
            if (audioIncorrecto != null)
                audioSource.PlayOneShot(audioIncorrecto);
        }
    }

    private IEnumerator EsperarYMostrar()
    {
        yield return new WaitForSeconds(3f);
        silabaTurno++;
        if (silabaTurno < silabas.Length)
        {
            MostrarSilaba();
        }
        else
        {
            // calcular estrellas
            int estrellas;
            if (fallos <= 1) estrellas = 3;
            else if (fallos <= 3) estrellas = 2;
            else estrellas = 1;

            HUDPesca.instance.MostrarEstrellas(estrellas);

            // guardar progreso local y mandar a la bd
            int nivelActual = PlayerPrefs.GetInt("nivelActual", 1);
            PlayerPrefs.SetInt("nivelCompletado", nivelActual);
            PlayerPrefs.Save();

            StartCoroutine(EnviarResultados(nivelActual));
        }
    }

    private IEnumerator EnviarResultados(int nivelActual)
    {
        // armar resultados por pregunta
        ResultadoPregunta[] resultados = new ResultadoPregunta[silabas.Length];
        for (int i = 0; i < silabas.Length; i++)
        {
            resultados[i] = new ResultadoPregunta
            {
                id_pregunta = idPreguntas[i],
                desaciertos = desaciertosPorPregunta[i]
            };

        }

        DatosResultado datos = new DatosResultado
        {
            id_partida = PlayerPrefs.GetInt("idPartida", 0),
            id_nivel = nivelActual,
            resultados = resultados
        };

        string json = JsonUtility.ToJson(datos);


        UnityWebRequest request = UnityWebRequest.Post(urlBase + "/resultado", json, "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success || request.responseCode == 401)
        {
            // si era el nivel 8, finalizar partida
            if (nivelActual == 8)
                StartCoroutine(FinalizarPartida());
        }

        request.Dispose();
    }

    private IEnumerator FinalizarPartida()
    {
        DatosFinalizarPartida datos = new DatosFinalizarPartida
        {
            id_partida = PlayerPrefs.GetInt("idPartida", 0)
        };

        string json = JsonUtility.ToJson(datos);

        UnityWebRequest request = UnityWebRequest.Put(urlBase + "/partida/finalizar", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        request.Dispose();
    }
}