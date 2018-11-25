using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    [Tooltip("True si ha terminado la partida. False si no ha terminado la partida.")]
    private bool isFinished;

    [SerializeField]
    [Tooltip("Turno jugador 1: 0. Turno jugador 2: 1.")]
    private byte turn;

    [SerializeField]
    private CanvasManager canvas;

    //True si ha seleccionado una ficha. False si no.
    private bool selectedChecker;

    //Pieza seleccionada
    private Checker tempChecker;

    //Tablero
    [SerializeField] private Board board;




    private void Awake()
    {
        turn = 1;
        canvas.setPlayerTurn(turn);
        isFinished = false;
    }



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
                            Debug.Log("Casilla no posible");
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

    private void checkVictory()
    {
        if (board.getNumStartBoxes() == board.getNumWhiteBoxesBusies())
        {
            Debug.Log("Victoria 2");
            isFinished = true;
            canvas.showVictory(2);
        }
            

        else if (board.getNumStartBoxes() == board.getNumBlackBoxesBusies())
        {
            Debug.Log("Victoria 1");
            isFinished = true;
            canvas.showVictory(1);
        }
            
    }


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
