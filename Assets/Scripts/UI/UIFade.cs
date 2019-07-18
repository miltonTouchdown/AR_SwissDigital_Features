using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    private void Start()
    {
    }

    public void Fade(float to, float time, OnFinishFade onFinish = null)
    {
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), to, time).setOnComplete(() => {
            if(onFinish != null)
                onFinish();
        });
    }

    // Indica si se ha detectado el suelo
    public delegate void OnFinishFade();
    public static event OnFinishFade onFinishFade;
}
