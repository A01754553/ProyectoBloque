using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private string[] silabas = { "MA", "ME", "MI", "MO", "MU" };
    [SerializeField] private AudioClip[] audiosSilabas;

    public int puntos = 0;
    private int silabaTurno = 0;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        MostrarSilaba();
    }

    public void MostrarSilaba()
    {
        // actualizar hud y reproducir audio
        HUDPesca.instance.ActualizarSilaba(silabas[silabaTurno]);
        if (audiosSilabas[silabaTurno] != null)
            audioSource.PlayOneShot(audiosSilabas[silabaTurno]);
    }

    public void ResponderPez(string silabaPez, Pez pez)
    {
        if (silabaPez == silabas[silabaTurno])
        {
            // respuesta correcta
            puntos += 100;
            silabaTurno++;
            if (silabaTurno < silabas.Length)
                MostrarSilaba();
            else
                Debug.Log("fin del nivel");
        }
        else
        {
            // respuesta incorrecta
            puntos -= 50;
        }
    }
}