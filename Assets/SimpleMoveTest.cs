﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveTest : MonoBehaviour
{
    public Vector3 velocity=Vector3.forward;
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }
}
