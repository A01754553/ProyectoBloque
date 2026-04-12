using UnityEngine;

public class Pez : MonoBehaviour
{
    [SerializeField] 
    private string silaba = "MA";

    private bool atrapado = false;
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;

    void Start()
    {
        // guardar posicion y rotacion iniciales
        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;
    }

    public void Regresar()
    {
        // resetear estado del pez a su posicion original
        atrapado = false;
        transform.SetParent(null);
        transform.position = posicionOriginal;
        transform.rotation = rotacionOriginal;

        // asegurar que el sprite sea visible
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;

        // reiniciar movimiento
        scFishMove movimiento = GetComponent<scFishMove>();
        if (movimiento != null)
        {
            movimiento.enabled = false;
            movimiento.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Anzuelo") && !atrapado)
        {
            AnzueloMovimiento anzuelo = collision.GetComponent<AnzueloMovimiento>();

            if (anzuelo != null && !anzuelo.EstaSubiendo())
            {
                atrapado = true;

                // pegar pez al anzuelo
                transform.SetParent(collision.transform);
                transform.localPosition = Vector2.zero;
                transform.localRotation = Quaternion.Euler(10f, 90f, 90f);

                // detener movimiento del pez
                scFishMove movimiento = GetComponent<scFishMove>();
                if (movimiento != null)
                    movimiento.enabled = false;

                // notificar al gamemanager
                if (GameManager.instance != null)
                    GameManager.instance.ResponderPez(silaba, this);

                // activar subida del anzuelo
                anzuelo.ActivarSubida(transform);
            }
        }
    }
}
