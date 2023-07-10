using System.Collections.Generic;
using UnityEngine;

public class LineBetweenTiles : MonoSingleton<LineBetweenTiles>
{
    private List<Vector3> positions;
    private List<Vector3> points;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        positions = new List<Vector3>();
        points = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void AddPointToLine(Vector3 point)
    {
        points.Add(point);
        DrawLine(points);
    }

    public void RemovePointToLine(Vector3 point)
    {
        points.Remove(point);
        DrawLine(points);
    }

    public void DrawLine(List<Vector3> points)
    {
        positions.Clear();
        positions.AddRange(points);

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    public void ClearLine()
    {
        points.Clear();
        positions.Clear();
        lineRenderer.positionCount = 0;
    }
}
