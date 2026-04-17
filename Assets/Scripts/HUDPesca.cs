using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HUDPesca : MonoBehaviour
{
    // instancia global
    public static HUDPesca instance;

    // elementos de ui
    private Label labelSilaba;
    private VisualElement panelEstrellas;
    private VisualElement estrella1;
    private VisualElement estrella2;
    private VisualElement estrella3;
    private Button botonContinuar;
    private VisualElement banner;
    private VisualElement panelPausa;
    private Button botonPausa;
    private Button botonReanudar;
    private Button botonReiniciar;
    private Button botonSalirPausa;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void OnEnable()
    {
        // recuperar elementos del hud
        var root = GetComponent<UIDocument>().rootVisualElement;
        labelSilaba = root.Q<Label>("LabelSilaba");
        panelEstrellas = root.Q<VisualElement>("Estrellas");
        estrella1 = root.Q<VisualElement>("Estrella_1");
        estrella2 = root.Q<VisualElement>("Estrella_2");
        estrella3 = root.Q<VisualElement>("Estrella_3");
        botonContinuar = root.Q<Button>("BotonContinuar");
        banner = root.Q<VisualElement>("Banner");
        panelPausa = root.Q<VisualElement>("PanelPausa");
        botonPausa = root.Q<Button>("BotonPausa");
        botonReanudar = root.Q<Button>("BotonReanudar");
        botonReiniciar = root.Q<Button>("BotonReiniciar");
        botonSalirPausa = root.Q<Button>("BotonSalir");

        // registrar callbacks
        botonContinuar.clicked += CerrarPanel;
        botonPausa.clicked += AbrirPausa;
        botonReanudar.clicked += CerrarPausa;
        botonReiniciar.clicked += Reiniciar;
        botonSalirPausa.clicked += SalirAlMenu;
    }

    void OnDisable()
    {
        botonContinuar.clicked -= CerrarPanel;
        botonPausa.clicked -= AbrirPausa;
        botonReanudar.clicked -= CerrarPausa;
        botonReiniciar.clicked -= Reiniciar;
        botonSalirPausa.clicked -= SalirAlMenu;
    }

    void Update()
    {
        // abrir/cerrar pausa con escape
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (panelPausa.style.display == DisplayStyle.Flex)
                CerrarPausa();
            else
                AbrirPausa();
        }
    }

    public void ActualizarSilaba(string silaba)
    {
        labelSilaba.text = "¡Pesca al pez con " + silaba + "!";
    }

    public void MostrarEstrellas(int estrellas)
    {
        // mostrar panel de estrellas
        panelEstrellas.style.display = DisplayStyle.Flex;
        estrella1.style.display = estrellas == 1 ? DisplayStyle.Flex : DisplayStyle.None;
        estrella2.style.display = estrellas == 2 ? DisplayStyle.Flex : DisplayStyle.None;
        estrella3.style.display = estrellas == 3 ? DisplayStyle.Flex : DisplayStyle.None;
        labelSilaba.style.display = DisplayStyle.None;
        banner.style.display = DisplayStyle.None;
        botonContinuar.style.display = DisplayStyle.Flex;
        botonPausa.style.display = DisplayStyle.None;

        // desactivar anzuelo
        AnzueloMovimiento anzuelo = FindAnyObjectByType<AnzueloMovimiento>();
        if (anzuelo != null)
            anzuelo.enabled = false;
    }

    private void CerrarPanel()
    {
        SceneManager.LoadScene("MenuNiveles");
    }

    private void AbrirPausa()
    {
        // mostrar panel y pausar juego
        panelPausa.style.display = DisplayStyle.Flex;
        botonPausa.style.display = DisplayStyle.None;
        Time.timeScale = 0f;
    }

    private void CerrarPausa()
    {
        // ocultar panel y reanudar juego
        panelPausa.style.display = DisplayStyle.None;
        botonPausa.style.display = DisplayStyle.Flex;
        Time.timeScale = 1f;
    }

    private void Reiniciar()
    {
        // reiniciar escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SalirAlMenu()
    {
        // salir al menu de niveles
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuNiveles");
    }
}