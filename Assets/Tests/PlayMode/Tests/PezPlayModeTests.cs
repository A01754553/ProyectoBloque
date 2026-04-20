using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PezPlayModeTests
{
    private GameManager gameManager;
    private Pez pez;
    private AnzueloMovimiento anzuelo;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // ignorar errores del asset pack de peces
        LogAssert.ignoreFailingMessages = true;

        SceneManager.LoadScene("Nivel_01");
        yield return null;
        yield return null;

        // reactivar despues de cargar
        LogAssert.ignoreFailingMessages = false;

        gameManager = Object.FindAnyObjectByType<GameManager>();
        anzuelo = Object.FindAnyObjectByType<AnzueloMovimiento>();
        pez = Object.FindAnyObjectByType<Pez>();

        Assert.IsNotNull(gameManager, "No se encontro GameManager en la escena.");
        Assert.IsNotNull(anzuelo, "No se encontro AnzueloMovimiento en la escena.");
        Assert.IsNotNull(pez, "No se encontro Pez en la escena.");
    }

    [UnityTest]
    public IEnumerator Pez_SilabaCorrecta_SumaAcierto()
    {
        // ARRANGE
        int aciertosIniciales = gameManager.aciertos;
        string silaba = GetPrivateField<string>(pez, "silaba");
        SetPrivateField(gameManager, "silabas", new string[] { silaba });

        // ACT
        gameManager.ResponderPez(silaba, pez);
        yield return null;

        // ASSERT
        Assert.AreEqual(aciertosIniciales + 1, gameManager.aciertos,
            "El pez con silaba correcta no sumo un acierto.");
    }

    [UnityTest]
    public IEnumerator Pez_SilabaIncorrecta_SumaFallo()
    {
        // ARRANGE
        int fallosIniciales = gameManager.fallos;

        // ACT
        gameManager.ResponderPez("ZZZZZ", pez);
        yield return null;

        // ASSERT
        Assert.AreEqual(fallosIniciales + 1, gameManager.fallos,
            "El pez con silaba incorrecta no sumo un fallo.");
    }

    [UnityTest]
    public IEnumerator Pez_Regresar_ResetaPosicion()
    {
        // ARRANGE
        Vector3 posicionOriginal = GetPrivateField<Vector3>(pez, "posicionOriginal");
        pez.transform.position = new Vector3(999f, 999f, 0f);

        // ACT
        pez.Regresar();
        yield return null;

        // ASSERT
        // ASSERT
        Assert.That(pez.transform.position.x, Is.EqualTo(posicionOriginal.x).Within(0.1f),
            "El pez no regreso a su posicion original en X.");
        Assert.That(pez.transform.position.y, Is.EqualTo(posicionOriginal.y).Within(0.1f),
            "El pez no regreso a su posicion original en Y.");
    }

    private static T GetPrivateField<T>(object instance, string fieldName)
    {
        FieldInfo field = instance.GetType().GetField(
            fieldName,
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        Assert.IsNotNull(field,
            $"No se encontro el campo privado '{fieldName}' en {instance.GetType().Name}.");
        return (T)field.GetValue(instance);
    }

    private static void SetPrivateField<T>(object instance, string fieldName, T value)
    {
        FieldInfo field = instance.GetType().GetField(
            fieldName,
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        Assert.IsNotNull(field,
            $"No se encontro el campo privado '{fieldName}' en {instance.GetType().Name}.");
        field.SetValue(instance, value);
    }
}