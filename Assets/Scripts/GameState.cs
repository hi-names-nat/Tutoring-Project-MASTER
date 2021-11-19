using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    int _dungeonKeys = 0;

    public void addKey()
    {
        _dungeonKeys++;
    }

    public void useKey()
    {
        _dungeonKeys--;
    }

    public bool hasKey()
    {
        if (_dungeonKeys > 0) return true; else return false;
    }
}
