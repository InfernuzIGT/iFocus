using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    /// <summary>
    /// On wich scene are we now.
    /// </summary>
    private int actualScene;

    /// <summary>
    /// All the scenes on the application.
    /// </summary>
    private Scene[] scenes;

    public void LoadScene(int sceneToLoad)
    {
        // TODO: Debe encargarse de cargar la escena correspondiente.
    }

    public IEnumerator LoadSceneAsync()
    {
        // TODO: Debe encargarse de ejecutar todos los efectos y transiciones que correspondan a la carga de escena.

        yield return null;
    }

}