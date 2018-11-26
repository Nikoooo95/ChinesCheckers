using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour {

    /// <summary>
    /// Controla a que jugador pertenece la ficha
    /// </summary>
    private byte player;

    /// <summary>
    /// Controla sobre que ficha se encuentra posicionada
    /// </summary>
    private Box box;

    public void setPlayer(byte player){ this.player = player; }
    public void setBox(Box box){ this.box = box; }
    public Box getBox(){ return this.box; }
    public byte getPlayer(){ return player; }
    
}
