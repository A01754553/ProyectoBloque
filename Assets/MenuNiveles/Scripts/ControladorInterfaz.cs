using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ControladorInterfaz : MonoBehaviour
{
    private VisualElement root;

    [Header("Configuración de Niveles")]
    [SerializeField] private int totalNiveles = 8; 

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // 1. Leemos el progreso (basado en tu GameManager)
        int nivelCompletado = PlayerPrefs.GetInt("nivelCompletado", 0);
        int nivelHabilitado = nivelCompletado + 1;

        // 2. Configuramos los 8 botones
        for (int i = 1; i <= totalNiveles; i++)
        {
            string nombreBoton = "btn-" + i;
            Button btn = root.Q<Button>(nombreBoton);

            if (btn != null)
            {
                if (i <= nivelHabilitado)
                {
                    // --- NIVEL DISPONIBLE ---
                    btn.SetEnabled(true); 
                    btn.text = i.ToString(); // Muestra el número del nivel
                    
                    int numeroCarga = i;
                    btn.clicked += () => CargarNivel(numeroCarga);
                }
                else
                {
                    // --- NIVEL BLOQUEADO ---
                    btn.SetEnabled(false); // Esto lo hace opaco automáticamente
                    btn.text = ""; // No muestra número si está bloqueado
                }
            }
        }

        // 3. Botón Play (al nivel más alto disponible)
        Button btnPlay = root.Q<Button>("btn-play");
        if (btnPlay != null)
        {
            btnPlay.clicked += () => CargarNivel(nivelHabilitado);
        }
    }


    void CargarNivel(int numero)
    {
        // Importante para que tu GameManager sepa qué nivel cargar
        PlayerPrefs.SetInt("nivelActual", numero);
        PlayerPrefs.Save();

        // Carga la escena (Asegúrate que se llamen Nivel_01, Nivel_02, etc.)
        SceneManager.LoadScene("Nivel_" + numero.ToString("D2"));
    }
}