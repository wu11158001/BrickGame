using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���Ƥ�r
/// </summary>
public class ScoreText : MonoBehaviour
{
    //Conponent
    Text thisText;
    Transform targetObject;//�ؼЪ���

    //��r
    int score;//����
    const int fullScore = 100;//����

    private void Awake()
    {
        thisText = GetComponent<Text>();

        thisText.text = score + "/" + fullScore;
    }

    private void Update()
    {
        OnScorePosition();//���Ʀ�m
    }

    /// <summary>
    /// �]�w�ؼ�
    /// </summary>
    public Transform SetTarget { set { targetObject = value; } }

    /// <summary>
    /// �W�[����
    /// </summary>
    public void OnSetScore()
    {
        score++;//���ƼW�[

        //��r
        thisText.text = score + "/" + fullScore;
    }

    /// <summary>
    /// ���Ʀ�m
    /// </summary>
    void OnScorePosition()
    {
        if (targetObject == null) return;

        //��m
        Vector3 position = Camera.main.WorldToScreenPoint(targetObject.position);
        transform.position = position;
    }
}
