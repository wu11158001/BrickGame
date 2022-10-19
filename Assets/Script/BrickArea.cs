using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 磚塊區域
/// </summary>
public class BrickArea : MonoBehaviour
{
    //建造面積
    const int buildWidth = 4;//建造寬度
    const int buildLength = 6;//建造長度    
    const float areaSizeX = 3.5f;//建造區域物件SizeX
    const float buildY = 0.5f;//建造區域位置Y
    const float areaSizeZ = 4.0f;//建造區域物件SizeZ

    //目前建造面積
    [SerializeField]int width;//目前建造寬度
    [SerializeField]int length;//目前建造長度    
    [SerializeField]float hight;//目前建造高度

    //數值
    const float createSpeed = 0.3f;//創建速度
    const int maxCount = 50;//最大數量
    Vector3 brickSize;//磚塊Size

    //紀錄
    [SerializeField]List<Transform> brick_List = new List<Transform>();//紀錄已創建磚塊    

    //判斷
    bool isReadyCreate;//是否準備創建磚塊

    private void Awake()
    {        
        //目前建造面積
        width = -1;//目前建造寬度
        hight = buildY;//目前建造高度
    }

    private void Update()
    {
        if (!isReadyCreate && brick_List.Count < maxCount) StartCoroutine(OnCreateBrick());//創建磚塊
    }        

    /// <summary>
    /// 獲取磚塊
    /// </summary>
    /// <returns></returns>
    public Transform OnGetBrick()
    {
        //判斷磚塊區域
        if (width > 0) width--;//目前建造寬度
        else
        {
            if (length > 0)
            {
                width = buildWidth - 1;//目前建造寬度
                length--;//目前建造長度
            }
            else
            {
                if (hight > buildY)
                {
                    width = buildWidth - 1;//目前建造寬度
                    length = buildLength - 1;//目前建造寬度
                    hight--;//目前建造高度
                }                
            }
        }
        if (brick_List.Count == 1) width = -1;

        if (brick_List.Count > 0)
        {         
            //紀錄已創建磚塊減少
            Transform brick = brick_List[brick_List.Count - 1];
            brick_List.Remove(brick);
            return brick;
        }
        return default;
    }

    /// <summary>
    /// 創建磚塊
    /// </summary>
    /// <returns></returns>
    IEnumerator OnCreateBrick()
    {
        isReadyCreate = true;//是否準備創建磚塊

        yield return new WaitForSeconds(createSpeed);

        //創建磚塊
        Transform obj_brick = Instantiate(GameManagement.Instance.brickObject.transform);
        obj_brick.gameObject.layer = LayerMask.NameToLayer("Brick");//物件Layer
        obj_brick.SetParent(transform);
        brickSize = brickSize == Vector3.zero ? obj_brick.GetComponent<MeshFilter>().mesh.bounds.size : brickSize;//磚塊Size
        obj_brick.localPosition = OnBrickPosition();//磚塊位置

        brick_List.Add(obj_brick);//紀錄已創建磚塊
        isReadyCreate = false;//是否準備創建磚塊
    }

    /// <summary>
    /// 磚塊位置
    /// </summary>
    /// <returns></returns>
    Vector3 OnBrickPosition()
    {
        //判斷磚塊區域
        if (width < buildWidth - 1) width++;//目前建造寬度
        else
        {
            width = 0;//目前建造寬度
            if (length < buildLength - 1) length++;//目前建造長度
            else
            {
                length = 0;//目前建造寬度
                hight++;//目前建造高度
            }    
        }

        //磚塊擺放位置       
        float X = -areaSizeX + (brickSize.x * 2 * width);
        float Y = brickSize.y * 2 * hight;
        float Z = areaSizeZ - (brickSize.z * 2 * length);        
        return new Vector3(X, Y, Z);
    }
}
