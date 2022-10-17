using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��v������
/// </summary>
public class CameraControl : MonoBehaviour
{
    //Component
    static CameraControl cameraControl;
    public static CameraControl Instance => cameraControl;

    [Header("���H����")]
    [SerializeField] Transform targetObject;//���H����

    //�ƭ�
    readonly float[] distanceFromPlayer = new float[]{ 10, -8};//�P���a�Z��(����, �`��)

    private void Awake()
    {
        if(cameraControl != null)
        {
            Destroy(this);
            return;
        }
        cameraControl = this;

        //��l��m/����
        transform.position = new Vector3(0, 10, -8);
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    private void Update()
    {
        OnFollowTarget();//���H�ؼЪ���   
    }

    /// <summary>
    /// �]�w���H����
    /// </summary>
    public Transform SetTargetObject { set { targetObject = value; } }

    /// <summary>
    /// ���H�ؼЪ���
    /// </summary>
    void OnFollowTarget()
    {
        if(targetObject != null)
        {
            transform.position = new Vector3(targetObject.position.x, targetObject.position.y + distanceFromPlayer[0], targetObject.position.z + distanceFromPlayer[1]);
        }
    }
}
