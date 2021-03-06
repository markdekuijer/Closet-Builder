﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHead : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Awake()
    {
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            Display.displays[1].SetParams(1920, 1080, 0, 0);
        }
    }

    void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target.transform.localRotation, Time.deltaTime);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.transform.localPosition, Time.deltaTime * 2.5f);
    }
}
