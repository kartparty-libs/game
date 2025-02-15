using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIPathManager : MonoBehaviour
{
    public static AIPathManager inst;

    LineRenderer[] lineRendererList;

    public int idx = 0;

    void Awake()
    {
        inst = this;

        lineRendererList = GetComponentsInChildren<LineRenderer>();

        foreach (var line in lineRendererList)
        {
            line.GetComponent<LineRenderer>().enabled = false;
        }

        idx = 0;
    }

    public LineRenderer GetRandomLine()
    {
        var randomIndex = Random.Range(0, lineRendererList.Length);
        return lineRendererList[randomIndex];
    }

    public LineRenderer GetNextLine()
    {
        LineRenderer lineRenderer = lineRendererList[idx];
        idx = (idx + 1) % lineRendererList.Length;
        return lineRenderer;
    }

    public LineRenderer GetLine(int index)
    {
        return lineRendererList[index];
    }
}