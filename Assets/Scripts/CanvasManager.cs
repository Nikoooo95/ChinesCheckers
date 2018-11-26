using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    /// <summary>
    /// Texto que muestra el turno del jugador
    /// </summary>
    [SerializeField] private Text playerTurnText;

    /// <summary>
    /// Imagen que muestra el turno del jugador
    /// </summary>
    [SerializeField] private GameObject[] imagePlayers;

    /// <summary>
    /// Texto que muestra la puntuacion de las fichas blancas
    /// </summary>
    [SerializeField] private Text whiteScore;

    /// <summary>
    /// Texto que muestra la puntuacion de las fichas negras
    /// </summary>
    [SerializeField] private Text blackScore;

    /// <summary>
    /// Canvas que muestra el fin de la partida y quien ha ganado.
    /// </summary>
    [SerializeField] private GameObject victoryCanvas;

    /// <summary>
    /// Texto que muestra el turno del jugador correspondiente
    /// </summary>
    private string defaultText = "TURNO DEL JUGADOR: ";
    
    /// <summary>
    /// Método que se encarga de cambiar el texto del turno del jugador
    /// y de cambiar el color a los iconos
    /// </summary>
    /// <param name="player"></param>
    public void setPlayerTurn(byte player)
    {
        playerTurnText.text = defaultText + (player +1);

        foreach(GameObject g in imagePlayers)
        {
            g.SetActive(false);
        }
        imagePlayers[player].SetActive(true);
    }

    /// <summary>
    /// Método que se encarga de actualizar la puntuación de los jugadores
    /// </summary>
    /// <param name="white"></param>
    /// <param name="black"></param>
    public void updateScore(byte white, byte black)
    {
        whiteScore.text = "" + white;
        blackScore.text = "" +  black;
    }

    /// <summary>
    /// Método que se encarga de mostrar una pantalla de fin de partida
    /// </summary>
    /// <param name="player"></param>
    public void showVictory(int player)
    {
        victoryCanvas.GetComponentInChildren<Text>().text = "Victoria del jugador " + player;
        victoryCanvas.SetActive(true);
    }
}
