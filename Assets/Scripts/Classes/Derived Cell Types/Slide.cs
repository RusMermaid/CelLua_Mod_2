using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : Cell
{
    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e pusher = CellType_e.DUMMY)
    {
        if(dir == this.getDirection() || ((int)dir + 2) % 4 == (int)this.getDirection())
            return base.Push(dir, bias);
        return (false, false);
    }
}
