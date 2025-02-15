using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    public static RankManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        var statusDict = CheckPointManager.Instance.objStatus;
        if (statusDict == null)
            return;


        var sortedList = statusDict.Values.Where(status => status != null).OrderByDescending(status => status.progress).ToList();

        for (int i = 0; i < sortedList.Count; i++)
        {
            var st = sortedList[i];
            st.rank = i + 1;
        }
    }
}
