using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    /// <summary>
    /// Controla si la partida ha finalizado o no.
    /// </summary>
    [SerializeField]
    [Tooltip("True si ha terminado la partida. False si no ha terminado la partida.")]
    private bool isFinished;

    /// <summary>
    /// Controla el turno del jugador
    /// </summary>
    [SerializeField]
    [Tooltip("Turno jugador 1: 0. Turno jugador 2: 1.")]
    private byte turn;

    /// <summary>
    /// Contiene el controlador del HUD del juego
    /// </summary>
    [SerializeField]
    private CanvasManager canvas;

    /// <summary>
    /// Controla si ha seleccionado una ficha o no.
    /// True si ha seleccionado una ficha. False si no.
    /// </summary>
    private bool selectedChecker;

    /// <summary>
    /// Contiene la ficha seleccionada
    /// </summary>
    private Checker tempChecker;

    /// <summary>
    /// Contiene el tablero de juego
    /// </summary>
    [SerializeField] private Board board;

    /// <summary>
    /// Metodo que inicializa las variables básicas
    /// </summary>
    private void Awake()
    {
        turn = 1;
        canvas.setPlayerTurn(turn);
        isFinished = false;
    }

    /// <summary>
    /// Se encarga de controlar cuando un jugador ha hecho click sobre una ficha
    /// y, dependiendo del turno, sobre que ficha.
    /// Por otro lado, se encarga de llamar a los metodos correspondientes para ver
    /// a que casillas se puede mover una ficha.
    /// Así mismo, se encarga de mover la ficha al lugar que el jugador le ha indicado.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFinished)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {

                if (!selectedChecker)
                {
                    
                    if (hit.transform.gameObject.tag == "Checker" && 
                        hit.transform.gameObject.GetComponent<Checker>().getPlayer() == turn)
                    {
                      
                        tempChecker = hit.transform.gameObject.GetComponent<Checker>();
                        
                        List<int> tempList = board.getPositions(tempChecker.getBox().getIndex());
                        if (tempList.Count > 0)
                        {
                            
                            ChangeAlphaColor(tempChecker.gameObject, 50);
                            board.showPosiblesBoxes(tempList);
                            selectedChecker = true;
                        } else
                            Debug.Log("No tiene movimientos");
                        
                    }
                }
                else
                {
                    if (hit.transform.gameObject.tag == "Box")
                    {
                        if (board.checkPosibleBox(hit.transform.GetComponent<Box>().getIndex())){
                            Box temp = tempChecker.getBox();
                            board.spaces[temp.getIndex()] = 0;
                            temp.setIsBusy(false);
                            tempChecker.getBox().setChecker(null);
                            hit.transform.gameObject.GetComponent<Box>().setChecker(tempChecker);
                            hit.transform.gameObject.GetComponent<Box>().setIsBusy(true);
                            tempChecker.setBox(hit.transform.gameObject.GetComponent<Box>());

                            
                            turn = (turn == 1) ? (byte)0 : (byte)1;
                            canvas.setPlayerTurn(turn);

                            board.hidePosiblesBoxes();

                            board.spaces[hit.transform.gameObject.GetComponent<Box>().getIndex()] = (byte)(turn+1);

                            canvas.updateScore(board.getNumBlackBoxesBusies(), board.getNumWhiteBoxesBusies());
                            checkVictory();
                        }
                        else
                        {
                            board.hidePosiblesBoxes();
                        }
                        ChangeAlphaColor(tempChecker.gameObject, 255);
                        selectedChecker = false;
                        tempChecker = null;
                    }
                    else
                    {
                        selectedChecker = false;
                        board.hidePosiblesBoxes();
                        ChangeAlphaColor(tempChecker.gameObject, 255);
                        selectedChecker = false;
                        tempChecker = null;
                    }
                        
                }   
            }
        }
    }

    /// <summary>
    /// Se encarga de comprobar la victoria y el final de partida
    /// </summary>
    private void checkVictory()
    {
        if (board.getNumStartBoxes() == board.getNumWhiteBoxesBusies())
        {
            //Debug.Log("Victoria 2");
            isFinished = true;
            canvas.showVictory(2);
        }
        else if (board.getNumStartBoxes() == board.getNumBlackBoxesBusies())
        {
            //Debug.Log("Victoria 1");
            isFinished = true;
            canvas.showVictory(1);
        }
            
    }

    /// <summary>
    /// Se encarga de cambiar el material de una ficha cuando esta ha sido seleccionada
    /// </summary>
    /// <param name="tempG"></param>
    /// <param name="value"></param>
    void ChangeAlphaColor(GameObject tempG, byte value)
    {
        MeshRenderer[] m = tempG.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in m)
        {
            Color32 col = mesh.material.GetColor("_Color");
            col.a = value;
            mesh.material.SetColor("_Color", col);
        }
    }

     

   
}
