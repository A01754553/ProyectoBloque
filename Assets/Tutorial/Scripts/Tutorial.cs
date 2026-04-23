using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private Button botonContinuar;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        botonContinuar = root.Q<Button>("BotonContinuar");

        botonContinuar.RegisterCallback<ClickEvent>(Continuar);
    }

    void OnDisable()
    {
        botonContinuar.UnregisterCallback<ClickEvent>(Continuar);
    }

    private void Continuar(ClickEvent evt)
    {
        SceneManager.LoadScene("MenuNiveles");
    }
}