using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ControladorInterfaz : MonoBehaviour
{
    private VisualElement root;

    void OnEnable()
    {
        // Esto busca el documento que está en el mismo objeto
        root = GetComponent<UIDocument>().rootVisualElement;

        // Configurar Botón 1
        Button btn1 = root.Q<Button>("btn-1");
        if (btn1 != null) btn1.clicked += () => SceneManager.LoadScene("Nivel_01");

        // Configurar Botón Play
        Button btnPlay = root.Q<Button>("btn-play");
        if (btnPlay != null) btnPlay.clicked += () => SceneManager.LoadScene("Nivel_01");
    }
}