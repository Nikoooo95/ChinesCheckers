using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour {

    //[SerializeField] private BoardIA board;
    VirtualBoard virtualBoard;

	//public byte activePlayer;
	[Range(2, 10)] public byte MAX_DEPTH = 2;
	private int reps = 0;

	public static IA instance;
	public GameControllerIA gameController;



    void Start (){
		instance = this;
	}
    
    /// <summary>
    /// Jugada de la IA
    /// </summary>
    /// <param name="_activePlayer"></param>
    public void Play (byte _activePlayer){

		Movement movement;
		DateTime timeBefore, timeAfter;

		CheckBoard ();
		reps = 0;
		timeBefore = DateTime.Now;
		movement = CheckMovementWhiteCheckers (BoardIA.instance.whiteCheckers);
        //movement = NegamaxAB(0, virtualBoard, 0,0, int.MinValue, int.MaxValue);

        timeAfter = DateTime.Now;
        MoveChecker(movement);
        GameControllerIA.instance.turn = 1;
	}
    
    /// <summary>
    /// Algoritmo que siga la IA para realizar su jugada.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="depth"></param>
    /// <param name="checkerIndex"></param>
    /// <param name="alfa"></param>
    /// <param name="beta"></param>
    /// <returns>Devuelve el movimiento a realizar</returns>
	private Movement NegamaxAB(VirtualBoard board, byte depth, byte checkerIndex, int alfa, int beta){
 
		reps++;

		byte bestMove = 0;
		int bestScore = 0;
		int currentScore = 0;
		Movement movement = null;
		VirtualBoard newBoard;

        if(GameControllerIA.instance.isFinished || depth == MAX_DEPTH)
        {
			if (depth % 2 == 0) {
				movement = new Movement (board.Evaluar (), 0, checkerIndex);
			} else {
				movement = new Movement (-board.Evaluar (), 0, checkerIndex);
			}
        }
        else
        {
			bestScore = int.MinValue;
            List<int> possibleMoves;
			possibleMoves = board.getPositions(checkerIndex);
            foreach(byte move in possibleMoves)
            {
                newBoard = board.generateBoard(move, checkerIndex);

                foreach(byte index in newBoard.getSpacesIndex())
                {
                    movement = NegamaxAB( newBoard, (byte)(depth + 1), index, -beta, -Math.Max(alfa, bestScore));
                    currentScore = -movement.score;

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestMove = move;
                    }

                    if (bestScore >= beta)
                    {
                        movement = new Movement(bestScore, bestMove,0, checkerIndex);
                        return movement;
                    }
                }
                
            }
			movement = new Movement(bestScore, bestMove, 0, checkerIndex);
		}
        
		return movement;
	}
    

        /*
    private Movement NegamaxAB(byte activePlayer, VirtualBoard board,byte indexCurrent, byte depth, int alfa, int beta)
    {

        reps++;

        byte bestMove = 0;
        int bestScore = 0;
        //	byte bestDepth = MAX_DEPTH;
        int currentScore = 0;
        Movement movement = null;
        //Movement movement2 = null;
        VirtualBoard newBoard;

        if (GameControllerIA.instance.isFinished || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {
                movement = new Movement(board.Evaluar(activePlayer), 0, indexCurrent);
            }
            else
            {
                movement = new Movement(-board.Evaluar(activePlayer), 0, indexCurrent);
            }
        }
        else
        {
            if (activePlayer == 0 || activePlayer == 1)
            {
                for (int i = 0; i < board.getSpacesIndex(activePlayer).Count; i++)
                {
                    bestScore = int.MinValue;
                    List<int> posibleMoves = board.getPositions(board.getSpacesIndex(activePlayer)[i]);
                    foreach (byte move in posibleMoves)
                    {
                        newBoard = board.generateBoard(move, (byte)i);
                        movement = NegamaxAB((activePlayer == 1 ? (byte)0 : (byte)1), newBoard,(byte)i, (byte)(depth + 1), -beta, -Math.Max(alfa, bestScore));
                        currentScore = -movement.score;
                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            bestMove = move;
                        }
                        if (bestScore >= beta)
                        {
                            movement = new Movement(bestScore, bestMove, 0, (byte)i);
                            return movement;
                        }

                    }
                    
                }
                movement = new Movement(bestScore, bestMove, 0, indexCurrent);
            }
            else
            {
                Debug.Log("Error active Player mimnimaz");
            }
        }

        return movement;
    }
    */

    /// <summary>
    /// Observacion del tablero
    /// </summary>
    private void CheckBoard(){

        virtualBoard = new VirtualBoard();
        virtualBoard.activePlayer = 0;
        virtualBoard.setSpaces(BoardIA.instance.GetSpaces());
        virtualBoard.showSpaces();
    }
    
    /// <summary>
    /// Inidica al controlador el movimiento que quiere realizar
    /// </summary>
    /// <param name="tempMovement"></param>
    private void MoveChecker(Movement tempMovement)
    {
        GameControllerIA.instance.MoveChecker(tempMovement.movement, tempMovement.checkerIndex);
    }

    /// <summary>
    /// Llama al algoritmo Negamax por cada ficha blanca del tablero
    /// </summary>
    /// <param name="whiteCheckers"></param>
    /// <returns></returns>
	private Movement CheckMovementWhiteCheckers(List<Checker> whiteCheckers){
		Movement movement = null;
		byte bestMove = 0;
		int bestScore = int.MinValue;
		byte bestDepth = MAX_DEPTH;
		byte checkerIndex = 100;
		for (int i = 0; i < whiteCheckers.Count; i++) {
			movement = NegamaxAB (virtualBoard, 0, whiteCheckers[i].getBox().getIndex(), int.MinValue, int.MaxValue);
            if (movement.score > bestScore)
			{
                bestScore = movement.score;
                bestMove = movement.movement;
                bestDepth = movement.depth;
                checkerIndex = whiteCheckers[i].getBox().getIndex();
            }
        }
        movement = new Movement(bestScore, bestMove, bestDepth, checkerIndex);
        
		return movement;
	}



}
