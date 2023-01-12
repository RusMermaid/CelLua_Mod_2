using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGenerator : TrackedCell
{
    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, rotation, generated);
    }

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

        //If we don't have a refrence cell return
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] == null)
            return;
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType == CellType_e.VOID)
            return;

        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
        {   
            PhysicalGenerate(offsetX, offsetY);    
            return;
        }
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
        {
            PhysicalGenerate(offsetX, offsetY);    
            return;
        }

        //If there is a cell in our way push it :3
        if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY] != null)
        {
            //if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY].cellType == CellType_e.TRASH)
            //    return;

            (bool, bool) pushResult = CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY].Push(this.getDirection(), 1);
            if (pushResult.Item2)
                return;
            if (!pushResult.Item1)
            {
                PhysicalGenerate(offsetX, offsetY);    
                return;
            }
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
    }

    void PhysicalGenerate(int offsetX, int offsetY)
    {
        Direction_e dir = this.getDirection();
        
        switch (this.getDirection())
        {
            case (Direction_e.RIGHT):
                dir = Direction_e.LEFT;
                break;
            case (Direction_e.LEFT):
                dir = Direction_e.RIGHT;
                break;
            case (Direction_e.UP):
                dir = Direction_e.DOWN;
                break;
            case (Direction_e.DOWN):
                dir = Direction_e.UP;
                break;
        }

        Cell referenceCell = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY];

        (bool, bool) pushResult = Push(dir, 1);
        if (pushResult.Item2 || !pushResult.Item1)
            return;
		try {
			if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY] != null)
				return;
		} catch (System.IndexOutOfRangeException) {
			return;
		}

        AudioManager.i.PlaySound(GameAssets.i.place);
        Cell newCell1 = GridManager.instance.SpawnCell(
            referenceCell.cellType,
            new Vector2((int)this.position.x + offsetX, (int)this.position.y + offsetY),
            referenceCell.getDirection(),
            true
            );
        newCell1.oldPosition = this.position;
        newCell1.generated = true;
    }
}
