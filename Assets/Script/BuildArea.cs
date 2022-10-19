using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建造區域
/// </summary>
public class BuildArea : MonoBehaviour
{
    //建造面積
    const int buildWidth = 4;//建造寬度
    const int buildLength = 6;//建造長度    
    const float areaSizeX = 1.8f;//建造區域物件SizeX
    const float areaSizeZ = 2.0f;//建造區域物件SizeZ

    //目前建造面積
    int width;//目前建造寬度
    int length;//目前建造長度    
    float hight;//目前建造高度

    //磚塊落下
    const float initialFallHight = 5;//初始落下高度
    List<GameObject> brick_List = new List<GameObject>();//紀錄建造磚塊
    List<BrickFall> brickFall_List = new List<BrickFall>();//紀錄落下磚塊

    Vector3 brickSize;//磚塊Size

    //建造數量文字物件
    BuildCountText buildCountTextObject;
    
    private void Awake()
    {
        OnInitial();//初始化
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void OnInitial()
    {
        //目前建造面積
        width = -1;//目前建造寬度
        length = 0;//目前建造長度  
        hight = 0.5f;//目前建造高度
    }

    private void Update()
    {
        OnBrickFall();//磚塊落下
    }

    /// <summary>
    /// 設定建造數量文字物件
    /// </summary>
    public BuildCountText SetBuildCountTextObject { set { buildCountTextObject = value; } }

    /// <summary>
    /// 磚塊落下
    /// </summary>
    void OnBrickFall()
    {
        if (brickFall_List.Count > 0)
        {
            for (int i = 0; i < brickFall_List.Count; i++)
            {
                brickFall_List[i].OnFall();//磚塊落下

                //到達目標位置
                if (brickFall_List[i].obj.position.y <= brickFall_List[i].targetPositionY)
                {                    
                    brickFall_List[i].obj.position = new Vector3(brickFall_List[i].obj.position.x, 
                                                                 brickFall_List[i].targetPositionY, 
                                                                 brickFall_List[i].obj.position.z);
                    brickFall_List.Remove(brickFall_List[i]);
                }
            }            
        }
    }

    /// <summary>
    /// 建造位置
    /// </summary>
    /// <returns></returns>
    public void OnBuildPosition(Transform birck)
    {        
        //建造區域
        if (width < buildWidth - 1) width++;//目前建造寬度
        else
        {
            width = 0;//目前建造寬度
            if (length < buildLength - 1) length++;//目前建造長度            
            else
            {
                length = 0;//目前建造長度
                hight++;//目前建造高度
            }
        }

        //磚塊擺放位置
        brickSize = brickSize == Vector3.zero ? birck.GetComponent<MeshFilter>().mesh.bounds.size : brickSize;//磚塊Size
        float X = -areaSizeX + (brickSize.x * width);
        float Y = brickSize.y * hight;
        float Z = areaSizeZ - (brickSize.z * length);
        Vector3 position = new Vector3(X, Y + initialFallHight, Z);
                
        //設定位置與選轉
        birck.SetParent(transform);
        birck.localPosition = position;
        birck.localRotation = Quaternion.Euler(Vector3.zero);
        brick_List.Add(birck.gameObject);//紀錄建造磚塊

        //紀錄落下磚塊
        BrickFall brickFall = new BrickFall();
        brickFall.obj = birck;//落下物件
        brickFall.targetPositionY = Y;//目標位置Y
        brickFall_List.Add(brickFall);//紀錄落下磚塊

        //增加分數
        if(buildCountTextObject.OnSetScore())
        {
            foreach (var brick in brick_List)
            {
                brick.SetActive(false);//關閉磚塊物件
            }

            //清除紀錄
            brickFall_List.Clear();
            brick_List.Clear();
            OnInitial();//初始化
        }
    }
}

/// <summary>
/// 磚塊落下
/// </summary>
class BrickFall
{
    public Transform obj;//落下物件
    public float targetPositionY;//目標位置Y

    const float fallSpeed = 10;//落下速度

    /// <summary>
    /// 落下
    /// </summary>
    public void OnFall()
    {
        if(obj.position.y > targetPositionY)
        {
            obj.position = obj.position + Vector3.down * fallSpeed * Time.deltaTime;            
        }
    }
}
