using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderBagControl : MonoBehaviour
{
    public float TimeTransition = 1f;
    public Texture2D CurrTexture;
    public Texture2D testTexture;
    public Material matBag;

    public bool IsSetTexture = false;

    void Start()
    {
        // Hacer visible (alpha)
        matBag.SetFloat("Vector1_FAB99759", 0f);
    }

    public void SetTexture(Texture2D texture)
    {
        if (IsSetTexture)
            return;

        IsSetTexture = true;
        
        // Desaparecer (0f) objeto cambiando el alpha en el shader. Luego cambiar la texture. Aparecer (1f) el objeto.
        SetAlpha(false, () => 
        {
            // El nombre de la variable esta en el shader. Es generado por Unity
            matBag.SetTexture("Texture2D_F572E918", texture);

            LeanTween.delayedCall(.3f, () => 
            {
                SetAlpha(true, ()=> { IsSetTexture = false; });
            });
        });
    }

    public void FocusTarget()
    {

    }

    public void SetAlpha(bool isVisible, OnTaskComplete onTask = null)
    {
        float to = (isVisible) ? 0f : 1f;
        float from = (isVisible) ? 1f : 0f;

        LeanTween.value(this.gameObject, (f) =>
        {
            // Cambiar alpha
            matBag.SetFloat("Vector1_FAB99759", f);
        }, from, to, TimeTransition).setOnComplete(()=> 
        {
            if (onTask != null)
                onTask();
        });
    }

    // Indica si se ha detectado el suelo
    public delegate void OnTaskComplete();
    public static event OnTaskComplete onTaskComplete;
}
