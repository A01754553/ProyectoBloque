using UnityEngine;

public class Pez : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Anzuelo"))
        {
            Debug.Log("Pez atrapado");

            Destroy(gameObject);
        }
    }
}