using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderBagControl : MonoBehaviour
{
    public ZoneBag[] arrZoneBags;

    [SerializeField]
    private ModeBagView m_currModeView = ModeBagView.Normal;

    void Start()
    {
        arrZoneBags = FindObjectsOfType<ZoneBag>();

        setModeView(ModeBagView.Normal);
    }

    public void setTexture(int id, Texture2D texture)
    {
        foreach(ZoneBag zb in arrZoneBags)
        {
            if(id == zb.id)
            {
                zb.SetTexture(texture);
            }else
            {
                zb.SetAlpha(false, () =>
                {
                    LeanTween.delayedCall(.3f, () =>
                    {
                        zb.SetAlpha(true);
                    });
                });
            }
        }
    }

    public void setFocus(int id)
    {
        foreach(ZoneBag zb in arrZoneBags)
        {
            zb.FocusTarget(id == zb.id);
        }
    }

    public void setModeView(ModeBagView modeView)
    {
        m_currModeView = modeView;

        foreach (ZoneBag zb in arrZoneBags)
        {
            zb.ChangeModeView(modeView);
        }
    }
}

public enum ModeBagView { None, Focus, Normal}