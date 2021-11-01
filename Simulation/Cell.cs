using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static string[] directions = new string[] { "UP", "DOWN", "LEFT", "RIGHT" };

    static float[][] translations;

    public static Dictionary<string, Dictionary<string, int>> directionIndexDict = new Dictionary<string, Dictionary<string, int>>() {
        { "Direction", new Dictionary<string, int> {
            { "UP",     1 },
            { "DOWN",   2 },
            { "LEFT",   4 },
            { "RIGHT",  8 },
        }},

        { "Index", new Dictionary<string, int> {
            { "UP",     0 },
            { "DOWN",   1 },
            { "LEFT",   2 },
            { "RIGHT",  3 },
        }},
    };

    public int wallState = 15;  // Direction.LEFT | Direction.RIGHT | Direction.UP | Direction.RIGHT
   
    public GameObject wallPrefab;
    public GameObject[] wallInstances;

    public bool updateRightWalls = false;
    public bool updateDownWalls = false;
    public bool visited = false;

    public int cellSize = 1;

    public float reward = -1;

    // Start is called before the first frame update
    void Start()
    {   
        wallInstances = new GameObject[4];
        wallState = 15;
        reward = -1;
        translations = new float[][] { new float[] { cellSize / 2f , 0, 0}, new float[] { -cellSize / 2f, 0, 0}, new float[] { 0, cellSize / 2f, 90}, new float[] { 0, -cellSize / 2f, 90} };
        UpdateWalls();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateWalls()
    {
        int thisDir, thisInd, i;

        List<int> directionsToCheck = new List<int> { directionIndexDict["Index"]["UP"], directionIndexDict["Index"]["LEFT"] };
        if (updateDownWalls) directionsToCheck.Add(directionIndexDict["Index"]["DOWN"]);
        if (updateRightWalls) directionsToCheck.Add(directionIndexDict["Index"]["RIGHT"]);

        for (int a = 0; a < directionsToCheck.Count; a++)
        {
            i = directionsToCheck[a];
            thisDir = directionIndexDict["Direction"][directions[i]];
            thisInd = directionIndexDict["Index"][directions[i]];

            if (((wallState & thisDir) == 0) && (wallInstances[thisInd] != null) && (!wallInstances[thisInd].GetComponent<Wall>().destroySelf))
            {
                wallInstances[thisInd].GetComponent<Wall>().BeginSelfdestruction();
            }

            if (((wallState & thisDir) != 0) && (wallInstances[thisInd] == null))
            {
                wallInstances[thisInd] = Instantiate(wallPrefab, transform.position, transform.rotation);
                wallInstances[thisInd].transform.Translate(translations[i][0], wallInstances[thisInd].GetComponent<Wall>().wallYDisplacement, translations[i][1]);
                wallInstances[thisInd].transform.Rotate(0, translations[i][2], 0);
                wallInstances[thisInd].transform.parent = transform;
            }
        }
    }
}
