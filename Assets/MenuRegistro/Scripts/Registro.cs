using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Registro : MonoBehaviour
{
    private UIDocument menu;

    private Button BotonListo;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();

        var root = menu.rootVisualElement;

        BotonListo = root.Q<Button>("BotonListo");

        BotonListo.RegisterCallback<ClickEvent>(AbrirMenuNiveles);
    }

    private void AbrirMenuNiveles(ClickEvent evt)
    {
        SceneManager.LoadScene("MenuNiveles");
    }
}
