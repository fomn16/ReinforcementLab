using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public AgentMovementScript agent;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = SetY(transform.position, 10);
    }

    Vector3 SetY(Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    Vector3 CopyXYWithSpacing(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v2.x - 5, v1.y, v2.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = CopyXYWithSpacing(transform.position, agent.transform.position);
    }
}
