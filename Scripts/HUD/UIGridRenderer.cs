using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[ExecuteInEditMode]
public class UIGridRenderer : Graphic
{
    public float outerEdgeWidth = 10f;
    public float gridWidth = 10f;
    public float lineWidth = 10f;

    public Color lineColor;

    public Vector2Int gridSize = new Vector2Int(1, 1);
    private int curVertex = 0;

    private VertexHelper vh;

    float w;
    float h;

    public Vector2[] points = { };
    public Vector2[] normPoints = { };
    public Vector2 maxValues = new Vector2();
    public Vector2 minValues = new Vector2();

    public float[] lastState = { 0, 0, 0, 0, 0 };

    public List<GameObject> texts = new List<GameObject>();

    public Text graphName;
    public string graphNameStr = "graph";

    public Font font;

    protected override void Start()
    {
        base.Start();
        lineColor = Color.black;//Random.ColorHSV();
        this.graphName.text = this.graphNameStr;
        font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        resetScale();
    }

    void Update()
    {
        this.graphName.text = this.graphNameStr;
        float[] curState = { points.Length, maxValues.x, maxValues.y, minValues.x, minValues.y };

        if (areNotEqual(curState, lastState))
        {
            curState.CopyTo(lastState, 0);
            resetScale();
        }
    }

    bool areNotEqual(float[] x, float[] y)
    {
        bool result = false;
        for (int i = 0; i < x.Length; i++)
        {
            result |= x[i] != y[i];
        }
        return result;
    }

    private void resetScale()
    {
        foreach (GameObject text in texts){
            DestroyImmediate(text);
        }

        texts.Clear();

        float wStep = w / (float)gridSize.x;
        float hStep = h / (float)gridSize.y;

        for (int i = 1; i < gridSize.x; i++)
        {
            string text = (((maxValues.x - minValues.x) / gridSize.x) * i + minValues.x).ToString("0.##");
            if(text != "NaN")
            {
                float xPos = (i) * wStep - gridWidth / 2;

                GameObject curGO = new GameObject();
                curGO.transform.SetParent(this.transform);

                Text curText = curGO.gameObject.AddComponent<Text>();

                curText.text = text;
                curText.transform.position = transform.position;
                curText.transform.Translate(new Vector3(xPos*transform.localScale.x, -10*transform.localScale.y));
                curText.alignment = TextAnchor.MiddleCenter;
                curText.font = font;
                curText.fontSize = 12;
                curText.color = Color.black;
                texts.Add(curGO);
            }
        }

        for (int j = 0; j < gridSize.y+1; j++)
        {
            string text = (((maxValues.y - minValues.y) / gridSize.y) * j + minValues.y).ToString("0.##");
            if (text != "NaN")
            {
                float yPos = j * hStep - gridWidth / 2;

                GameObject curGO = new GameObject();
                curGO.transform.SetParent(this.transform);

                Text curText = curGO.gameObject.AddComponent<Text>();

                curText.text = text;
                curText.transform.position = transform.position;
                curText.transform.Translate(new Vector3(-1.7f* wStep * transform.localScale.x, yPos * transform.localScale.y));
                curText.alignment = TextAnchor.MiddleRight;
                curText.font = font;
                curText.fontSize = 12;
                curText.color = Color.black;
                texts.Add(curGO);
            }
        }
    }


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        this.vh = vh;
        this.w = rectTransform.rect.width;
        this.h = rectTransform.rect.height;

        vh.Clear();
        curVertex = 0;

        NormalizePoints();

        DrawGrid();

        DrawPoints();

        DrawOuterEdge();
    }

    private void NormalizePoints()
    {
        maxValues.x = float.NegativeInfinity;
        maxValues.y = float.NegativeInfinity;

        minValues.x = float.PositiveInfinity;
        minValues.y = float.PositiveInfinity;

        for (int i = 0; i < points.Length; i++)
        {
            if(points[i].x > maxValues.x)
                maxValues.x = points[i].x;

            if (points[i].y > maxValues.y)
                maxValues.y = points[i].y;

            if (points[i].x < minValues.x)
                minValues.x = points[i].x;

            if (points[i].y < minValues.y)
                minValues.y = points[i].y;
        }

        normPoints = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            normPoints[i] = new Vector2((points[i].x - minValues.x) * w / (maxValues.x - minValues.x), (points[i].y - minValues.y) * w / (maxValues.y - minValues.y) + 1);
        }
    }

    private void DrawPoints()
    {
        for (int i = 1; i < normPoints.Length; i++){
            Vector2 curPoint = normPoints[i-1];
            Vector2 nextPoint = normPoints[i];
            
            float[,] corners1 = { { curPoint.x, curPoint.y - lineWidth }, { curPoint.x, curPoint.y + lineWidth }, { nextPoint.x, nextPoint.y + lineWidth }, { nextPoint.x, nextPoint.y - lineWidth } };
            Quad(corners1, lineColor);

            float[,] corners2 = { { curPoint.x - lineWidth, curPoint.y }, { curPoint.x + lineWidth, curPoint.y }, { nextPoint.x + lineWidth, nextPoint.y }, { nextPoint.x - lineWidth, nextPoint.y } };
            Quad(corners2, lineColor);
        }
    }

    private void DrawGrid()
    {
        float wStep = w / (float) gridSize.x;
        float hStep = h / (float) gridSize.y;

        for (int i = 1; i < gridSize.x; i++)
        {
            float xPos = i * wStep - gridWidth / 2;

            Rect(xPos, 0, gridWidth, h);

            /*GameObject curGO = new GameObject();
            curGO.transform.SetParent(this.transform);

            Text curText = curGO.gameObject.AddComponent<Text>();

            curText.text = (((maxValues.x - minValues.x) / gridSize.x) * i + minValues.x).ToString("0.####");
            curText.transform.position = transform.position;
            //curText.transform.Translate(new Vector3(xPos,60));
            texts.Add(curGO);*/
        }

        for (int j = 1; j < gridSize.y; j++)
        {
            float yPos = j * hStep - gridWidth / 2;
            Rect(0, yPos, w, gridWidth);

            /*GameObject curGO = new GameObject();
            curGO.transform.SetParent(this.transform);

            Text curText = curGO.gameObject.AddComponent<Text>();

            curText.text = (((maxValues.y - minValues.y) / gridSize.y) * j + minValues.y).ToString("0.####");
            curText.transform.position = transform.position;
            //curText.transform.Translate(new Vector3(0, yPos+60));
            texts.Add(curGO);*/
        }
    }

    private void DrawOuterEdge()
    {
        Rect(0, 0, w, outerEdgeWidth);

        Rect(0, 0, outerEdgeWidth, h);
    }

    private void Rect(float x, float y, float w, float h)
    {
        float[,] points0 = { { x, y }, { x, y + h }, { x + w, y + h }, { x + w, y } };
        Quad(points0, color);
    }

    private void Quad(float[,] points, Color color)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        for (int i = 0; i<4; i++)
        {
            vertex.position = new Vector3(points[i,0], points[i,1]);
            vh.AddVert(vertex);
        }

        vh.AddTriangle(curVertex, curVertex+1, curVertex+2);
        vh.AddTriangle(curVertex+2, curVertex+3, curVertex);
        curVertex += 4;
    }
}
