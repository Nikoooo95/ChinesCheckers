using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {


    [SerializeField] private GameObject checkerPrefab;
    [SerializeField] private Material[] checkerMaterials;
    [SerializeField] private Material startBoxMaterials;

    [SerializeField]
    [Tooltip("Indica el numero de columnas que ocupan las fichas al inicio")]
    [Range(2, 8)]
    private int initialCheckers = 4;

    public Box[] boxes;

    private int boardSize = 9;

    private List<int> posiblesMoves;

    private List<Box> whiteBoxes;
    private List<Box> blackBoxes;

    private List<Checker> whiteCheckers;
    private List<Checker> blackCheckers;

    public byte[] spaces;


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

    public int getNumStartBoxes()
    {
        return whiteBoxes.Count;
    }

    public byte getNumWhiteBoxesBusies()
    {
        
        byte num = 0;
        foreach (Box b in blackBoxes)
        {
            if (spaces[b.getIndex()] == 1)
                ++num;
        }
        return num;
    }

    public byte getNumBlackBoxesBusies()
    {
        byte num = 0;
        foreach (Box b in blackBoxes)
        {
            if (spaces[b.getIndex()] == 2)
                ++num;
        }

        return num;
    }

    private void setStartBoard()
    {
        setStartBoxes();
    }

    public bool checkPosibleBox(byte index)
    {
        return posiblesMoves.Contains(index);
    }

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

    private void setStartBox(Box box)
    {
        box.gameObject.GetComponent<MeshRenderer>().material = startBoxMaterials;
    }

    public void showPosiblesBoxes(List<int> list)
    {
        posiblesMoves = list;
        
        foreach(int i in posiblesMoves)
        {
            boxes[i].setProjectorVisibility(true);
        }
    }

    public void hidePosiblesBoxes()
    {
        foreach (int i in posiblesMoves)
        {
            boxes[i].setProjectorVisibility(false);
        }
    }

	private bool checkBoxesBusy(int box)
    {
		return boxes [box].isBusy;
    }
    
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
