using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : TrackedCell
{

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

        // return if the space in front / behind it is outside the grid
        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0)
            return;     
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight)
            return;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
            return;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
            return;
        
        // return if there is no cell in front of it
        if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY] == null)
            return;
        if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY].cellType == CellType_e.VOID)
            return;

        // return if there is no cell behind it
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] == null)
            return;
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType == CellType_e.VOID)
            return;
        
        Cell refrenceCell = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY];
        Cell convertingCell = CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY];

        if (refrenceCell.cellType == convertingCell.cellType)
        {
            if (refrenceCell.cellType != CellType_e.COUNTER) return;
            else if (convertingCell.GetComponent<Counter>().counter == refrenceCell.gameObject.GetComponent<Counter>().counter) return;
        }

        AudioManager.i.PlaySound(GameAssets.i.place);
        convertingCell.Delete(false);
        Cell newCell = GridManager.instance.SpawnCell(refrenceCell.cellType, new Vector2(this.position.x + offsetX, this.position.y + offsetY), convertingCell.getDirection(), true);
        
        if (refrenceCell.cellType == CellType_e.COUNTER)
        {
            newCell.gameObject.GetComponent<Counter>().counter = refrenceCell.gameObject.GetComponent<Counter>().counter;
        }
    }
}
