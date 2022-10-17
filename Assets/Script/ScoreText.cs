using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// だ计ゅ
/// </summary>
public class ScoreText : MonoBehaviour
{
    //Conponent
    Text thisText;
    Transform targetObject;//ヘ夹ン

    //ゅ
    int score;//だ计
    const int fullScore = 100;//骸だ

    private void Awake()
    {
        thisText = GetComponent<Text>();

        thisText.text = score + "/" + fullScore;
    }

    private void Update()
    {
        OnScorePosition();//だ计竚
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
        score++;//だ计糤

        //ゅ
        thisText.text = score + "/" + fullScore;
    }

    /// <summary>
    /// だ计竚
    /// </summary>
    void OnScorePosition()
    {
        if (targetObject == null) return;

        //竚
        Vector3 position = Camera.main.WorldToScreenPoint(targetObject.position);
        transform.position = position;
    }
}
