using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : TrackedCell
{
    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, rotation, generated);
    }

    public override void Step()
    {
        int offsetX = 0;
        int offsetY = 0;

        switch (this.getDirection())
        {
            case (Direction_e.RIGHT):
                offsetX += 1;
                break;
            case (Direction_e.DOWN):
                offsetY += -1;
                break;
            case (Direction_e.LEFT):
                offsetX += -1;
                break;
            case (Direction_e.UP):
                offsetY += 1;
                break;
        }

        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
            return;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
            return;

        if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY] != null)
        {
            return;
        }

        this.Push(this.getDirection(), 0);
        //Suppressed will get set to true so we have to reset it.
        this.suppresed = false;
    }

    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e pusher = CellType_e.DUMMY)
    {
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
