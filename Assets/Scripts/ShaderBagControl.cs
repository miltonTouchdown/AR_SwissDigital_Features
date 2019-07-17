using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderBagControl : MonoBehaviour
{
    public ZoneBag[] arrZoneBags;

    public bool IsShow = false;

    [SerializeField]
    private ModeBagView m_currModeView = ModeBagView.Normal;

    void Start()
    {
        arrZoneBags = FindObjectsOfType<ZoneBag>();

        foreach(ZoneBag z in arrZoneBags)
        {
            z.SetShaderControl(this);
        }

        setModeView(ModeBagView.Normal);
    }

    public void ShowObject()
    {
        //SetActiveObject(true);
        if (IsShow)
            return;

        IsShow = true;

        foreach (ZoneBag zb in arrZoneBags)
        {
            zb.Show();
        }
    }

    public void HideObject()
    {
        //SetActiveObject(false);
        if (!IsShow)
            return;

        IsShow = false;

        foreach (ZoneBag zb in arrZoneBags)
        {
            zb.Hide();
        }
    }

    public void setTexture(int id, Texture2D texture)
    {
        if (m_currModeView != ModeBagView.Normal)
            return;

        foreach (ZoneBag zb in arrZoneBags)
        {
            if (zb.ModeView == ModeBagView.Normal)
            {
                if (id == zb.id)
                {
                    zb.SetTexture(texture);
                }
                else
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
    }

    public void setFocus(int id)
    {
        if (m_currModeView != ModeBagView.Focus)
            return;

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

    #region TEST
    public void setModeView(int index)
    {
        setModeView((ModeBagView)index);
    }

    public void setTexture1(Texture2D texture)
    {
        setTexture(0, texture);
    }

    public void setTexture2(Texture2D texture)
    {
        setTexture(1, texture);
    }

    public void setTexture3(Texture2D texture)
    {
        setTexture(2, texture);
    }
    #endregion // TEST
}

public enum ModeBagView { None, Focus, Normal}