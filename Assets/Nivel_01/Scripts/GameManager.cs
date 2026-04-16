using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // instancia global
    public static GameManager instance;

    // configuracion del nivel
    [SerializeField] private string[] silabas = { "MA", "ME", "MI", "MO", "MU" };
    [SerializeField] private AudioClip[] audiosSilabas;
    [SerializeField] private AudioClip audioCorrecto;
    [SerializeField] private AudioClip audioIncorrecto;

    // variables internas
    public int aciertos = 0;
    public int fallos = 0;
    private int silabaTurno = 0;
    private AudioSource audioSource;

    // desaciertos por pregunta
    private int[] desaciertosPorPregunta;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        // inicializar componentes
        audioSource = GetComponent<AudioSource>();

        // crear array del mismo tamaño que silabas, todo en 0
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
            // respuesta incorrecta — sumar al total y a la pregunta actual
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
            if (fallos <= 1)
                estrellas = 3;
            else if (fallos <= 3)
                estrellas = 2;
            else
                estrellas = 1;

            HUDPesca.instance.MostrarEstrellas(estrellas);

            // guardar progreso con playerprefs
            int nivelActual = PlayerPrefs.GetInt("nivelActual", 1);
            PlayerPrefs.SetInt("nivelCompletado", nivelActual);
            PlayerPrefs.Save();

            // debug para ver los desaciertos por pregunta
            for (int i = 0; i < desaciertosPorPregunta.Length; i++)
            {
                Debug.Log("Silaba " + silabas[i] + ": " + desaciertosPorPregunta[i] + " desaciertos");
            }
        }
    }
}