using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �سy�ƶq��r
/// </summary>
public class BuildCountText : MonoBehaviour
{
    //Conponent
    Text thisText;
    Transform targetObject;//�ؼЪ���    

    //�سy�ƶq
    int currentCount;//�ثe�ƶq
    const int completeCount = 30;//�سy�����ƶq
    const int addScore = 100;//�[��

    private void Awake()
    {
        thisText = GetComponent<Text>();       

        //��r
        thisText.text = currentCount + "/" + completeCount;//�سy�����ƶq
    }

    private void Update()
    {
        OnPosition();//���Ʀ�m
    }

    /// <summary>
    /// �]�w�ؼ�
    /// </summary>
    public Transform SetTarget { set { targetObject = value; } }

    /// <summary>
    /// �W�[����
    /// </summary>
    public bool OnSetScore()
    {
        bool isComplete = false;//�O�_�سy����
        currentCount++;//�ثe�ƶq�W�[

        //�����سy�ƶq
        if (currentCount == completeCount)
        {
            isComplete = true;//�O�_�سy����
            currentCount = 0;
            GameManagement.Instance.currentScore += addScore;//�[��
            GameManagement.Instance.score_Text.text = $"����: {GameManagement.Instance.currentScore}";
        }

        //��r
        thisText.text = currentCount + "/" + completeCount;

        return isComplete;
    }

    /// <summary>
    /// ��m
    /// </summary>
    void OnPosition()
    {
        if (targetObject == null) return;

        //��m
        Vector3 position = Camera.main.WorldToScreenPoint(targetObject.position);
        transform.position = position;
    }
}
