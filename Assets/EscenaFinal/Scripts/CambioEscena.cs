using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    // componentes
    private Button botonNuevaPartida;
    private Button botonSalir;

    void OnEnable()
    {
        // recuperar elementos del hud
        var root = GetComponent<UIDocument>().rootVisualElement;

        botonNuevaPartida = root.Q<Button>("BotonNuevaPartida");
        botonSalir = root.Q<Button>("BotonSalir");

        botonNuevaPartida.RegisterCallback<ClickEvent>(NuevaPartida);
        botonSalir.RegisterCallback<ClickEvent>(Salir);
    }

    void OnDisable()
    {
        botonNuevaPartida.UnregisterCallback<ClickEvent>(NuevaPartida);
        botonSalir.UnregisterCallback<ClickEvent>(Salir);
    }

    private void NuevaPartida(ClickEvent evt)
    {
        // resetear progreso y volver al menu de niveles
        PlayerPrefs.DeleteKey("nivelCompletado");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MenuNiveles");
    }

    private void Salir(ClickEvent evt)
    {
        // resetear progreso y volver al login
        PlayerPrefs.DeleteKey("nivelCompletado");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MenuInicio");
    }
}
