using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 硑计秖ゅ
/// </summary>
public class BuildCountText : MonoBehaviour
{
    //Conponent
    Text thisText;
    Transform targetObject;//ヘ夹ン    

    //硑计秖
    int currentCount;//ヘ玡计秖
    const int completeCount = 100;//硑ЧΘ计秖
    const int addScore = 100;//だ

    private void Awake()
    {
        thisText = GetComponent<Text>();       

        //ゅ
        thisText.text = currentCount + "/" + completeCount;//硑ЧΘ计秖
    }

    private void Update()
    {
        OnPosition();//だ计竚
    }

    /// <summary>
    /// 砞﹚ヘ夹
    /// </summary>
    public Transform SetTarget { set { targetObject = value; } }

    /// <summary>
    /// 糤だ计
    /// </summary>
    public void OnSetScore()
    {
        currentCount++;//ヘ玡计秖糤

        //ЧΘ硑计秖
        if (currentCount == completeCount)
        {
            currentCount = 0;
            GameManagement.Instance.currentScore += addScore;//だ
            GameManagement.Instance.score_Text.text = $"だ计: {GameManagement.Instance.currentScore}";
        }

        //ゅ
        thisText.text = currentCount + "/" + completeCount;        
    }

    /// <summary>
    /// 竚
    /// </summary>
    void OnPosition()
    {
        if (targetObject == null) return;

        //竚
        Vector3 position = Camera.main.WorldToScreenPoint(targetObject.position);
        transform.position = position;
    }
}
