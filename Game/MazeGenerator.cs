using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenerationMode
{
    HuntAndKill
}

public class MazeGenerator : MonoBehaviour
{
    public int updatePeriod = 120;
    public GenerationMode mode = GenerationMode.HuntAndKill;
    public MazeManager parent = null;

    int currentT = 0;
    int [] pos;
    int unvisitedCells = 0;
    int huntAndKillState = 0;


    // Start is called before the first frame update
    void Start()
    {
        switch(mode){
            case GenerationMode.HuntAndKill:
                StartHuntAndKill();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (parent != null)
        {
            if(currentT > updatePeriod)
            {
                switch(mode){
                    case GenerationMode.HuntAndKill:
                        do
                            UpdateHuntAndKill();
                        while (updatePeriod == 0 && unvisitedCells > 0);
                        break;
                }
                currentT = 0;
            }
            currentT++;
        }
    }

    void StartHuntAndKill()
    {
        pos = new int[2] { (int)Random.Range(0, parent.mazeSize) , (int)Random.Range(0, parent.mazeSize) };
        parent.cells[pos[0],pos[1]].visited = true;
        unvisitedCells = (int)Mathf.Pow(parent.mazeSize, 2);
        unvisitedCells--;
    }

    void UpdateHuntAndKill()
    {
        if (unvisitedCells == 0)
        {
            Destroy(this);
        }

        switch (huntAndKillState)
        {
            case 0:
                Kill();
                break;
            case 1:
                Hunt();
                break;
        }
    }

    void Kill()
    {
        List<int[]> availableNeighbors = CheckNeighbors(pos[0], pos[1])[1];

        if (availableNeighbors.Count == 0)
        {
            if(Random.Range(0,1) < 0.1)
            {
                List<int[]> occupiedNeighbors = CheckNeighbors(pos[0], pos[1])[0];
                int[] conToMake = occupiedNeighbors[(int)Random.Range(0, occupiedNeighbors.Count)];

                parent.DeleteWalls(pos, conToMake);
            }

            huntAndKillState = 1;
            return;
        }

        int[] nextPos = availableNeighbors[(int)Random.Range(0, availableNeighbors.Count)];

        parent.DeleteWalls(pos, nextPos);

        pos = nextPos;

        parent.cells[pos[0], pos[1]].visited = true;

        unvisitedCells--;
    }

    void Hunt()
    {
        for (int i = 0; i < parent.mazeSize; i++)
        {
            for (int j = 0; j < parent.mazeSize; j++)
            {
                if (!parent.cells[i, j].visited)
                {
                    List<int[]> occupiedNeighbors = CheckNeighbors(i, j)[0];

                    if (occupiedNeighbors.Count != 0)
                    {
                        int[] lastPos = occupiedNeighbors[(int)Random.Range(0, occupiedNeighbors.Count)];

                        pos[0] = i;
                        pos[1] = j;

                        parent.DeleteWalls(pos, lastPos);

                        huntAndKillState = 0;

                        parent.cells[i, j].visited = true;

                        unvisitedCells--;
                        return;
                    }
                }
            }
        }
    }

    // retunn[0] = List of visited neighbors, return[1] = list of available neighbors
    List<int[]>[] CheckNeighbors (int i, int j)
    {

        List<int[]>[] info = new List<int[]>[2] { new List<int[]>(), new List<int[]>() };

        CheckAndAddToInfo(i + 1, j, info);
        CheckAndAddToInfo(i - 1, j, info);
        CheckAndAddToInfo(i, j + 1, info);
        CheckAndAddToInfo(i, j - 1, info);

        return info;
    }

    // checks if cell has been visited or not, and adds it to info vector
    void CheckAndAddToInfo(int i, int j, List<int[]>[] info)
    {
        if (i < parent.mazeSize && i >= 0 && j < parent.mazeSize && j >= 0)
        {
            if (parent.cells[i, j].visited)
                info[0].Add(new int[2] { i, j });
            else
                info[1].Add(new int[2] { i, j });
        }
    }
}
