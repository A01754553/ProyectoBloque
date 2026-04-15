using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument _document;
    private Button _playButton;
    private AudioSource _audioSource;

    private void Awake()
    {
        // Referencias
        _document = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        if (_document == null)
        {
            Debug.LogError("No se encontró UIDocument en el GameObject.");
            return;
        }

        _playButton = _document.rootVisualElement.Q<Button>("playButton");

        if (_playButton == null)
        {
            Debug.LogError("No se encontró el botón 'playButton' en el UXML.");
            return;
        }

        _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);
    }

    private void OnPlayButtonClick(ClickEvent evt)
    {
        Debug.Log("¡Botón Jugar presionado!");

        if (_audioSource != null && _audioSource.clip != null)
        {
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource o AudioClip no asignado.");
        }

        Invoke(nameof(LoadNextScene), 1f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("MenuRegistro");
    }
}