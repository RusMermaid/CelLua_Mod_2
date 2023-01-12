using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nudge : TrackedCell
{
    [HideInInspector]
    public bool nudged = false;

    public SpriteRenderer on;
    
    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, rotation, generated);
        nudged = false;
        GetComponent<SpriteRenderer>().enabled = true;
        on.enabled = false;
    }

    public override void Step()
    {
        if(nudged == true)
        {
            this.Push(this.getDirection(), 0);
            this.suppresed = false;
        }
    }

    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e pusher = CellType_e.DUMMY)
    {
        if (nudged == false)
        {
            nudged = true;
            on.enabled = true;
            GetComponent<SpriteRenderer>().enabled = false;
            return base.Push(dir, bias);
        }

        if(this.suppresed)
            return base.Push(dir, bias);
        if (this.getDirection() == dir)
        {
            bias += 1;
        }

        //if bias is opposite our direction
        else if ((int)(dir + 2) % 4 == (int)this.getDirection()) {
            bias -= 1;
        }

        return base.Push(dir, bias);
    }
}
