using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerIA : MonoBehaviour {

	[Tooltip("True si ha terminado la partida. False si no ha terminado la partida.")]
	public  bool isFinished;

    //Turno IA: 0. Turno jugador: 1
    [Tooltip("Turno IA: 0. Turno jugador: 1.")]
	public byte turn;

	[SerializeField]
	private CanvasManager canvas;

	//True si ha seleccionado una ficha. False si no.
	private bool selectedChecker;
	//Pieza seleccionada
	private Checker tempChecker;

	//Tablero
	public BoardIA board;

    public static GameControllerIA instance;

	private void Awake()
	{
        instance = this;
		turn = 1;
		canvas.setPlayerTurn(turn);
		isFinished = false;
	}

    /// <summary>
    /// Se ejecuta el turno del jugador o el de la IA
    /// </summary>
	void Update()
	{
        if (turn == 1 && !isFinished)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {

                    if (!selectedChecker)
                    {

                        if (hit.transform.gameObject.tag == "Checker" &&
                            hit.transform.gameObject.GetComponent<Checker>().getPlayer() == 1 &&
                            turn == 1)
                        {

                            tempChecker = hit.transform.gameObject.GetComponent<Checker>();

                            List<int> tempList = board.getPositions(tempChecker.getBox().getIndex());
                            if (tempList.Count > 0)
                            {

                                ChangeAlphaColor(tempChecker.gameObject, 50);
                                board.showPosiblesBoxes(tempList);
                                selectedChecker = true;
                            }
                        }
                    }
                    else
                    {
                        if (hit.transform.gameObject.tag == "Box")
                        {
                            if (board.checkPosibleBox(hit.transform.GetComponent<Box>().getIndex()))
                            {
                                Box temp = tempChecker.getBox();
                                BoardIA.instance.spaces[temp.getIndex()] = 0;
                                temp.setIsBusy(false);
                                tempChecker.getBox().setChecker(null);
                                hit.transform.gameObject.GetComponent<Box>().setChecker(tempChecker);
                                hit.transform.gameObject.GetComponent<Box>().setIsBusy(true);
                                tempChecker.setBox(hit.transform.gameObject.GetComponent<Box>());

                                board.hidePosiblesBoxes();

                                
                                BoardIA.instance.spaces[hit.transform.gameObject.GetComponent<Box>().getIndex()] = 2;

                                if (!checkVictory(turn))
                                    ChangeTurn();

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
        else if (turn == 0 && !isFinished)
        {
            IA.instance.Play(turn);
        }
        
        checkVictory(turn);
    }


    /// <summary>
    /// Cambia el turno
    /// </summary>
    public void ChangeTurn()
    {   
        turn = turn == 1 ? (byte)0 : (byte)1;
        canvas.setPlayerTurn(turn);
        canvas.updateScore(board.getNumBlackBoxesBusies(), board.getNumWhiteBoxesBusies());
    }

    /// <summary>
    /// Comprueba la victoria del jugador
    /// </summary>
    /// <param name="jugador"></param>
    /// <returns></returns>
	public bool checkVictory(byte jugador)
	{
        if(jugador == 0)
        {
            if (board.getNumStartBoxes() == board.getNumWhiteBoxesBusies())
            {
                canvas.showVictory(2);
                return true;
            }
        }
        else
        {
            if (board.getNumStartBoxes() == board.getNumBlackBoxesBusies())
            {
                canvas.showVictory(1);
                return true;
            }
        }

        return false;

    }

    /// <summary>
    /// Modifica el color del gameobject
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

    /// <summary>
    /// Mueve la ficha a la posicion que recibe por parametro
    /// </summary>
    /// <param name="position"></param>
    /// <param name="tempChecker"></param>
    public void MoveChecker(byte position, byte checkerIndex)
    {
        Checker newChecker = BoardIA.instance.boxes[checkerIndex].getChecker();
        newChecker.getBox().setIsBusy(false);
        BoardIA.instance.spaces[checkerIndex] = 0;
        newChecker.getBox().setChecker(null);
        board.boxes[position].setChecker(newChecker);
        board.boxes[position].setIsBusy(true);
        newChecker.setBox(board.boxes[position]);

        ChangeTurn();
        canvas.setPlayerTurn(turn);

        canvas.updateScore(board.getNumBlackBoxesBusies(), board.getNumWhiteBoxesBusies());
        isFinished = checkVictory(turn);

        BoardIA.instance.spaces[position] = 1;
    }

}
