using UnityEngine;

public class LineaAnzuelo : MonoBehaviour
{
    [SerializeField]
    private Transform puntaCana;

    [SerializeField]
    private Transform puntoAnzuelo; // ← nuevo: empty hijo del anzuelo

    private LineRenderer linea;

    void Start()
    {
        linea = GetComponent<LineRenderer>();
        linea.positionCount = 2;
    }

    void Update()
    {
        linea.SetPosition(0, puntaCana.position);
        linea.SetPosition(1, puntoAnzuelo.position); // ← usa el punto exacto
    }
}
