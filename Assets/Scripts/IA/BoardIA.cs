using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardIA : MonoBehaviour
{

    /// <summary>
    /// Prefab de una ficha
    /// </summary>
    [SerializeField] private GameObject checkerPrefab;
    /// <summary>
    /// Materiales de las fichas(blancas o negras)
    /// </summary>
    [SerializeField] private Material[] checkerMaterials;
    /// <summary>
    /// Material de las casillas de inicio
    /// </summary>
    [SerializeField] private Material startBoxMaterials;

    [SerializeField]
    [Tooltip("Indica el numero de columnas que ocupan las fichas al inicio")]
    [Range(1, 8)]
    private int initialCheckers = 4;

    /// <summary>
    /// Array unidimensional de casillas
    /// </summary>
    public Box[] boxes;

    /// <summary>
    /// Dimension del tablero
    /// </summary>
    private int boardSize;

    /// <summary>
    /// Posibles movimientos de una ficha en el tablero
    /// </summary>
    private List<int> posiblesMoves;

    /// <summary>
    /// Conjunto de casillas iniciales blancas
    /// </summary>
    private List<Box> whiteBoxes;
    /// <summary>
    /// Conjunto de casillas iniciales negras
    /// </summary>
    private List<Box> blackBoxes;

    /// <summary>
    /// Conjunto de fichas blancas
    /// </summary>
    public List<Checker> whiteCheckers;
    /// <summary>
    /// Conjunto de fichas blancas
    /// </summary>
    public List<Checker> blackCheckers;

    /// <summary>
    /// Jugador activo
    /// </summary>
    public byte activePlayer;

    /// <summary>
    /// Array que representa el tablero real mediante numeros.
    /// Vacio: 0, Blancas: 1, Negras: 2
    /// </summary>
    public byte[]  spaces;

    /// <summary>
    /// Singleton
    /// </summary>
    public static BoardIA instance;

    void Start()
    {
        instance = this;
        whiteBoxes = new List<Box>();
        blackBoxes = new List<Box>();
        posiblesMoves = new List<int>();
        whiteCheckers = new List<Checker>();
        blackCheckers = new List<Checker>();
        spaces = new byte[boxes.Length];
        boardSize = 9;
        setStartBoard();

        for (byte i = 0; i < boxes.Length; ++i)
        {
            boxes[i].setIndex(i);
        }

        
    }

    /// <summary>
    /// Devuelve el numero de casillas de salida
    /// </summary>
    /// <returns></returns>
    public int getNumStartBoxes()
    {
        return whiteBoxes.Count;
    }

    /// <summary>
    /// Devuelve el numero de casillas blancas ocupadas por el contrario
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
    /// Devuelve el numero de casillas negras ocupadas por el contrario
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
    /// Inicializacion del tablero
    /// </summary>
    private void setStartBoard()
    {
        setStartBoxes();
    }

    /// <summary>
    /// Devuelve true cuando el indice de la casilla seleccionada es valido
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool checkPosibleBox(byte index)
    {
        return posiblesMoves.Contains(index);
    }

    /// <summary>
    /// Inicializa las casillas iniciales
    /// </summary>
    private void setStartBoxes()
    {
        int index, i, j = 0;
        for (i = 0; i < initialCheckers - j; i++)
        {
            for (j = 0; j < initialCheckers - i; j++)
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
                blackCheckers.Add(setStartChecker(boxes[boxes.Length - 1 - index], checkerMaterials[1], (byte)1));
                setStartBox(boxes[boxes.Length - 1 - index]);
                blackBoxes.Add(boxes[boxes.Length - 1 - index]);
                spaces[boxes.Length - 1 - index] = 2;
            }
            j = 0;
        }
    }

    /// <summary>
    /// Crea una ficha y la posiciona en su lugar correspondiente, asignandole el color del jugador
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
        MeshRenderer[] m = c.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in m)
        {
            mesh.material = mat;
        }
        box.setChecker(c);
        box.isBusy = true;

        

        return c;
    }

    /// <summary>
    /// Convierte la casilla en una casilla de inicio
    /// </summary>
    /// <param name="box"></param>
    private void setStartBox(Box box)
    {
        box.gameObject.GetComponent<MeshRenderer>().material = startBoxMaterials;
    }

    /// <summary>
    /// Muestra al usuario los posibles movimientos
    /// </summary>
    /// <param name="list"></param>
    public void showPosiblesBoxes(List<int> list)
    {
        posiblesMoves = list;

        foreach (int i in posiblesMoves)
        {
            boxes[i].setProjectorVisibility(true);
        }
    }

    /// <summary>
    /// Oculta los posibles movimientos
    /// </summary>
    public void hidePosiblesBoxes()
    {
        foreach (int i in posiblesMoves)
        {
            boxes[i].setProjectorVisibility(false);
        }
    }

    /// <summary>
    /// Comprueba si la casilla esta ocupada
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
    private bool checkBoxesBusy(int box)
    {
        return boxes[box].isBusy;
    }

    /// <summary>
    /// Comprueba si el indice recibido mas su transformación es un indice valido dentro del tablero
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
    /// Devuelve las posibles posiciones de una ficha segun su indice
    /// </summary>
    /// <param name="index"></param>
    /// <param name="plus"></param>
    /// <param name="jump"></param>
    /// <returns></returns>
    byte getPosition(byte index, int plus, bool jump)
    {
		//Debug.Log ("Index: " + index + ". Plus: " + plus);
        if (checkIndex(index, plus))
        {
            if (!boxes[index + plus].getIsBusy())
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
    /// Devuelve las posiciones de una ficha cuyo indice es la posicion del tablero incluyendo los saltos
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
    /// Modifica el jugador que debe jugar
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private byte ChangePlayer(byte player)
    {
        return (byte)(player == 1 ? 0 : 1);
    }

    /// <summary>
    /// Devuelve el array de espacios
    /// </summary>
    /// <returns></returns>
    public byte[] GetSpaces()
    {
        return spaces;
    }

}
