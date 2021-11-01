using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    // Q-learning variables
    public IAgent agent;
    public Cell[,] stateSpace;
    public int[] actionSpace;

    public int nStates;
    public int nActions;

    // Management variables
    public int goalState;
    public int initState;
    public int mazeSize;

    // Integration variables
    public MovementQueue movementQueue;
    public bool animate = false;
    public bool showGUI = false;

    public void InitEnvironment(Cell[,] cells,
                                int[] goalPos,
                                int mazeSize,
                                int[] initPos) {
        this.stateSpace  = cells;
        this.actionSpace = new int[] {0, 1, 2, 3};
        this.mazeSize    = mazeSize;
        this.goalState   = goalPos[0] + mazeSize*goalPos[1];
        this.initState   = To1D(initPos);

        this.nActions = this.actionSpace.Length;
        this.nStates  = mazeSize*mazeSize;
    }

    public int UpdateState(int movementIndex) {
        var direction = Cell.directions[movementIndex];
        if (animate)
            movementQueue.AppendToQueue(direction);
        return GetNewPos(direction);
    }

    public void EndEpisode()
    {
        agent.currentState = initState;
        if (animate)
            movementQueue.AppendToQueue("Reset");
    }

    public bool CheckGoal(int state) {
        return state == goalState;
    }

    public int GetNewPos(string direction)
    {
        int[] mazePos = To2D(agent.currentState);
        int wallState = stateSpace[mazePos[0], mazePos[1]].wallState;
        int dir = Cell.directionIndexDict["Direction"][direction];

        if ((wallState & dir) == 0)
        {
            switch (direction)
            {
                case "UP":
                    mazePos[0] += 1;
                    break;
                case "DOWN":
                    mazePos[0] -= 1;
                    break;
                case "LEFT":
                    mazePos[1] += 1;
                    break;
                case "RIGHT":
                    mazePos[1] -= 1;
                    break;
            }
        }
        return To1D(mazePos);
    }

    // Helper Functions
    public int[] To2D(int pos1D)
    {
        int[] pos2D = new int[2];
        pos2D[0] = pos1D % mazeSize;
        pos2D[1] = (int)(pos1D / mazeSize);

        return pos2D;
    }

    public int To1D (int[] pos1D)
    {
        return pos1D[0] + mazeSize * pos1D[1];
    }
}
