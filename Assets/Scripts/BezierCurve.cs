using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{   
    LineRenderer lineRenderer;
    public int numPoints = 200;
    public Vector3[] positions = new Vector3[200];
    // Start is called before the first frame update
    void Awake()
    {   
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        
    }

    public void drawLine(Vector3 p0, Vector3 p1)
    {   
        for (int i = 1; i < numPoints + 1; i++) {
            float t = i / (float)numPoints;
            positions[i-1] = calculateBezierPoint_(p0, p1, t);
            // Debug.Log(positions[i-1]);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 calculateBezierPoint_(Vector3 p0, Vector3 p1, float t)
    {
        return p0 + t * (p1 - p0);
    }

    public void drawCurve(Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1)
    {   
        for (int i = 1; i < numPoints + 1; i++) {
            float t = i / (float)numPoints;
            positions[i-1] = calculateBezierPoint(p0, c0, c1, p1, t);
            // Debug.Log(positions[i-1]);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 calculateBezierPoint(Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1, float t)
    {
        Vector3 p0c0 = Vector3.LerpUnclamped(p0, c0, t);
        Vector3 c0c1 = Vector3.LerpUnclamped(c0, c1, t);
        Vector3 c1p1 = Vector3.LerpUnclamped(c1, p1, t);
    
        Vector3 x = Vector3.LerpUnclamped(p0c0, c0c1, t);
        Vector3 y = Vector3.LerpUnclamped(c0c1, c1p1, t);
    
        return Vector3.LerpUnclamped(x, y, t);
    }
}
