using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class ARManager : MonoBehaviour
{
    void Start()
    {
        DeviceTrackerARController.Instance.RegisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy() called.");

        DeviceTrackerARController.Instance.UnregisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
    }

    void OnDevicePoseStatusChanged(TrackableBehaviour.Status status, TrackableBehaviour.StatusInfo statusInfo)
    {
        Debug.Log("GroundPlaneUI.OnDevicePoseStatusChanged(" + status + ", " + statusInfo + ")");

        string statusMessage = "";

        switch (statusInfo)
        {
            case TrackableBehaviour.StatusInfo.NORMAL:
                statusMessage = "Normal Status";
                break;
            case TrackableBehaviour.StatusInfo.UNKNOWN:
                statusMessage = "Limited Status";
                break;
            case TrackableBehaviour.StatusInfo.INITIALIZING:
                statusMessage = "Point your device to the floor and move to scan";
                break;
            case TrackableBehaviour.StatusInfo.EXCESSIVE_MOTION:
                statusMessage = "Move slower";
                break;
            case TrackableBehaviour.StatusInfo.INSUFFICIENT_FEATURES:
                statusMessage = "Not enough visual features in the scene";
                break;
            case TrackableBehaviour.StatusInfo.INSUFFICIENT_LIGHT:
                statusMessage = "Not enough light in the scene";
                break;
            case TrackableBehaviour.StatusInfo.RELOCALIZING:
                // Display a relocalization message in the UI if:
                // * No AnchorBehaviours are being tracked
                // * None of the active/tracked AnchorBehaviours are in TRACKED status

                // Set the status message now and clear it none of conditions are met.
                statusMessage = "Point camera to previous position to restore tracking";

                StateManager stateManager = TrackerManager.Instance.GetStateManager();
                if (stateManager != null)
                {
                    // Cycle through all of the active AnchorBehaviours first.
                    foreach (TrackableBehaviour behaviour in stateManager.GetActiveTrackableBehaviours())
                    {
                        if (behaviour is AnchorBehaviour)
                        {
                            if (behaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED)
                            {
                                // If at least one of the AnchorBehaviours has Tracked status,
                                // then don't display the relocalization message.
                                statusMessage = "";
                            }
                        }
                    }
                }
                break;
            default:
                statusMessage = "";
                break;
        }

        setTextCanvas(statusMessage);
        // Uncomment the following line to show Status and StatusInfo values
        //StatusMessage.Instance.Display(status.ToString() + " -- " + statusInfo.ToString());
    }

    #region TESTING
    public Text textCanvas;

    public void setTextCanvas(string text)
    {
        textCanvas.text = text;
    }
    #endregion // Testing
}
