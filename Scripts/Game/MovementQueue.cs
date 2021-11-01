using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementQueue : MonoBehaviour
{
    [SerializeField]
    public Queue<string> moveQueue;

    public AgentMovementScript agentMovement;
    // Start is called before the first frame update
    void Start()
    {
        moveQueue = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveQueue.Count > 0 && !agentMovement.isMoving)
        {
            agentMovement.Move(moveQueue.Dequeue());
        }
    }

    public void AppendToQueue(string move)
    {
        moveQueue.Enqueue(move);
    } 
}
