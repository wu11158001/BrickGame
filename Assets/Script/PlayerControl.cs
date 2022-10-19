using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制
/// </summary>
public class PlayerControl : MonoBehaviour
{
    //Component
    Animator animator;
    BoxCollider boxCollider;

    //輸入值
    float horizontal;//水平
    float vertical;//垂直
    Vector2 mouseDownPosition;//滑鼠點擊位置

    //移動
    const float moveSpeed = 10;//移動速度

    //向量
    Vector3 objectForward;//物件前方向量

    //磚塊
    const float moveBrickSpeed = 0.1f;//移動磚塊速度
    const int maxAvailableQuantity = 50;//最大可擁有數量
    int currentBrick;//目前擁有磚塊
    Transform brickParent;//磚塊父物件
    readonly Vector3 initialBrickPosition = new Vector3(0, 1f, -0.3f);////磚塊擺放位置    
    float dorpBrickCountdown;//放下磚塊(計時器)    

    private void Awake()
    {
        //Component
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        //向量
        objectForward = transform.forward;

        brickParent = FindChildMethod.OnFindChild<Transform>(transform, "Bricks");//磚塊擺放位置

        CameraControl.Instance.SetTargetObject = transform;//設定攝影機跟隨物件
    }

    private void Update()
    {
        OnMoveControl();//移動控制        
        OnCollsion();//碰撞        
        OnExitGame();//離開遊戲
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    void OnExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

            Application.Quit();
        }
    }

    /// <summary>
    /// 移動控制
    /// </summary>
    void OnMoveControl()
    {
        //鍵盤輸入值
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");            
        OnMouseInput();//滑鼠輸入

        //總輸入值
        float inputValue = Mathf.Clamp(Mathf.Abs(horizontal) + Mathf.Abs(vertical), 0, 1);
        
        //轉向
        Vector3 v3_horizontal = Vector3.Cross(Vector3.up, objectForward * horizontal);
        Vector3 v3_vertical = Vector3.Cross(Vector3.right, Vector3.up * vertical);
        transform.forward = Vector3.RotateTowards(transform.forward, v3_horizontal + v3_vertical, 1, 0);

        //移動
        transform.position = transform.position + transform.forward * inputValue * moveSpeed * Time.deltaTime;      

        animator.SetFloat("Run", inputValue);
    }

    /// <summary>
    /// 滑鼠輸入
    /// </summary>
    void OnMouseInput()
    {        
        //紀錄滑鼠點擊位置
        if (Input.GetMouseButtonDown(0)) mouseDownPosition = Input.mousePosition;

        //滑鼠拖曳
        if (Input.GetMouseButton(0))
        {
            horizontal = Input.mousePosition.x - mouseDownPosition.x;
            vertical = Input.mousePosition.y - mouseDownPosition.y;
        }   
    }

    /// <summary>
    /// 碰撞
    /// </summary>
    void OnCollsion()
    {
        Collider[] obj_buildArea = OnCollisionArea("BuildArea");//建造區域碰撞
        Collider[] obj_brickArea = OnCollisionArea("BrickArea");//磚塊區域碰撞        

        OnAreaBehavior(colliders: obj_buildArea, action: OnDropBrick);//建造區域碰撞
        OnAreaBehavior(colliders: obj_brickArea, action: OnTakeBruck);//磚塊區域碰撞
    }

    /// <summary>
    /// 碰撞區域
    /// </summary>
    /// <param name="layer">碰撞Layer</param>
    Collider[] OnCollisionArea(string layer)
    {
        return Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size, Quaternion.Euler(transform.localEulerAngles), 1 << LayerMask.NameToLayer(layer));
    }

    /// <summary>
    /// 碰撞區域行為
    /// </summary>
    /// <param name="colliders">碰撞區域</param>
    /// <param name="action">執行涵式</param>
    void OnAreaBehavior(Collider[] colliders, Action<Transform> action)
    {
        if(colliders.Length > 0)
        {
            dorpBrickCountdown -= Time.deltaTime;//放下磚塊(計時器)
            if (dorpBrickCountdown <= 0)
            {
                dorpBrickCountdown = moveBrickSpeed;//重製放下磚塊(計時器)
                action.Invoke(colliders[0].transform);
            }
        }
    }

    /// <summary>
    /// 放下磚塊
    /// </summary>
    /// <param name="area">建造區域</param>
    void OnDropBrick(Transform area)
    {
        Transform obj_bricks = FindChildMethod.OnFindChild<Transform>(transform, "Bricks");
        if (obj_bricks.childCount > 0)
        {
            area.GetComponent<BuildArea>().OnBuildPosition(obj_bricks.GetChild(obj_bricks.childCount - 1));
            currentBrick--;//目前擁有磚塊數量
        }
    }

    /// <summary>
    /// 拿起磚塊
    /// </summary>
    /// <param name="area"></param>
    void OnTakeBruck(Transform area)
    {
        if (currentBrick < maxAvailableQuantity)//目前擁有磚塊數量
        {
            Transform brick = area.GetComponent<BrickArea>().OnGetBrick();//獲取磚塊
            if (brick == null) return;
            brick.SetParent(brickParent);//設定Parent
            Vector3 brickSize = brick.GetComponent<MeshFilter>().mesh.bounds.size;//磚塊Size
            brick.transform.localPosition = new Vector3(initialBrickPosition.x, initialBrickPosition.y + (brickSize.y / 3 * currentBrick), initialBrickPosition.z);//位置
            brick.localRotation = Quaternion.Euler(Vector3.zero);//選轉

            currentBrick++;//目前擁有磚塊數量
        }
    }
}
