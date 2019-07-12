﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using TMPro;

public class UIMenuAR : MonoBehaviour
{
    [Header("Snack Bar")]
    public WindowMovement snackBar;
    public TextMeshProUGUI snackText;

    [Header("Video Feedback")]
    public RawImageVideoPlayer imageVideo;

    private bool m_IsGroundDetected = false;

    void Start()
    {
        DeviceTrackerARController.Instance.RegisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
        ARManager.onHitTest += OnHitGround;
    }

    void OnDestroy()
    {
        DeviceTrackerARController.Instance.UnregisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
        ARManager.onHitTest -= OnHitGround;
    }

    public void ShowSnackBar(string message)
    {
        if (snackBar.isActive)
            return;

        snackBar.setActiveWindow(true);
        snackText.text = message;
    }

    public void HideSnackBar()
    {
        if (!snackBar.isActive)
            return;

        snackBar.setActiveWindow(false);
    }

    void OnDevicePoseStatusChanged(TrackableBehaviour.Status status, TrackableBehaviour.StatusInfo statusInfo)
    {
        string statusMessage = "";

        switch (statusInfo)
        {
            case TrackableBehaviour.StatusInfo.NORMAL:
                {
                    statusMessage = "";
                    HideSnackBar();
                    if (imageVideo.IsPlaying) imageVideo.StopVideo();

                    break;
                }
            case TrackableBehaviour.StatusInfo.UNKNOWN:
                statusMessage = "No hay suficientes características visuales (<b>ver en botón información</b>)";
                break;
            case TrackableBehaviour.StatusInfo.INITIALIZING:
                statusMessage = "<b>Apunta</b> con tu dispositivo al suelo y <b>mueve</b> para escanear";
                break;
            case TrackableBehaviour.StatusInfo.EXCESSIVE_MOTION:
                statusMessage = "Mueve tu dispositivo mas <b>lento</b>";
                break;
            case TrackableBehaviour.StatusInfo.INSUFFICIENT_FEATURES:
                statusMessage = "No hay suficientes características visuales (<b>ver en botón información</b>)";
                break;
            case TrackableBehaviour.StatusInfo.INSUFFICIENT_LIGHT:
                statusMessage = "No hay suficiente <b>luz</b>";
                break;
            case TrackableBehaviour.StatusInfo.RELOCALIZING:
                // Display a relocalization message in the UI if:
                // * No AnchorBehaviours are being tracked
                // * None of the active/tracked AnchorBehaviours are in TRACKED status

                // Set the status message now and clear it none of conditions are met.
                statusMessage = "<b>Mueve</b> la cámara al punto anterior para restaurar la detección";

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

        if(statusInfo != TrackableBehaviour.StatusInfo.NORMAL)
        {
            ShowSnackBar(statusMessage);
            if(!imageVideo.IsPlaying) imageVideo.PlayVideo();
        }else if (!m_IsGroundDetected)
        {
            statusMessage = "<b>Apunta</b> con tu dispositivo al suelo y <b>mueve</b> para escanear";
            ShowSnackBar(statusMessage);
            if (!imageVideo.IsPlaying) imageVideo.PlayVideo();
        }
    }

    private void OnHitGround(bool value)
    {
        m_IsGroundDetected = value;

        setMessage("is ground detected: " + value);

        if (!m_IsGroundDetected)
        { 
            string statusMessage = "<b>Apunta</b> con tu dispositivo al suelo y <b>mueve</b> para escanear";
            ShowSnackBar(statusMessage);
            if (!imageVideo.IsPlaying) imageVideo.PlayVideo();
        }
        else
        {
            HideSnackBar();
            if (imageVideo.IsPlaying) imageVideo.StopVideo();
        }   
    }

    public Text uiText;

    public void setMessage(string t)
    {
        uiText.text = t;
    }
}