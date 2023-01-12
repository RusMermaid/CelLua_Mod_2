using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TrackedCell
{
    public ParticleSystem deathParticals;

    public Sprite pig;

    void Update()
    {
        base.Update();
        if (GameObject.Find("PlayerEE(Clone)") != null) GetComponent<SpriteRenderer>().sprite = pig;
    }

    public override void Step()
    {
        KeyCode[] moveDirs = FindObjectOfType<GridManager>().moveDirs;
        for (int i = 0; i < moveDirs.Length; i++) if (Input.GetKey(moveDirs[i]))
        {
            this.Move(this.getDirection(), 1);
            this.suppresed = false;
            break;
        }
    }

    public (bool, bool) Move(Direction_e dir, int bias)
    {
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

    public override (bool, bool) Push(Direction_e dir, int bias, CellType_e pusher = CellType_e.DUMMY)
    {
        if (bias < 1)
            return (false, false);

        if (this.generated)
            this.Delete(true);
        else this.Delete(false);

        AudioManager.i.PlaySound(GameAssets.i.destroy);
        Instantiate(deathParticals, this.gameObject.transform.position, Quaternion.identity);

        return (true, true);
    }

    public override void Rotate(int amount)
    {
        if (suppresed)
        {
            base.Rotate(amount);
            return;
        }
        return;
    }
}
