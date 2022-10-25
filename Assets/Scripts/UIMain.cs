using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField]
    private RectTransform UIRoot;

    [SerializeField]
    private GameObject MokugyoCell;

    [SerializeField]
    private UIQuickMerit UIQuickMerit;

    [SerializeField]
    private ScrollRect Sv_Mokugyo;

    [SerializeField]
    private Text Txt_Merit;

    [SerializeField]
    private ButtonEx Btn_Setting;

    [SerializeField]
    private Button Btn_Quick;

    /// <summary>
    /// 按钮图片列表
    /// </summary>
    public List<Sprite> IconList;

    /// <summary>
    /// 功德计数
    /// </summary>
    public int MeritCount;

    /// <summary>
    /// 是否进行时
    /// </summary>
    public bool IsStart;

    private void Awake()
    {
        DontDestroyOnLoad(UIRoot);
    }

    private void Start()
    {
        MokugyoCell.gameObject.SetActive(false);
        CreateObj( );
        RefreshUI();
    }

    public void RefreshUI()
    {
        Txt_Merit.text = MeritCount.ToString();
    }

    public void OnClick_Setting()
    {
        UIQuickMerit.gameObject.SetActive(!UIQuickMerit.gameObject.activeSelf);
    }

    public void OnClick_Quick()
    {
        var inputCountStr = UIQuickMerit.Input_Count.text;
        int count = string.IsNullOrEmpty(inputCountStr) ? 1 : inputCountStr == "0" ? 1 : int.Parse(inputCountStr);
        var inputSpeedStr = UIQuickMerit.Input_Speed.text;
        int speed = string.IsNullOrEmpty(inputSpeedStr) ? 1000 : int.Parse(inputSpeedStr);

        IsStart = !IsStart;
        Btn_Quick.image.sprite = IsStart ? IconList[1] : IconList[0];
        if (IsStart)
        {
            StartCoroutine(AutoClickMokugyo(count, speed));
        }
        else
        {
            StopAllCoroutines();
            for (int i = 0; i < ObjList.Count; i++)
            {
                var btn = ObjList[i].Btn_Mokugyo;
                btn.rectTransform.DOKill();
                btn.rectTransform.localScale = Vector3.one;
            }
            RecycleAll();
            CreateObj();
        }
    }

    private IEnumerator AutoClickMokugyo(int count, int speed)
    {
        UIQuickMerit.gameObject.SetActive(false);
        RecycleAll();
        for (int i = 0; i < count; i++)
        {
            CreateObj();
        }

        while (true)
        {
            for (int i = 0; i < ObjList.Count; i++)
            {
                var btn = ObjList[i].Btn_Mokugyo;
                btn.rectTransform.DOKill();
                btn.rectTransform.localScale = Vector3.one;
                btn.rectTransform.DOScale(Vector3.one * 0.98f, 0.12f).onComplete = () => 
                {
                    btn.rectTransform.DOScale(Vector3.one, 0.12f);
                };
                btn.onClick.Invoke();
            }
            yield return new WaitForSeconds((float)speed / 1000);
        }
    }

    #region 对象池
    /// <summary>
    /// 池队列
    /// </summary>
    public Queue<MokugyoCell> ObjQueue { private set; get; } = new Queue<MokugyoCell>();

    /// <summary>
    /// 已创建出来的对象
    /// </summary>
    public List<MokugyoCell> ObjList { private set; get; } = new List<MokugyoCell>();

    private void CreateObj()
    {
        MokugyoCell cell = null;
        if (ObjQueue.Count != 0)
        {
            cell = ObjQueue.Dequeue();
        }
        else
        {
            cell = Instantiate(MokugyoCell, Sv_Mokugyo.content).GetComponent<MokugyoCell>();
        }
        cell.gameObject.SetActive(true);
        ObjList.Add(cell);
    }

    private void Recycle(MokugyoCell cell)
    {
        cell.gameObject.SetActive(false);
        ObjList.Remove(cell);
        ObjQueue.Enqueue(cell);
    }

    public void RecycleAll()
    {
        for (int i = 0; i < ObjList.Count; i++)
        {
            var cell = ObjList[i];
            cell.gameObject.SetActive(false);
            ObjQueue.Enqueue(cell);
        }
        ObjList.Clear();
    }
    #endregion
}
