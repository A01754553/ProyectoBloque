using UnityEngine;
using UnityEngine.UIElements;

public class FloatingText : MonoBehaviour
{
    public float amplitude = 5f;
    public float frequency = 1f;
    private Button playButton;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        playButton = root.Q<Button>("playButton");
    }

    void Update()
    {
        if (playButton != null)
        {
            float y = Mathf.Sin(Time.time * frequency) * amplitude;
            playButton.style.translate = new Translate(0, y, 0);
        }
    }
}