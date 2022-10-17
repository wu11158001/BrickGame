using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攝影機控制
/// </summary>
public class CameraControl : MonoBehaviour
{
    //Component
    static CameraControl cameraControl;
    public static CameraControl Instance => cameraControl;

    [Header("跟隨物件")]
    [SerializeField] Transform targetObject;//跟隨物件

    //數值
    readonly float[] distanceFromPlayer = new float[]{ 10, -8};//與玩家距離(高度, 深度)

    private void Awake()
    {
        if(cameraControl != null)
        {
            Destroy(this);
            return;
        }
        cameraControl = this;

        //初始位置/選轉
        transform.position = new Vector3(0, 10, -8);
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    private void Update()
    {
        OnFollowTarget();//跟隨目標物件   
    }

    /// <summary>
    /// 設定跟隨物件
    /// </summary>
    public Transform SetTargetObject { set { targetObject = value; } }

    /// <summary>
    /// 跟隨目標物件
    /// </summary>
    void OnFollowTarget()
    {
        if(targetObject != null)
        {
            transform.position = new Vector3(targetObject.position.x, targetObject.position.y + distanceFromPlayer[0], targetObject.position.z + distanceFromPlayer[1]);
        }
    }
}
