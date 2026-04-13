using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] 
    private string[] silabas = { "MA", "ME", "MI", "MO", "MU" };
    [SerializeField] 
    private AudioClip[] audiosSilabas;
    [SerializeField] 
    private AudioClip audioCorrecto;
    [SerializeField] 
    private AudioClip audioIncorrecto;

    public int aciertos = 0;
    public int fallos = 0;
    private int silabaTurno = 0;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        // mostrar primera silaba
        audioSource = GetComponent<AudioSource>();
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

            // esperar audio antes de mostrar siguiente silaba
            StartCoroutine(EsperarYMostrar());
        }
        else
        {
            // respuesta incorrecta
            fallos++;
            if (audioIncorrecto != null)
                audioSource.PlayOneShot(audioIncorrecto);
        }
    }

    private IEnumerator EsperarYMostrar()
    {
        // esperar a que termine el audio de correcto
        yield return new WaitForSeconds(3f);
        silabaTurno++;
        if (silabaTurno < silabas.Length)
            MostrarSilaba();
        else {
            // calcular estrellas
            int estrellas;
            if (fallos <= 1)
                estrellas = 3;
            else if (fallos <= 3)
                estrellas = 2;
            else
                estrellas = 1;

            HUDPesca.instance.MostrarEstrellas(estrellas);
        }
    }
}