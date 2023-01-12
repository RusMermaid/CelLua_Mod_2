using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : Cell
{
    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e pusher = CellType_e.DUMMY)
    {
        return (false, false);
    }

    public override void Rotate(int amount)
    {
        return;
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        GetComponent<SpriteRenderer>().color = FindObjectOfType<Camera>().backgroundColor;
        base.Setup(position, rotation, generated);
    }
}
