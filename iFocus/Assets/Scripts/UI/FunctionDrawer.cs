using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionDrawer : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    
    private GameObject currentLine;
    
    [SerializeField] private GameObject curveHighlightPointPrefab;

    private LineRenderer lineRenderer;

    [SerializeField] private AdvanceSettings advanceSettings;

    private Coroutine drawCoroutine;

    [SerializeField] private Vector2 step = new Vector2(1,1);
    [SerializeField] private Vector2 offsetTweak = new Vector2(0, 0);

    [SerializeField] private Transform offsetWithTransform;

    private Vector2[] pointsToDraw;

    private List<CurveHighlightPoint> curveHighlightPoints = new List<CurveHighlightPoint>();

    private void Awake()
    {
        CreateLine();
        SetLineColor(advanceSettings.lineColor);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Spawns a line to use for drawing a function curve.
    /// </summary>
    public void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLine.transform.SetParent(this.transform);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    public void DrawFunction()
    {
        if (pointsToDraw == null && pointsToDraw.Length <= 0)
            return;

        if (drawCoroutine != null)
            StopCoroutine(drawCoroutine);
        
        drawCoroutine = StartCoroutine(DrawingFunction());
    }

    public void DrawFunctionInmediate()
    {
        if (pointsToDraw == null && pointsToDraw.Length <= 0)
            return;

        lineRenderer.positionCount = 0;

        for (int i = 0; i < pointsToDraw.Length; i++)
        {
            lineRenderer.positionCount++;

            // Hay que revisar esta conversion de 3D a 2D.
            //lineRenderer.SetPosition(i, points[i]);

            lineRenderer.SetPosition(i, new Vector3((pointsToDraw[i].x * step.x) + (offsetWithTransform.position.x + offsetTweak.x), (pointsToDraw[i].y * step.y) + (offsetWithTransform.position.y + offsetTweak.y)));
        }
    }

    IEnumerator DrawingFunction()
    {
        lineRenderer.positionCount = 0;

        for (int i = 0; i < pointsToDraw.Length; i++)
        {
            lineRenderer.positionCount++;

            // Hay que revisar esta conversion de 3D a 2D.
            //lineRenderer.SetPosition(i, points[i]);

            lineRenderer.SetPosition(i, new Vector3((pointsToDraw[i].x * step.x) + (offsetWithTransform.position.x + offsetTweak.x), (pointsToDraw[i].y * step.y) + (offsetWithTransform.position.y + offsetTweak.y)));
            //Debug.Log("OffsetX: " + offsetWithTransform.position.x);
            //Debug.Log("OffsetY: " + offsetWithTransform.position.y);

            yield return new WaitForSeconds(advanceSettings.deltaTimeToDraw);
        }
    }

    public void SetLineColor(Color newColor)
    {
        lineRenderer.material.color = newColor;
    }

    public void StorePoints(Vector2[] pointsToStore)
    {
        pointsToDraw = new Vector2[pointsToStore.Length];
        pointsToDraw = pointsToStore;
    }

    [SerializeField]
    private int testIndex;
    
    [ContextMenu("TestCreatePoint")]
    private void TestCreatePoint()
    {
        CreatePointOfInterest(testIndex);
    }

    /// <summary>
    /// This method will create a point of interest in the curve on the desired index. The values of the points are going to be the same as the values on the curve point with the designed index.
    /// </summary>
    /// <param name="index"></param>
    public void CreatePointOfInterest(int index)
    {
        CurveHighlightPoint auxPoint = Instantiate(curveHighlightPointPrefab, this.transform).GetComponent<CurveHighlightPoint>();
        auxPoint.Index = curveHighlightPoints.Count - 1;
        auxPoint.SetPosition(pointsToDraw[index]);
    }

    private void OnValidate()
    {
        if (!linePrefab)
        {
            Debug.LogWarning("Please assign a prefab with a line renderer to the linePrefab field on the inspector.");
        }

        if (pointsToDraw != null && pointsToDraw.Length > 0)
            DrawFunctionInmediate();

    }

    [System.Serializable]
    private class AdvanceSettings
    {
        /// <summary>
        /// The distance int "time" on wich each new point is going to be drawn.
        /// </summary>
        public float deltaTimeToDraw;
        
        /// <summary>
        /// The color of thw drawn line.
        /// </summary>
        public Color lineColor;
    }

}
