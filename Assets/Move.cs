﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public RectTransform tf;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GridManager.playSimulation)
        {
            tf.position += new Vector3(0, speed, 0);
        }
    }
}
