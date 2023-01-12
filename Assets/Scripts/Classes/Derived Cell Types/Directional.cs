using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional : Cell
{
    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e dummy)
    {
        if(dir == this.getDirection())
            return base.Push(dir, bias);
        return (false, false);
    }
}
