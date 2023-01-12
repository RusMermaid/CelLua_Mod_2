using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : TrackedCell
{
    public bool isActive() {
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
        //Array index error prevention
        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0)
            return false;
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight)
            return false;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
            return false;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
            return false;
        //If we don't have a refrence cell return
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] == null)
            return false;
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType == CellType_e.VOID)
            return false;
        return true;
    }

    public override void Step()
    {
        //Subract to find refrence, add to find target
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
        //Array index error prevention
        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0)
            return;     
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight)
            return;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
            return;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
            return;

        //If we don't have a refrence cell return
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] == null)
            return;
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType == CellType_e.VOID)
            return;
        //If there is a cell in our way push it :3
        if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY] != null)
        {
            if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY].cellType == CellType_e.COUNTER && CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType == CellType_e.COUNTER)
            {
                CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY].gameObject.GetComponent<Counter>().counter++;
                CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].gameObject.GetComponent<Counter>().counter--;
                AudioManager.i.PlaySound(GameAssets.i.place);
                return;
            }

            (bool, bool) pushResult = CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY].Push(this.getDirection(), 1, CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType);
            if (pushResult.Item2 || !pushResult.Item1)
                return;
        }

        AudioManager.i.PlaySound(GameAssets.i.place);
        Cell refrenceCell = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY];
        Cell newCell = GridManager.instance.SpawnCell(
            refrenceCell.cellType,
            new Vector2((int)this.position.x + offsetX, (int)this.position.y + offsetY),
            refrenceCell.getDirection(),
            true
            );
        newCell.oldPosition = this.position;
        newCell.generated = true;

        if (refrenceCell.cellType == CellType_e.COUNTER) newCell.gameObject.GetComponent<Counter>().counter = refrenceCell.gameObject.GetComponent<Counter>().counter;
    }
}
