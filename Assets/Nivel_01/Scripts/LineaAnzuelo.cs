using UnityEngine;

public class LineaAnzuelo : MonoBehaviour
{
    [SerializeField]
     private Transform puntaCana;
    [SerializeField]
     private Transform puntoAnzuelo;

    private LineRenderer linea;

    void Start()
    {
        // inicializar line renderer con 2 puntos
        linea = GetComponent<LineRenderer>();
        linea.positionCount = 2;
    }

    void Update()
    {
        // actualizar posicion de la linea cada frame
        linea.SetPosition(0, puntaCana.position);
        linea.SetPosition(1, puntoAnzuelo.position);
    }
}