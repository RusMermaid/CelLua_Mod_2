using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConverter : TrackedCell
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

        CellType_e convertTo = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType;
        Cell convert = CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY];

        if (convertTo == convert.cellType && convertTo != CellType_e.COUNTER)
            return;

        List<Cell> allCells = new List<Cell>();

        for (int x = 0; x < CellFunctions.gridWidth; x++)
        {
            for (int y = 0; y < CellFunctions.gridHeight; y++)
            {
                if (CellFunctions.cellGrid[x, y] != null)
                {
                    allCells.Add(CellFunctions.cellGrid[x, y]);
                }
            }
        }

        foreach (Cell cell in allCells)
        {
            if(cell.cellType == convert.cellType && cell.position != convert.position && cell.position != new Vector2Int((int)this.position.x - offsetX, (int)this.position.y - offsetY))
            {
                AudioManager.i.PlaySound(GameAssets.i.place);
                cell.Delete(false);
                Cell newCell = GridManager.instance.SpawnCell(convertTo, cell.position, cell.getDirection(), true);

                if (convertTo == CellType_e.COUNTER)
                {
                    newCell.gameObject.GetComponent<Counter>().counter = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].gameObject.GetComponent<Counter>().counter;
                }
            }
        }
    }
}
