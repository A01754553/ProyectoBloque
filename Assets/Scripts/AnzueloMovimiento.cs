using UnityEngine;
using UnityEngine.InputSystem;

public class AnzueloMovimiento : MonoBehaviour
{
    [SerializeField] 
    private InputAction accionMover;
    [SerializeField] 
    private float limiteArriba = -100f;
    [SerializeField] 
    private float limiteAbajo = -120f;
    [SerializeField] private AudioClip audioCarrete;
    private AudioSource audioSource;
    private bool sonandoCarrete = false;
    private Rigidbody2D rb;
    private float velocidad = 5f;
    private bool subiendoAutomatico = false;
    private Transform pezActual = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        accionMover.Enable();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool moviendose = false;

        if (!subiendoAutomatico)
        {
            // movimiento manual del anzuelo
            Vector2 movimiento = accionMover.ReadValue<Vector2>();
            rb.linearVelocityY = velocidad * movimiento.y;
            moviendose = movimiento.y != 0;
        }
        else
        {
            // subida automatica con pez
            rb.linearVelocityY = velocidad * 2f;
            moviendose = true;

            if (transform.position.y >= limiteArriba)
            {
                // llego arriba, regresar pez
                rb.linearVelocityY = 0;
                subiendoAutomatico = false;

                if (pezActual != null)
                {
                    Pez scriptPez = pezActual.GetComponent<Pez>();
                    if (scriptPez != null)
                        scriptPez.Regresar();

                    pezActual = null;
                }
            }
        }

        // reproducir o detener audio del carrete
        if (moviendose && !sonandoCarrete)
        {
            audioSource.clip = audioCarrete;
            audioSource.loop = true;
            audioSource.Play();
            sonandoCarrete = true;
        }
        else if (!moviendose && sonandoCarrete)
        {
            audioSource.Stop();
            sonandoCarrete = false;
        }

        // limitar posicion en y
        float posY = Mathf.Clamp(transform.position.y, limiteAbajo, limiteArriba);
        transform.position = new Vector2(transform.position.x, posY);
    }

    public void ActivarSubida(Transform pez)
    {
        subiendoAutomatico = true;
        pezActual = pez;
    }

    public bool EstaSubiendo()
    {
        return subiendoAutomatico;
    }
}
