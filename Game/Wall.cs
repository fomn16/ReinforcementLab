using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int wallYDisplacement = 2;
    public bool destroySelf = false;
    private float deleteHeight;
    private float moveSpeed = -2f;

    // Start is called before the first frame update
    void Start()
    {
        deleteHeight = -wallYDisplacement;
        enabled = false;
    }

    public void BeginSelfdestruction()
    {
        destroySelf = true;
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroySelf)
        {
            transform.Translate(0, moveSpeed * Time.deltaTime, 0);
            if (transform.position.y < deleteHeight)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
