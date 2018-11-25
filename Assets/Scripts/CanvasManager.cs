using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    [SerializeField] private Text playerTurnText;
    [SerializeField] private GameObject[] imagePlayers;

    [SerializeField] private Text whiteScore;
    [SerializeField] private Text blackScore;

    [SerializeField] private GameObject victoryCanvas;

    private string defaultText = "TURNO DEL JUGADOR: ";
    

    public void setPlayerTurn(byte player)
    {
        playerTurnText.text = defaultText + (player +1);

        foreach(GameObject g in imagePlayers)
        {
            g.SetActive(false);
        }
        imagePlayers[player].SetActive(true);
    }

    public void updateScore(byte white, byte black)
    {
        whiteScore.text = "" + white;
        blackScore.text = "" +  black;
    }

    public void showVictory(int player)
    {
        victoryCanvas.GetComponentInChildren<Text>().text = "Victoria del jugador " + player;
        victoryCanvas.SetActive(true);
    }
}
