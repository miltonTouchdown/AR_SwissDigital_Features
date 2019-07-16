using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneBag : MonoBehaviour
{
    public int id;
    public string nameZone;

    public float TimeTransition = 1f;
    public Texture2D CurrTexture;
    public Material matBag;
    public Material matFocus;

    public bool IsSetTexture = false;

    private Renderer m_renderer;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();

        m_renderer.sharedMaterial = matBag;

        // Hacer visible (alpha)
        matBag.SetFloat("Vector1_FAB99759", 0f);

        CurrTexture = (Texture2D)matBag.GetTexture("Texture2D_F572E918");
    }

    /// <summary>
    /// Cambiar el material del objecto para visualizacion
    /// </summary>
    /// <param name="modeView">Modo de visualizacion</param>
    public void ChangeModeView(ModeBagView modeView)
    {
        switch (modeView)
        {
            case ModeBagView.Focus:
                {
                    m_renderer.sharedMaterial = matFocus;

                    FocusTarget(false);
                    break;
                }
            case ModeBagView.Normal:
                {
                    m_renderer.sharedMaterial = matBag;
                    break;
                }
        }
    }

    public void SetTexture(Texture2D texture)
    {
        if (IsSetTexture)
            return;

        IsSetTexture = true;

        CurrTexture = texture;

        // Desaparecer (0f) objeto cambiando el alpha en el shader. Luego cambiar la texture. Aparecer (1f) el objeto.
        SetAlpha(false, () =>
        {
            // El nombre de la variable esta en el shader. Es generado por Unity
            matBag.SetTexture("Texture2D_F572E918", texture);

            LeanTween.delayedCall(.3f, () =>
            {
                SetAlpha(true, () => { IsSetTexture = false; });
            });
        });
    }

    public void FocusTarget(bool value)
    {
        float focus = (value) ? 1f : 0f;

        matFocus.SetFloat("Boolean_DAFC24A6", focus);
    }

    public void SetAlpha(bool isVisible, OnTaskComplete onTask = null)
    {
        float to = (isVisible) ? 0f : 1f;
        float from = (isVisible) ? 1f : 0f;

        LeanTween.value(this.gameObject, (f) =>
        {
            // Cambiar alpha
            matBag.SetFloat("Vector1_FAB99759", f);
        }, from, to, TimeTransition).setOnComplete(() =>
        {
            if (onTask != null)
                onTask();
        });
    }

    public Texture2D GetTexture2D()
    {
        return CurrTexture;
    }

    // Indica si se ha detectado el suelo
    public delegate void OnTaskComplete();
    public static event OnTaskComplete onTaskComplete;
}
