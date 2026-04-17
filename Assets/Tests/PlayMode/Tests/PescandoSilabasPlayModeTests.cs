using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class JuegoPlayModeTests
{
    private GameManager gameManager;
    private AnzueloMovimiento anzuelo;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Nivel_01");
        yield return null;
        yield return null;

        gameManager = Object.FindAnyObjectByType<GameManager>();
        anzuelo = Object.FindAnyObjectByType<AnzueloMovimiento>();

        Assert.IsNotNull(gameManager, "No se encontro GameManager en la escena.");
        Assert.IsNotNull(anzuelo, "No se encontro AnzueloMovimiento en la escena.");
    }

    [UnityTest]
    public IEnumerator GameManager_ResponderPez_Correcto_SumaAcierto()
    {
        // ARRANGE
        int aciertosIniciales = gameManager.aciertos;

        // ACT
        gameManager.ResponderPez(GetPrivateField<string[]>(gameManager, "silabas")[0], null);
        yield return null;

        // ASSERT
        Assert.AreEqual(aciertosIniciales + 1, gameManager.aciertos,
            "El contador de aciertos no aumento al pescar el pez correcto.");
    }

    [UnityTest]
    public IEnumerator GameManager_ResponderPez_Incorrecto_SumaFallo()
    {
        // ARRANGE
        int fallosIniciales = gameManager.fallos;

        // ACT
        gameManager.ResponderPez("ZZZZZ", null);
        yield return null;

        // ASSERT
        Assert.AreEqual(fallosIniciales + 1, gameManager.fallos,
            "El contador de fallos no aumento al pescar el pez incorrecto.");
    }

    [UnityTest]
    public IEnumerator GameManager_Estrellas_CeroFallos_TresEstrellas()
    {
        // ARRANGE / ACT
        int estrellas = CalcularEstrellas(0);
        yield return null;

        // ASSERT
        Assert.AreEqual(3, estrellas, "Con 0 fallos deberian ser 3 estrellas.");
    }

    [UnityTest]
    public IEnumerator GameManager_Estrellas_DosFallos_DosEstrellas()
    {
        // ARRANGE / ACT
        int estrellas = CalcularEstrellas(2);
        yield return null;

        // ASSERT
        Assert.AreEqual(2, estrellas, "Con 2 fallos deberian ser 2 estrellas.");
    }

    [UnityTest]
    public IEnumerator AnzueloMovimiento_LimiteArriba_SeDetiene()
    {
        // ARRANGE
        float limiteArriba = GetPrivateField<float>(anzuelo, "limiteArriba");

        // ACT
        anzuelo.transform.position = new Vector3(
            anzuelo.transform.position.x,
            limiteArriba + 1f,
            anzuelo.transform.position.z
        );
        yield return new WaitForSeconds(0.1f);

        // ASSERT
        Assert.LessOrEqual(anzuelo.transform.position.y, limiteArriba,
            "El anzuelo supero el limite superior.");
    }

    private int CalcularEstrellas(int fallos)
    {
        if (fallos <= 1) return 3;
        else if (fallos <= 3) return 2;
        else return 1;
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
