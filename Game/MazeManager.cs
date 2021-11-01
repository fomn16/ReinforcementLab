using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MazeManager : MonoBehaviour
{
    [SerializeField]
    private int cellSize = 1;

    [SerializeField]
    private int updatePeriod = 60;

    public GameObject cellPrefab;
    public Cell[,] cells;
    public MazeGenerator generator;
    public AgentMovementScript agent;
    public GameObject flag;
    public GameObject flagPrefab;

    public int[] initPos;
    public int[] goalPos;

    public int mazeSize;

    public void ResetMazeManager(int newMazeSize){
        foreach (Cell cell in cells){
            DestroyImmediate(cell.gameObject);
        }
        DestroyImmediate(flag);
        InitMazeManager(newMazeSize);
    }

    // TODO Implementar outro maze generator
    // Start is called before the first frame update
    public void InitMazeManager(int mazeSize)
    {
        this.mazeSize = mazeSize;
        cells = new Cell[mazeSize, mazeSize];

        for (int i = 0; i < mazeSize; i++)
        {
            for (int j = 0; j < mazeSize; j++)
            {
                cells[i, j] = Instantiate(cellPrefab, transform.position, transform.rotation).GetComponent<Cell>();
                cells[i, j].transform.Translate(cellSize * (i - mazeSize / 2), 0, cellSize * (j - mazeSize / 2));
                cells[i, j].transform.parent = this.transform;
                cells[i, j].updateRightWalls = j == 0;
                cells[i, j].updateDownWalls = i == 0;
                cells[i, j].cellSize = cellSize;
            }
        }

        initPos = new int[2]{(int)Random.Range(0, mazeSize), (int)Random.Range(0, mazeSize)};
        agent.moveDist = cellSize;
        agent.resetLocation();

        goalPos = new int[2] { (int)Random.Range(0, mazeSize), (int)Random.Range(0, mazeSize) };
        flag = Instantiate(flagPrefab, transform.position, transform.rotation);
        flag.transform.Translate(cellSize * (goalPos[0] - mazeSize / 2) + 0.75f, 0 , cellSize * (goalPos[1] - mazeSize / 2) + 0.75f);

        generator = gameObject.AddComponent(typeof(MazeGenerator)) as MazeGenerator;
        generator.updatePeriod = updatePeriod;
        generator.parent = this;
    }

    public void DeleteWalls(int[] a, int[] b)
    {
        int xOff = a[0] - b[0];
        int zOff = a[1] - b[1];
        int[] directions;

        //populates directions with either horizontal or vertical directions
        if (xOff != 0)
        {
            directions = new int[2] { ~Cell.directionIndexDict["Direction"]["UP"], ~Cell.directionIndexDict["Direction"]["DOWN"] };
        }
        else
        {
            directions = new int[2] { ~Cell.directionIndexDict["Direction"]["LEFT"], ~Cell.directionIndexDict["Direction"]["RIGHT"] };
        }

        // switches indexes 0 and 1 depending on direction

        if(xOff + zOff > 0)
        {
            int buffer = directions[0];
            directions[0] = directions[1];
            directions[1] = buffer;
        }

        // applies calculated directions

        cells[a[0], a[1]].wallState &= directions[0];
        cells[b[0], b[1]].wallState &= directions[1];
        cells[a[0], a[1]].UpdateWalls();
        cells[b[0], b[1]].UpdateWalls();
    }
}
