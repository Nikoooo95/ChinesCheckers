using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box: MonoBehaviour
{

    
    /// <summary>
    /// False si no está ocupada. True si está ocupada
    /// </summary>
    public bool isBusy;

    /// <summary>
    /// False si no es de inicio. True si es de inicio
    /// </summary>
    private bool isStart;

    /// <summary>
    /// Ficha que le corresponde
    /// </summary>
    public Checker checker;

    /// <summary>
    /// Proyector
    /// </summary>
    private Projector projector;

    /// <summary>
    /// Indice de la casilla dentro del tablero
    /// </summary>
    private byte index;

    /// <summary>
    /// Establece los valores por defecto de las variables
    /// </summary>
    private void Awake()
    {
        
        isBusy = false;
        isStart = false;
        checker = null;

        projector = GetComponentInChildren<Projector>();
        projector.enabled = false;
    }

    /// <summary>
    /// Devuelve la posicion que debe tomar la ficha cuando ocupa la casilla
    /// </summary>
    /// <returns></returns>
    private Vector3 getCheckerPosition()
    {
        Vector3 temp = transform.position;
        temp.y += 1.2f;
        return temp;
    }

    /// <summary>
    /// Settea una ficha en la casilla
    /// </summary>
    /// <param name="_checker"></param>
    public void setChecker(Checker _checker)
    {
        
        checker = _checker;
        if(_checker != null)
            checker.transform.SetPositionAndRotation(getCheckerPosition(), Quaternion.identity);
    }

    /// <summary>
    /// Devuelve la ficha que ocupa la casilla
    /// </summary>
    /// <returns></returns>
    public Checker getChecker()
    {
        return checker;
    }

    /// <summary>
    /// Settea el indice de la casilla
    /// </summary>
    /// <param name="i"></param>
    public void setIndex(byte i)
    {
        this.index = i;
    }

    /// <summary>
    /// Devuelve el indice
    /// </summary>
    /// <returns></returns>
    public byte getIndex()
    {
        return index;
    }

    public bool getIsBusy() { return isBusy; }
    public void setIsBusy(bool b) { isBusy = b; }

    public bool getIsStart() { return isStart; }
    public void setIsStart(bool b) { isStart = b; }

    /// <summary>
    /// Modifica la visibilidad del proyector
    /// </summary>
    /// <param name="visibility"></param>
    public void setProjectorVisibility(bool visibility)
    {
        projector.enabled = visibility;
    }


}
