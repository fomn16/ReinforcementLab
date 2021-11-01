using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementScript : MonoBehaviour
{
    public float moveDist;
    private Vector2 dstPos;
    public MazeManager mazeManager;

    public int[] mazePos;       

    public float InverseMoveSpeed;

    public bool isMoving;


    // Start is called before the first frame update
    void Start()
    {
        dstPos = ToVector2(transform.position);
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dst = (ToVector2(transform.position) - dstPos);
        if (dst.magnitude > 0.1)
        {
            transform.position -= ToVector3(dst.normalized / InverseMoveSpeed) * Time.deltaTime;
            transform.position = SetY(transform.position, 1 + Mathf.Sin((dst.magnitude / moveDist) * Mathf.PI));
        }
        else
        {
            isMoving = false;
            GetInputFromPlayer();
        }
    }

    Vector2 ToVector2(Vector3 v)
    {
        return new Vector3(v.x, v.z);
    }

    Vector3 ToVector3(Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    Vector3 SetY(Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }


    void GetInputFromPlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move("UP");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move("DOWN");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move("LEFT");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move("RIGHT");
        }
    }


    public void Move(string direction)
    {
        if(!isMoving) {
            if (direction == "Reset")
            {
                resetLocation();
                return;
            }
            int wallState = mazeManager.cells[mazePos[0], mazePos[1]].wallState;
            int dir = Cell.directionIndexDict["Direction"][direction];

            if ((wallState & dir) == 0)
            {
                switch (direction)
                {
                    case "UP":
                        dstPos.x += moveDist;
                        mazePos[0] += 1;
                        break;
                    case "DOWN":
                        dstPos.x -= moveDist;
                        mazePos[0] -= 1;
                        break;
                    case "LEFT":
                        dstPos.y += moveDist;
                        mazePos[1] += 1;
                        break;
                    case "RIGHT":
                        dstPos.y -= moveDist;
                        mazePos[1] -= 1;
                        break;
                }
                isMoving = true;  
            }
        }
    }

    public void resetLocation()
    {
        setLocation(mazeManager.initPos);
    }

    //TODO consertar set location, não está teleportando agente
    public void setLocation(int[] pos)
    {
        transform.position = mazeManager.transform.position;
        transform.Translate(moveDist * (pos[0] - mazeManager.mazeSize / 2), 1, moveDist * (pos[1] - mazeManager.mazeSize / 2));
        mazePos = new int[2] { pos[0], pos[1] };
        dstPos = ToVector2(transform.position);
        isMoving = false;
    }
}
