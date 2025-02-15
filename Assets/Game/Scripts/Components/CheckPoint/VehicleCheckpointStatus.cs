using System.Collections.Generic;
using UnityEngine;

public class VehicleCheckpointStatus
{
    public int CurrentIndex;
    public Transform Target;
    public Transform Self;
    public readonly List<int> checkPointIndexList = new List<int>();
    public readonly List<int> aiPointIndexList = new List<int>();

    public int lapsCompleted;
    public int CompleteCheckPointCount;
    public float progress => getProgress();
    public int rank=100;
    public void AddPoint(int index, int pointIndex = -1)
    {
        checkPointIndexList.Add(index);
        if (pointIndex >= 0)
        {
            aiPointIndexList.Add(pointIndex);
        }
        CompleteCheckPointCount = checkPointIndexList.Count;
    }
    public void AddLaps()
    {
        lapsCompleted++;
        checkPointIndexList.Clear();
    }
    private float getProgress()
    {
        var result = lapsCompleted * 100000f + checkPointIndexList.Count * 1000f;
        if (Self != null && Target != null)
        {
            result += 100f-Vector3.Distance(Self.position, Target.position);
        }
        return result;
    }
}
