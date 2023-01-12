using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEnemy : TickedCell
{   

    public ParticleSystem deathParticals;

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
        GridManager.enemyCount++;
        base.Setup(position, rotation, generated);
    }

    public override void Delete(bool destroy)
    {
        GridManager.enemyCount--;
        base.Delete(destroy);
    }

    public void OnMouseDown()
    {
        bool denied = false;

        foreach (int[] offset in offsets)
        {
            if (Check(offset[0], offset[1]) == true)
                denied = true;
        }
        
        if (GridManager.playSimulation == true && denied == false)
        {
            this.Die(this.generated);
        }
    }

    public void Die(bool destroy)
    {
        AudioManager.i.PlaySound(GameAssets.i.destroy);
        Instantiate(deathParticals, this.gameObject.transform.position, Quaternion.identity);

        Delete(destroy);
    }
}
