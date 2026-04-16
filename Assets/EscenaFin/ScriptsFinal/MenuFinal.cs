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
        
        // Callbacks
        botonPlayAgain.RegisterCallback<ClickEvent>(AbrirPlayAgain);
        botonSalir.RegisterCallback<ClickEvent>(AbrirSalirJuego);
    }

    private void AbrirPlayAgain(ClickEvent evt)
    {
        SceneManager.LoadScene("MenuRegistro");   
    }

    private void AbrirSalirJuego(ClickEvent evt)
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
