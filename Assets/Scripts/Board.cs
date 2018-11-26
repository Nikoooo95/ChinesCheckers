using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    /// <summary>
    /// Gameobject que contiene el prefab de la ficha
    /// </summary>
    [SerializeField] private GameObject checkerPrefab;

    /// <summary>
    /// Array de materiales de las fichas
    /// </summary>
    [SerializeField] private Material[] checkerMaterials;

    /// <summary>
    /// Material de las casillas iniciales
    /// </summary>
    [SerializeField] private Material startBoxMaterials;

    /// <summary>
    /// Cantidad de columnas rellenas con fichas por jugador
    /// </summary>
    [SerializeField]
    [Tooltip("Indica el numero de columnas que ocupan las fichas al inicio")]
    [Range(2, 8)]
    private int initialCheckers = 4;

    /// <summary>
    /// Array de casillas del tablero
    /// </summary>
    public Box[] boxes;

    /// <summary>
    /// Tamaño de fila y columna del tablero
    /// </summary>
    private int boardSize = 9;

    /// <summary>
    /// Lista con los posible movimientos que puede hacer el jugador
    /// </summary>
    private List<int> posiblesMoves;

    /// <summary>
    /// Lista con las casillas ocupadas por fichas blancas
    /// </summary>
    private List<Box> whiteBoxes;

    /// <summary>
    /// Lista con las casillas ocupadas por las fichas negras
    /// </summary>
    private List<Box> blackBoxes;

    /// <summary>
    /// Lista de fichas blancas
    /// </summary>
    private List<Checker> whiteCheckers;

    /// <summary>
    /// Lista de fichas negras
    /// </summary>
    private List<Checker> blackCheckers;

    /// <summary>
    /// Array con los valores de las casillas.
    /// Contiene 0 si esta vacio.
    /// Contiene 1 si esta ocupado por las fichas blancas.
    /// Contiene 2 si esta ocupado por las fichas negras
    /// </summary>
    public byte[] spaces;


    /// <summary>
    /// Inicializa con los valores
    /// </summary>
    void Start () {
        whiteBoxes = new List<Box>();
        blackBoxes = new List<Box>();
        posiblesMoves = new List<int>();
        whiteCheckers = new List<Checker>();
        blackCheckers = new List<Checker>();
        spaces = new byte[boxes.Length];
        setStartBoard();

        for(byte i = 0; i< boxes.Length; ++i)
        {
            boxes[i].setIndex(i);
        }
	}

    /// <summary>
    /// Devuelve  el numero de casillas iniciales
    /// </summary>
    /// <returns></returns>
    public int getNumStartBoxes()
    {
        return whiteBoxes.Count;
    }

    /// <summary>
    /// Devuelve el numero de casillas blancas ocupadas
    /// </summary>
    /// <returns></returns>
    public byte getNumWhiteBoxesBusies()
    {
        
        byte num = 0;
        foreach (Box b in whiteBoxes)
        {
            if (spaces[b.getIndex()] == 2)
                ++num;
        }
        return num;
    }

    /// <summary>
    /// Devuelve el numero de casillas negras ocupadas
    /// </summary>
    /// <returns></returns>
    public byte getNumBlackBoxesBusies()
    {
        byte num = 0;
        foreach (Box b in blackBoxes)
        {
            if (spaces[b.getIndex()] == 1)
                ++num;
        }

        return num;
    }

    /// <summary>
    /// Llama al método setStartBoxes()
    /// </summary>
    private void setStartBoard()
    {
        setStartBoxes();
    }

    /// <summary>
    /// Comprueba si contiene una posicion dada entre los posibles moviemiento
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool checkPosibleBox(byte index)
    {
        return posiblesMoves.Contains(index);
    }
    /// <summary>
    /// Establece las casillas y fichas iniciales para la partida
    /// </summary>
    private void setStartBoxes()
    {
        int index, i, j = 0;
        for(i = 0; i < initialCheckers - j; i++)
        {
            for(j = 0; j< initialCheckers - i; j++)
            {
                index = boardSize * i + j;
                
                boxes[index].setIsBusy(true);
                boxes[index].setIsStart(true);
                whiteCheckers.Add(setStartChecker(boxes[index], checkerMaterials[0], (byte)0));
                setStartBox(boxes[index]);
                whiteBoxes.Add(boxes[index]);
                spaces[index] = 1;


                boxes[boxes.Length - 1 - index].setIsBusy(true);
                boxes[boxes.Length - 1 - index].setIsStart(true);
                blackCheckers.Add(setStartChecker(boxes[boxes.Length -1 - index], checkerMaterials[1], (byte)1));
                setStartBox(boxes[boxes.Length - 1 - index]);
                blackBoxes.Add(boxes[boxes.Length - 1 - index]);
                spaces[index] = 1;
            }
            j = 0;
        }
    }

    /// <summary>
    /// Establece el estado de cada ficha en el momento de iniciar una partida
    /// </summary>
    /// <param name="box"></param>
    /// <param name="mat"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    private Checker setStartChecker(Box box, Material mat, byte player)
    {
        Checker c = Instantiate(checkerPrefab).GetComponent<Checker>();
        c.setPlayer(player);
        c.setBox(box);
        MeshRenderer[] m =  c.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in m)
        {
            mesh.material = mat;
        }
        box.setChecker(c);
        box.isBusy = true;

        return c;
    }

    /// <summary>
    /// Establece el material de las casillas iniciales
    /// </summary>
    /// <param name="box"></param>
    private void setStartBox(Box box)
    {
        box.gameObject.GetComponent<MeshRenderer>().material = startBoxMaterials;
    }

    /// <summary>
    /// Muestra las posibles casillas a las que una ficha se puede mover
    /// </summary>
    /// <param name="list"></param>
    public void showPosiblesBoxes(List<int> list)
    {
        posiblesMoves = list;
        
        foreach(int i in posiblesMoves)
        {
            boxes[i].setProjectorVisibility(true);
        }
    }

    /// <summary>
    /// Oculta la marca sobre las posibles casillas a las que una ficha podía moverse
    /// </summary>
    public void hidePosiblesBoxes()
    {
        foreach (int i in posiblesMoves)
        {
            boxes[i].setProjectorVisibility(false);
        }
    }

    /// <summary>
    /// Comprueba si una casilla está ocupada o no
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
	private bool checkBoxesBusy(int box)
    {
		return boxes [box].isBusy;
    }
    
    /// <summary>
    /// Comprueba sobre que casos se puede mover en un indice concreto.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="plus"></param>
    /// <returns></returns>
    bool checkIndex(byte index,int plus)
    {
        bool result = false;
        switch (plus)
        {
            case -10:
                if (index >=9 && index % 9 != 0)
                    result = true;
                break;
            case -9:
                if (index >= 9)
                    result = true;
                break;
            case -8:
                if (index >= 9 && ((index-8) % 9) != 0)
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
                if (index <=71)
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
    /// Devuelve 0 si no se puede mover a una posicion
    /// Devuelve 1 si puede moverse a dicha posicion
    /// Devuelve 2 si puede saltar.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="plus"></param>
    /// <param name="jump"></param>
    /// <returns></returns>
    byte getPosition (byte index, int plus, bool jump)
    {
        if(checkIndex(index, plus))
        {
            if (!boxes[index + plus].isBusy)
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
    /// Devuelve una lista con las posiciones a las que una ficha puede moverse.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<int> getPositions(byte index)
    {
        int[] arr = { -10, -9, -8, -1, 1, 8, 9, 10 };
        List<int> list = new List<int>();
        for (int i = 0; i < arr.Length; ++i)
        {
            switch(getPosition(index, arr[i], true))
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
}
