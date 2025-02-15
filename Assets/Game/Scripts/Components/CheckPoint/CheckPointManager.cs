using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }

    public readonly List<CheckPoint> checkPointList = new List<CheckPoint>();

    public readonly Dictionary<GameObject, VehicleCheckpointStatus> objStatus = new Dictionary<GameObject, VehicleCheckpointStatus>();

    void Awake()
    {
        Instance = this;
        var self = transform;

        for (int i = 0; i < self.childCount; i++)
        {
            var child = self.GetChild(i);
            var cp = child.GetComponent<CheckPoint>();
            checkPointList.Add(cp);
        }

        gameObject.AddComponent<RankManager>();
        this.name = "--------CheckPointManager";
    }

    public void Check(GameObject obj, CheckPoint cp)
    {
        if (!objStatus.ContainsKey(obj))
        {
            objStatus.Add(obj, new VehicleCheckpointStatus());
        }

        var status = objStatus[obj];


        var index = checkPointList.IndexOf(cp);
        status.CurrentIndex = index;
        switch (cp.type)
        {
            case ECheckPointType.Start:
            case ECheckPointType.General:

                if (!status.checkPointIndexList.Contains(index))
                {
                    var ai = obj.transform.parent.GetComponent<VehicleAI>();
                    if (ai != null)
                    {
                        status.AddPoint(index, ai.currentSegment);
                    }
                    else
                    {
                        status.AddPoint(index);
                    }
                }

                break;

            case ECheckPointType.End:
                if (status.checkPointIndexList.Count + 1 == checkPointList.Count)
                {
                    // Debug.LogError("add laps " + status.checkPointIndexList.Count + " " + checkPointList.Count);
                    status.AddLaps();
                }

                break;
        }
        status.Self = obj.transform;
        if (index < checkPointList.Count - 2)
        {
            status.Target = checkPointList[index + 1].transform;
        }
    }

    public VehicleCheckpointStatus GetStatus(GameObject obj)
    {
        if (obj == null)
        {
            return null;
        }
        if (objStatus.TryGetValue(obj, out var status))
        {

        }
        return status;
    }

    public static GameObject AddTriggerForCheckableObject(Transform checkableObject)
    {
        var colliderForCheckpoint = new GameObject("ColliderForCheckpoint");
        colliderForCheckpoint.transform.SetParent(checkableObject);
        colliderForCheckpoint.transform.localPosition = Vector3.zero;
        colliderForCheckpoint.AddComponent<BoxCollider>().isTrigger = true;
        colliderForCheckpoint.layer = LayerMask.NameToLayer(LayerNames.CheckableObject);
        return colliderForCheckpoint;
    }
    public void ResetTarget(GameObject colliderCheckPoint)
    {
        var status = CheckPointManager.Instance.GetStatus(colliderCheckPoint);
        if (status != null)
        {
            var indexOfLastCheckout = 0;
            if (status.checkPointIndexList.Count > 0)
            {
                indexOfLastCheckout = status.checkPointIndexList.Last();
            }

            var cp = CheckPointManager.Instance.checkPointList[indexOfLastCheckout];

            var t = colliderCheckPoint.transform.parent;

            var character = t.GetComponent<ICharacter>();
            character.SetPosition(cp.transform.position);
            character.SetRotation(cp.transform.rotation);

            var ai = t.GetComponent<VehicleAI>();
            if (ai != null)
            {
                ai.currentSegment = status.aiPointIndexList.Count > 0 ? status.aiPointIndexList.Last() : 0;
            }
        }
        else
        {
            var cp = GameEntry.Context.SceneConfig.GetOriginPoint(0);
            var t = colliderCheckPoint.transform.parent;

            var character = t.GetComponent<ICharacter>();
            character.SetPosition(cp.transform.position);
            character.SetRotation(cp.transform.rotation);

            var ai = t.GetComponent<VehicleAI>();
            if (ai != null)
            {
                ai.currentSegment = status.aiPointIndexList.Count > 0 ? status.aiPointIndexList.Last() : 0;
            }
        }
    }
}
