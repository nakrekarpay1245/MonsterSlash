using System.Collections.Generic;
using UnityEngine;

public class LineBetweenTiles : MonoSingleton<LineBetweenTiles>
{
    private List<Vector3> points;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        points = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Adds the specified point to the line and updates the line renderer.
    /// </summary>
    /// <param name="point"></param>
    public void AddPointToLine(Vector3 point)
    {
        points.Add(point);
        DrawLine();
    }

    /// <summary>
    /// Removes the specified point from the line and updates the line renderer.
    /// </summary>
    /// <param name="point"></param>
    public void RemovePointFromLine(Vector3 point)
    {
        points.Remove(point);
        DrawLine();
    }

    /// <summary>
    /// Draws the line by setting the position count of the line renderer and updating its 
    /// positions with the current points.
    /// </summary>
    private void DrawLine()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    /// <summary>
    /// Clears the line by removing all points and resetting the position count of the line renderer.
    /// </summary>
    public void ClearLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }
}
