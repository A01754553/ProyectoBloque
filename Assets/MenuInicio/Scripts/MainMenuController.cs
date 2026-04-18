using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument _document;
    private Button _playButton;
    private AudioSource _audioSource;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        if (_document == null) return;

        _playButton = _document.rootVisualElement.Q<Button>("playButton");

        if (_playButton == null) return;

        _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);
    }

    void OnDisable()
    {
        if (_playButton != null)
            _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClick);
    }

    private void OnPlayButtonClick(ClickEvent evt)
    {
        // reproducir audio y cargar siguiente escena
        if (_audioSource != null && _audioSource.clip != null)
            _audioSource.Play();

        Invoke(nameof(LoadNextScene), 1f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("MenuRegistro");
    }
}