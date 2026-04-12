using UnityEngine;
using UnityEngine.InputSystem;

public class AnzueloMovimiento : MonoBehaviour
{
    [SerializeField] 
    private InputAction accionMover;
    [SerializeField] 
    private float limiteArriba = -94f;
    [SerializeField] 
    private float limiteAbajo = -120f;

    private Rigidbody2D rb;
    private float velocidad = 5f;
    private bool subiendoAutomatico = false;
    private Transform pezActual = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        accionMover.Enable();
    }

    void Update()
    {
        if (!subiendoAutomatico)
        {
            // movimiento manual del anzuelo
            Vector2 movimiento = accionMover.ReadValue<Vector2>();
            rb.linearVelocityY = velocidad * movimiento.y;
        }
        else
        {
            // subida automatica con pez
            rb.linearVelocityY = velocidad * 2f;

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
