using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CambiaEscena : MonoBehaviour
{
    private UIDocument menu;

    private Button BotonJugar;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();

        var root = menu.rootVisualElement;

        BotonJugar = root.Q<Button>("BotonJugar");

        BotonJugar.RegisterCallback<ClickEvent>(AbrirMenuRegistro);
    }

    private void AbrirMenuRegistro(ClickEvent evt)
    {
        SceneManager.LoadScene("MenuRegistro");
    }
}
