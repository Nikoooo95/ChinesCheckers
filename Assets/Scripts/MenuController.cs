using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	/// <summary>
    /// Carga el nivel que se le pasa por parametro
    /// </summary>
    /// <param name="level"></param>
    public void loadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Sale del juego
    /// </summary>
    public void quit()
    {
        Application.Quit();
    }
}
