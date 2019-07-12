using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ARTrackableEventHandler : DefaultTrackableEventHandler
{

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        if (mTrackableBehaviour)
        {
            GetComponentInChildren<LookAt>().LookAtOnce();
        }
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        if (mTrackableBehaviour)
        {

        }
    }

    public void test(HitTestResult d)
    {
        //setTextCanvas(d);
        Debug.Log("Hit Result: " + d.HitTestPtr);
        return;
        if (mTrackableBehaviour)
        {
            switch (m_NewStatus)
            {
                case TrackableBehaviour.Status.NO_POSE:
                    {
                        Debug.Log("Status info NO_POSE: " + mTrackableBehaviour.CurrentStatusInfo);
                        //if (mTrackableBehaviour.CurrentStatusInfo == TrackableBehaviour.StatusInfo.UNKNOWN)
                        //{
                        //    Debug.Log("Status info: ")
                        //}
                        break;
                    }
                case TrackableBehaviour.Status.TRACKED:
                    {
                        Debug.Log("Status info TRACKED: " + mTrackableBehaviour.CurrentStatusInfo);
                        break;
                    }
            }
        }
    }

    void OnDevicePoseStatusChanged(Vuforia.TrackableBehaviour.Status status, Vuforia.TrackableBehaviour.StatusInfo statusInfo)

    {

        Debug.Log("OnDevicePoseStatusChanged(" + status + ", " + statusInfo + ")");

        switch (statusInfo)

        {

            case Vuforia.TrackableBehaviour.StatusInfo.INITIALIZING:

                Debug.Log("Tracker Initializing");

                break;

            case Vuforia.TrackableBehaviour.StatusInfo.EXCESSIVE_MOTION:

                Debug.Log("Excessive Motion");

                break;

            case Vuforia.TrackableBehaviour.StatusInfo.INSUFFICIENT_FEATURES:

                Debug.Log("Insufficient Features");

                break;

            default:

                Debug.Log("");

                break;

        }

    }
}
