using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : TrackedCell
{
    private bool destroy = false;

    public override void Step()
    {
        int fallDistance = 0;

        Check:
        {
            if (this.position.y - fallDistance - 1 < 0)
            {
                if (fallDistance != 0)
                    this.setPosition((int)this.position.x, (int)this.position.y - fallDistance);
            }
            else if (CellFunctions.cellGrid[(int)this.position.x, (int)this.position.y - fallDistance - 1] == null)
            {
                fallDistance++;
                goto Check;
            }
            else
            {
                CellType_e bottom = CellFunctions.cellGrid[(int)this.position.x, (int)this.position.y - fallDistance - 1].cellType;

                if (bottom == CellType_e.TRASH)
                    destroy = true;
                if (bottom == CellType_e.ENEMY)
                    destroy = true;
                if (bottom == CellType_e.COUNTER)
                    destroy = true;
                if (bottom == CellType_e.PLAYER)
                    destroy = true;
                if (bottom == CellType_e.PRESENT)
                    destroy = true;
                    
                this.setPosition((int)this.position.x, (int)this.position.y - fallDistance);

                if(destroy)
                    Push(this.getDirection(), 1, this.cellType);
            }
        }
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        destroy = false;
        base.Setup(position, Direction_e.DOWN, generated);
    }

    public override void Rotate(int amount)
    {
        return;
    }
}