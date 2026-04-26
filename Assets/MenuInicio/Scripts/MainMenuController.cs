using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument _document;
    private Button _playButton;
    private Button _botonCreditos;
    private Button _botonCerrarCreditos;
    private VisualElement _popUpCreditos;
    private VisualElement _obscurecimiento;
    private AudioSource _audioSource;

    void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        if (_document == null) return;

        var root = _document.rootVisualElement;

        _playButton = root.Q<Button>("playButton");
        _botonCreditos = root.Q<Button>("BotonCreditos");
        _botonCerrarCreditos = root.Q<Button>("BotonCerrarCreditos");
        _popUpCreditos = root.Q<VisualElement>("PopUpCreditos");
        _obscurecimiento = root.Q<VisualElement>("Obscurecimiento");

        if (_playButton != null)
            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);

        if (_botonCreditos != null)
            _botonCreditos.RegisterCallback<ClickEvent>(AbrirCreditos);

        if (_botonCerrarCreditos != null)
            _botonCerrarCreditos.RegisterCallback<ClickEvent>(CerrarCreditos);
    }

    void OnDisable()
    {
        if (_playButton != null)
            _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClick);

        if (_botonCreditos != null)
            _botonCreditos.UnregisterCallback<ClickEvent>(AbrirCreditos);

        if (_botonCerrarCreditos != null)
            _botonCerrarCreditos.UnregisterCallback<ClickEvent>(CerrarCreditos);
    }

    private void OnPlayButtonClick(ClickEvent evt)
    {
        // reproducir audio y cargar siguiente escena
        if (_audioSource != null && _audioSource.clip != null)
            _audioSource.Play();

        Invoke(nameof(LoadNextScene), 1f);
    }

    private void AbrirCreditos(ClickEvent evt)
    {
        // mostrar popup y obscurecimiento
        _popUpCreditos.style.display = DisplayStyle.Flex;
        _obscurecimiento.style.display = DisplayStyle.Flex;
    }

    private void CerrarCreditos(ClickEvent evt)
    {
        // ocultar popup y obscurecimiento
        _popUpCreditos.style.display = DisplayStyle.None;
        _obscurecimiento.style.display = DisplayStyle.None;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("MenuRegistro");
    }
}