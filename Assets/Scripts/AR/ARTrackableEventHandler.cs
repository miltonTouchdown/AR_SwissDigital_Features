﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ARTrackableEventHandler : DefaultTrackableEventHandler
{
    private bool m_IsPlaced = false;
    [SerializeField]
    private ShaderBagControl m_shaderControl;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        if (mTrackableBehaviour)
        {
            GetComponentInChildren<LookAt>().LookAtOnce();

            m_IsPlaced = true;

            ARManager.Instance.OnTrackingFound();

            m_shaderControl.ShowObject();
        }
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        if (mTrackableBehaviour)
        {
            m_IsPlaced = false;

            ARManager.Instance.OnTrackingLost();

            m_shaderControl.HideObject();
        }
    }

}
