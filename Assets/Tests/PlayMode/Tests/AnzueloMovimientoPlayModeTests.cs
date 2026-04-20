using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AnzueloMovimientoPlayModeTests
{
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

        anzuelo = Object.FindAnyObjectByType<AnzueloMovimiento>();
        Assert.IsNotNull(anzuelo, "No se encontro AnzueloMovimiento en la escena.");
    }

    [UnityTest]
    public IEnumerator AnzueloMovimiento_NoSuperaLimiteSuperior()
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

    [UnityTest]
    public IEnumerator AnzueloMovimiento_NoSuperaLimiteInferior()
    {
        // ARRANGE
        float limiteAbajo = GetPrivateField<float>(anzuelo, "limiteAbajo");

        // ACT
        anzuelo.transform.position = new Vector3(
            anzuelo.transform.position.x,
            limiteAbajo - 1f,
            anzuelo.transform.position.z
        );
        yield return new WaitForSeconds(0.1f);

        // ASSERT
        Assert.GreaterOrEqual(anzuelo.transform.position.y, limiteAbajo,
            "El anzuelo supero el limite inferior.");
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
}
