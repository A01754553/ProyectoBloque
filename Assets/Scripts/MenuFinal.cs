using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    private UIDocument menu;
    private Button botonPlayAgain;
    private Button botonSalir;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonPlayAgain = root.Q<Button>("BotonPlayAgain");
        botonSalir = root.Q<Button>("BotonSalir");

        botonPlayAgain.RegisterCallback<ClickEvent>(AbrirPlayAgain);
        botonSalir.RegisterCallback<ClickEvent>(AbrirSalirJuego);
    }

    void OnDisable()
    {
        botonPlayAgain.UnregisterCallback<ClickEvent>(AbrirPlayAgain);
        botonSalir.UnregisterCallback<ClickEvent>(AbrirSalirJuego);
    }

    private void AbrirPlayAgain(ClickEvent evt)
    {
        // resetear progreso local y volver al login
        // la nueva partida se crea automaticamente al iniciar sesion
        PlayerPrefs.DeleteKey("nivelCompletado");
        PlayerPrefs.DeleteKey("idPartida");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MenuRegistro");
    }

    private void AbrirSalirJuego(ClickEvent evt)
    {
        // resetear progreso local y salir
        PlayerPrefs.DeleteKey("nivelCompletado");
        PlayerPrefs.DeleteKey("idPartida");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MenuInicio");
    }
}