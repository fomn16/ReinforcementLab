using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUDArrowScript : MonoBehaviour
{
    public bool showArrows;
    public double[] values;

    void Start()
    {
        this.values = new double[4] { 0, 0, 0, 0 };
    }
}