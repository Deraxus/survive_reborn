using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;

public class ItemMessageShowing : MonoBehaviour
{
    public bool isReady = true;

    public int messageIndex = 0;
    public float k = 1;
    public float timeRun = 1;
    public UI localUI;
    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        if (localUI == null )
        {
            localUI = GameObject.Find("UIObject").GetComponent<UI>();
        }
    }

    public void StartShowing()
    {
        transform.localPosition = startPos;
        localUI.ItemMessagesDeck[int.Parse(name[name.Length - 1].ToString()) - 1] = true;
        isReady = false;

        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(transform.DOLocalMoveX(0, timeRun).SetEase(Ease.OutBack, 2f));


        mySeq.AppendInterval(3f);
        mySeq.Append(transform.DOLocalMoveX(1, timeRun).SetEase(Ease.InBack));

        mySeq.OnComplete(() => {
            transform.localPosition = startPos;
            gameObject.GetComponent<TMP_Text>().text = "123";
            int targetIndex = int.Parse(name[name.Length - 1].ToString());
            DOTween.Sequence().AppendInterval(0f).OnComplete(() => { isReady = true; localUI.ItemMessagesDeck[int.Parse(name[name.Length - 1].ToString()) - 1] = false;});
            if ((targetIndex != localUI.ItemMessages.Count) && (localUI.ItemMessages[targetIndex].GetComponent<ItemMessageShowing>().isReady == false))
            {
                localUI.ItemMessages[targetIndex].GetComponent<ItemMessageShowing>().EditPosition();
            }
        });

    }

    public void EditPosition()
    {
        transform.DOLocalMoveY(0.25f, 0.35f).SetRelative().OnComplete(() => {
            if (isReady == true)
            {
                transform.localPosition = startPos;
            }
        });
        int targetIndex = int.Parse(name[name.Length - 1].ToString());
        if ((targetIndex != localUI.ItemMessages.Count) && (localUI.ItemMessages[targetIndex].GetComponent<ItemMessageShowing>().isReady == false))
        {
            localUI.ItemMessages[targetIndex].GetComponent<ItemMessageShowing>().EditPosition();
        }
    }
    
    
}
