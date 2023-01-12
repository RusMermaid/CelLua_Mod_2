using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMover : TrackedCell
{
    private bool active;

    private SpriteRenderer off;
    public SpriteRenderer on;

    int[][] offsets = new int[][] { 
        new int[] {1, 0}, 
        new int[] { 0, -1}, 
        new int[] { -1, 0}, 
        new int[] { 0, 1} };

    bool Check(int xOffset, int yOffset)
    {
        if (this.position.x + xOffset >= CellFunctions.gridWidth || this.position.y + yOffset >= CellFunctions.gridHeight)
            return false;
        if (this.position.x + xOffset < 0 || this.position.y + yOffset < 0)
            return false;
        if (CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset] == null)
            return false;
        if (CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset].cellType != CellType_e.DENIER)
            return false;

        return true;
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        if (off == null)
            off = GetComponent<SpriteRenderer>();
            
        off.enabled = true;
        on.enabled = false;
        base.Setup(position, rotation, generated);
        active = false;
    }

    public override void Step()
    {
        bool denied = false;

        foreach (int[] offset in offsets)
        {
            if (Check(offset[0], offset[1]) == true)
                denied = true;
        }

        if(denied == true && active == true)
        {
            active = false;
            off.enabled = true;
            on.enabled = false;
        }

        if(active == true && denied == false)
        {
            this.Push(this.getDirection(), 0);
            this.suppresed = false;
        }
    }

    void OnMouseDown()
    {
        bool denied = false;

        foreach (int[] offset in offsets)
        {
            if (Check(offset[0], offset[1]) == true)
                denied = true;
        }

        if (GridManager.playSimulation == true && denied == false)
        {
            if (active == true)
            {
                off.enabled = true;
                on.enabled = false;
            }
            else
            {
                on.enabled = true;
                off.enabled = false;
            }
            active = !active;
        }
    }

    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e pusher = CellType_e.DUMMY)
    {
        if(active == false)
            return base.Push(dir, bias);
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
