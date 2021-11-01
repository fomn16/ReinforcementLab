using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagRotation : MonoBehaviour
{
    float spd;
    float ms = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        spd = 0;
    }

    // Update is called once per frame
    void Update()
    {
        spd += (float)Random.Range(-0.01f, 0.01f);
        if (spd > ms) spd = ms;
        if (spd < -ms) spd = -ms;
        transform.Rotate(0, spd, 0);
    }
}
