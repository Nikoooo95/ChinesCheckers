using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour {

    private byte player;
    private Box box;

    public void setPlayer(byte player)
    {
        this.player = player;
    }

    public void setBox(Box box)
    {
        this.box = box;
    }

    public Box getBox()
    {
        return this.box;
    }

    public byte getPlayer()
    {
        return player;
    }
    
}
