using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBoard {

    public byte[] spaces;

    public byte activePlayer;

    public short[] matrizEvaluacion =
{
        0, 1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 4, 7, 11, 16, 22, 30, 38,
        2, 4, 8, 15, 26, 42, 64, 94, 132,
        3, 7, 15, 30, 56, 98, 162, 256, 388,
        4, 11, 26, 56, 112, 210, 372, 618, 1006,
        5, 16, 42, 98, 210, 420, 802, 1420, 2426,
        6, 22, 64, 162, 372, 802, 1604, 2024, 4450,
        7, 30, 94, 256, 618, 1420, 2024, 4048, 8498,
        8, 38, 132, 388, 1006, 2426, 4450, 8498, 16996
    };

    /// <summary>
    /// Constructor
    /// </summary>
    public VirtualBoard()
    {
        spaces = new byte[BoardIA.instance.spaces.Length];
        activePlayer = 0;
    }

    /// <summary>
    /// Funcion de evaluacion
    /// </summary>
    /// <returns></returns>
    public short Evaluar()
    {
        
        if (GameControllerIA.instance.checkVictory(0))
        {
            return short.MaxValue;
        }
        else if (GameControllerIA.instance.checkVictory(ChangePlayer(1)))
        {
            return short.MinValue;
        }

        return getScore(spaces);
    }

    /// <summary>
    /// Cambio de jugador
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private byte ChangePlayer(byte player)
    {
        return (byte)(player == 1 ? 0 : 1);
    }

    /// <summary>
    /// Devuelve las posibles posiciones de movimiento segun el indice que recibe, con salto
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<int> getPositions(byte index)
    {
        
        int[] arr = { -10, -9, -8, -1, 1, 8, 9, 10 };
        List<int> list = new List<int>();
        for (int i = 0; i < arr.Length; ++i)
        {
            switch (getPosition(index, arr[i], true))
            {
                case 0:
                    break;
                case 1:
                    list.Add(index + arr[i]);
                    break;
                case 2:
                    list.Add(index + arr[i] + arr[i]);
                    break;
                default:
                    break;
            }
        }
        return list;
    }

    /// <summary>
    /// Devuelve las posibles posiciones de movimiento segun el indice que recibe
    /// </summary>
    /// <param name="index"></param>
    /// <param name="plus"></param>
    /// <param name="jump"></param>
    /// <returns></returns>
    byte getPosition(byte index, int plus, bool jump)
    {
        if (checkIndex(index, plus))
        {
            if (spaces[index + plus] == 0 )
            {
                return 1;
            }
            else
            {
                if (jump)
                {
                    if (getPosition((byte)(index + plus), plus, false) == 1)
                        return 2;
                }
                return 0;
            }
        }
        return 0;

    }

    /// <summary>
    /// Comprueba si el indice mas su incremento es posible
    /// </summary>
    /// <param name="index"></param>
    /// <param name="plus"></param>
    /// <returns></returns>
    bool checkIndex(byte index, int plus)
    {
        bool result = false;
        switch (plus)
        {
            case -10:
                if (index >= 9 && index % 9 != 0)
                    result = true;
                break;
            case -9:
                if (index >= 9)
                    result = true;
                break;
            case -8:
                if (index >= 9 && ((index - 8) % 9) != 0)
                    result = true;
                break;
            case -1:
                if (index % 9 != 0)
                    result = true;
                break;
            case 1:
                if ((index - 8) % 9 != 0)
                    result = true;
                break;
            case 8:
                if (index <= 71 && (index % 9 != 0))
                    result = true;
                break;
            case 9:
                if (index <= 71)
                    result = true;
                break;
            case 10:
                if (index <= 71 && ((index - 8) % 9) != 0)
                    result = true;
                break;
            default:
                break;

        }

        return result;
    }

    /// <summary>
    /// Genera un nuevo tablero a partir del movimiento que recibe por parametro
    /// </summary>
    /// <param name="newPos"></param>
    /// <param name="oldPos"></param>
    /// <returns></returns>
    public VirtualBoard generateBoard(byte newPos, byte oldPos)
    {
        spaces[newPos] = spaces[oldPos];
        spaces[oldPos] = 0;
        activePlayer = ChangePlayer(activePlayer);
        return this;
    }

    /// <summary>
    /// Devuelve los indices en el tablero de las fichas del jugador activo
    /// </summary>
    /// <returns></returns>
    public List<byte> getSpacesIndex()
    {
        List<byte> tempSpaces = new List<byte>();
        for(byte i = 0; i < spaces.Length; i++)
        {
            if(spaces[i] == activePlayer + 1)
            {
                tempSpaces.Add(i);
            }
        }

        return tempSpaces;
    }

    /// <summary>
    /// Muestra los espacios por consola
    /// </summary>
    public void showSpaces()
    {
        string s = "";
        foreach(byte b in spaces)
        {
            s += " " + b;
        }
        Debug.Log(s);
    }

    /// <summary>
    /// Settea los espacios
    /// </summary>
    /// <param name="newSpaces"></param>
    public void setSpaces(byte[] newSpaces)
    {
        for(int i = 0; i < spaces.Length; ++i)
        {
            spaces[i] = newSpaces[i];
        }
    }

    /// <summary>
    /// Devuelve la puntuacion del tablero
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public short getScore(byte[] index)
    {
        short value = 0;
        for (byte i = 0; i < index.Length; ++i)
        {
            
            if (index[i] == activePlayer+1)
            {
                value += matrizEvaluacion[i];
            }

        }

        return value;
    }
}
