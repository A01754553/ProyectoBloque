using UnityEngine;
using UnityEngine.InputSystem;

public class AnzueloMovimiento : MonoBehaviour
{
    [SerializeField]
    private InputAction accionMover;

    [SerializeField]
    private float limiteArriba = -94f;   
    [SerializeField]
    private float limiteAbajo = -103f;   

    private Rigidbody2D rb;
    private float velocidad = 5f;

    void Start()
    {
        accionMover.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movimiento = accionMover.ReadValue<Vector2>();
        rb.linearVelocityY = velocidad * movimiento.y;

        float posY = Mathf.Clamp(transform.position.y, limiteAbajo, limiteArriba);
        transform.position = new Vector2(transform.position.x, posY);
    }
}
