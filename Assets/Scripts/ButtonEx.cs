using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 按钮扩展类
/// </summary>
public class ButtonEx : Button
{
    private RectTransform selfRect;
    public RectTransform rectTransform
    {
        get
        {
            if (selfRect == null)
            {
                selfRect = GetComponent<RectTransform>();
            }
            return selfRect;
        }
    }

    [Header("按键动效参数")]
    [SerializeField]
    private float maxScale = 1;                     // 最大缩放
    [SerializeField]
    private float minScl = 0.98f;                  // 最小缩放
    [SerializeField]
    private float timeScale = 0.12f;                 // 缩放时间
    [SerializeField]
    private ButtonType btnType = ButtonType.DoScale;         // 按键类型

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (btnType == ButtonType.DoScale)
        {
            rectTransform.DOKill();
            rectTransform.DOScale(Vector3.one * minScl, timeScale);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (btnType == ButtonType.DoScale)
        {
            rectTransform.DOKill();
            rectTransform.DOScale(Vector3.one * maxScale, timeScale);
        }
    }
}

public enum ButtonType
{
    None,
    DoScale,
}
