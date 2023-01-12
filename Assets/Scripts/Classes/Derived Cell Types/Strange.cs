using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strange : TrackedCell
{
    int[][] rotationOffsets = new int[][] { 
        new int[] {0, 0}, 
        new int[] {1, 0}, 
        new int[] { 0, -1}, 
        new int[] { -1, 0}, 
        new int[] { 0, 1} };

    public override void Step()
    {
        int rng = Random.Range(0, 3);

        if (rng == 0)
        {
            int dir = Random.Range(0, 4);
            Push((Direction_e)(Random.Range(0,4)), 1);
        }
        else if (rng == 1)
        {
            foreach (int[] offset in rotationOffsets)
            {
                rotateCell(offset[0], offset[1]);
            }
        }
    }

    void rotateCell(int xOffset, int yOffset)
    {
        if (this.position.x + xOffset >= CellFunctions.gridWidth || this.position.y + yOffset >= CellFunctions.gridHeight)
            return;
        if (this.position.x + xOffset < 0 || this.position.y + yOffset < 0)
            return;
        if (CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset] == null)
            return;

        CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset].Rotate(Random.Range(0,3));
    }
}