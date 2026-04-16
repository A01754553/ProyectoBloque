using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HUDPesca : MonoBehaviour
{
    public static HUDPesca instance;
    private Label labelSilaba;
    private VisualElement panelEstrellas;
    private VisualElement estrella1;
    private VisualElement estrella2;
    private VisualElement estrella3;
    private Button botonContinuar;
    private VisualElement banner;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        labelSilaba = root.Q<Label>("LabelSilaba");
        panelEstrellas = root.Q<VisualElement>("Estrellas");
        estrella1 = root.Q<VisualElement>("Estrella_1");
        estrella2 = root.Q<VisualElement>("Estrella_2");
        estrella3 = root.Q<VisualElement>("Estrella_3");
        botonContinuar = root.Q<Button>("BotonContinuar");
        banner = root.Q<VisualElement>("Banner");

        botonContinuar.clicked += CerrarPanel;
    }

    void OnDisable()
    {
        botonContinuar.clicked -= CerrarPanel;
    }

    public void ActualizarSilaba(string silaba)
    {
        labelSilaba.text = "¡Pesca al pez con " + silaba + "!";
    }

    public void MostrarEstrellas(int estrellas)
    {
        // mostrar panel
        panelEstrellas.style.display = DisplayStyle.Flex;

        // mostrar imagen segun estrellas
        estrella1.style.display = estrellas == 1 ? DisplayStyle.Flex : DisplayStyle.None;
        estrella2.style.display = estrellas == 2 ? DisplayStyle.Flex : DisplayStyle.None;
        estrella3.style.display = estrellas == 3 ? DisplayStyle.Flex : DisplayStyle.None;
        labelSilaba.style.display = DisplayStyle.None;
        banner.style.display = DisplayStyle.None;
        botonContinuar.style.display = DisplayStyle.Flex;
        // desactivar anzuelo
        AnzueloMovimiento anzuelo = FindAnyObjectByType<AnzueloMovimiento>();
        if (anzuelo != null)
            anzuelo.enabled = false;
    }

    private void CerrarPanel()
    {
        panelEstrellas.style.display = DisplayStyle.None;
        banner.style.display = DisplayStyle.Flex;
        labelSilaba.style.display = DisplayStyle.Flex;
        AnzueloMovimiento anzuelo = FindAnyObjectByType<AnzueloMovimiento>();
        if (anzuelo != null)
            anzuelo.enabled = true;
        
        SceneManager.LoadScene("MenuNiveles");
    }
}