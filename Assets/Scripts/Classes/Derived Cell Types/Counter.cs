using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : Cell
{
    public TextMesh text;
    public int counter;

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

    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e p = CellType_e.DUMMY)
    {

        if (bias > 0)
        {
            bool denied = false;

            foreach (int[] offset in offsets)
            {
                if (Check(offset[0], offset[1]) == true)
                    denied = true;
            }
            
            if (denied) goto End;
            
            if (p != CellType_e.DUMMY)
            {
                goto End;
            }

            int offsetX = 0;
            int offsetY = 0;

            switch (dir)
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
            
            if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0 || this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight)
                p = CellType_e.PULLER;
            else if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] == null)
                p = CellType_e.PULLER;
            else p = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType;

            End:
            {
                AudioClip inst = GameAssets.i.piano;

                if (p == CellType_e.FALL) inst = GameAssets.i.vineboom;
                else if (p == CellType_e.BLOCK || p == CellType_e.SLIDE || p == CellType_e.DIRECTIONAL) inst = GameAssets.i.square;
                else if (p == CellType_e.INPUTMOVER || p == CellType_e.INPUTGENERATOR || p == CellType_e.INPUTENEMY || p == CellType_e.DENIER) inst = GameAssets.i.snare;
                else if (p == CellType_e.CONVERTER) inst = GameAssets.i.trumpet;
                
                if (!denied) AudioManager.i.PlaySound(inst, 1 * Mathf.Pow(1.05946f, counter), true);
                counter += 1;
                return (true, true);
            }
        }
        else return (false, false);
    }

    void Update()
    {
        base.Update();
        text.text = counter.ToString();
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        counter = 0;
        text.text = "0";
        base.Setup(position, rotation, generated);
    }
}
