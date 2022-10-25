using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MokugyoCell : MonoBehaviour
{
    public UIMain UIMain;

    [SerializeField]
    public ButtonEx Btn_Mokugyo;

    [SerializeField]
    public Text Txt_Mokugyo;

    public Vector3 TxtLocalPos;

    [SerializeField]
    private AudioSource AudioSource;

    void Start()
    {
        TxtLocalPos = new Vector3(190, 190, 0);
        Txt_Mokugyo.gameObject.SetActive(false);
    }

    public void OnClick_Mokugyo()
    {
        AudioSource.Play();
        UIMain.MeritCount++;
        UIMain.RefreshUI();

        Txt_Mokugyo.transform.DOKill();
        Txt_Mokugyo.gameObject.SetActive(true);
        Txt_Mokugyo.transform.localPosition = TxtLocalPos;
        var doTween = Txt_Mokugyo.transform.DOLocalMove(new Vector3(TxtLocalPos.x, TxtLocalPos.y + 10, TxtLocalPos.z),0.12f);
        doTween.onComplete = () => {
            Txt_Mokugyo.gameObject.SetActive(false);
        }; 
        doTween.onKill = () => {
            Txt_Mokugyo.gameObject.SetActive(false);
        };
    }
}
