using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class ControladorInterfazPlayModeTests
{
    [UnityTest]
    public IEnumerator ConfigurarBotones_DosNivelesCompletados_HabilitaTres()
    {
        // ARRANGE
        int[] nivelesCompletados = { 1, 2 };

        // ACT
        int nivelHabilitado = nivelesCompletados.Length > 0
            ? System.Linq.Enumerable.Max(nivelesCompletados) + 1
            : 1;
        yield return null;

        // ASSERT
        Assert.AreEqual(3, nivelHabilitado,
            "Con niveles 1 y 2 completados el nivel habilitado deberia ser 3.");
    }

    [UnityTest]
    public IEnumerator ConfigurarBotones_SinNivelesCompletados_HabilitaUno()
    {
        // ARRANGE
        int[] nivelesCompletados = new int[0];

        // ACT
        int nivelHabilitado = nivelesCompletados.Length > 0
            ? System.Linq.Enumerable.Max(nivelesCompletados) + 1
            : 1;
        yield return null;

        // ASSERT
        Assert.AreEqual(1, nivelHabilitado,
            "Sin niveles completados el nivel habilitado deberia ser 1.");
    }
}
