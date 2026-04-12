using UnityEngine;

public class TextoFijo : MonoBehaviour
{
    private Canvas canvas;

    void Start()
    {
        // obtener canvas padre
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        // mantener rotacion fija sin importar el padre
        transform.rotation = Quaternion.identity;
    }
}
