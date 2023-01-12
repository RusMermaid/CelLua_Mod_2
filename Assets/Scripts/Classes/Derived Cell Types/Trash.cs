using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : Cell
{
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

    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e pusher = CellType_e.DUMMY)
    {
        if (bias > 0)
        {
            bool denied = false;

            foreach (int[] offset in offsets)
            {
                if (Check(offset[0], offset[1]) == true)
                    denied = true;
            }

            if (!denied) AudioManager.i.PlaySound(GameAssets.i.destroy);
            return (true, true);
        }
        else return (false, false);
    }
}
