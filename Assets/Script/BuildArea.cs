using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �سy�ϰ�
/// </summary>
public class BuildArea : MonoBehaviour
{
    //�سy���n
    const int buildWidth = 4;//�سy�e��
    const int buildLength = 6;//�سy����    
    const float areaSizeX = 1.8f;//�سy�ϰ쪫��SizeX
    const float areaSizeZ = 2.0f;//�سy�ϰ쪫��SizeZ

    //�ثe�سy���n
    int width;//�ثe�سy�e��
    int length;//�ثe�سy����    
    float hight;//�ثe�سy����

    //�j�����U
    const float initialFallHight = 5;//��l���U����
    List<GameObject> brick_List = new List<GameObject>();//�����سy�j��
    List<BrickFall> brickFall_List = new List<BrickFall>();//�������U�j��

    Vector3 brickSize;//�j��Size

    //�سy�ƶq��r����
    BuildCountText buildCountTextObject;
    
    private void Awake()
    {
        OnInitial();//��l��
    }

    /// <summary>
    /// ��l��
    /// </summary>
    void OnInitial()
    {
        //�ثe�سy���n
        width = -1;//�ثe�سy�e��
        length = 0;//�ثe�سy����  
        hight = 0.5f;//�ثe�سy����
    }

    private void Update()
    {
        OnBrickFall();//�j�����U
    }

    /// <summary>
    /// �]�w�سy�ƶq��r����
    /// </summary>
    public BuildCountText SetBuildCountTextObject { set { buildCountTextObject = value; } }

    /// <summary>
    /// �j�����U
    /// </summary>
    void OnBrickFall()
    {
        if (brickFall_List.Count > 0)
        {
            for (int i = 0; i < brickFall_List.Count; i++)
            {
                brickFall_List[i].OnFall();//�j�����U

                //��F�ؼЦ�m
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
    /// �سy��m
    /// </summary>
    /// <returns></returns>
    public void OnBuildPosition(Transform birck)
    {        
        //�سy�ϰ�
        if (width < buildWidth - 1) width++;//�ثe�سy�e��
        else
        {
            width = 0;//�ثe�سy�e��
            if (length < buildLength - 1) length++;//�ثe�سy����            
            else
            {
                length = 0;//�ثe�سy����
                hight++;//�ثe�سy����
            }
        }

        //�j���\���m
        brickSize = brickSize == Vector3.zero ? birck.GetComponent<MeshFilter>().mesh.bounds.size : brickSize;//�j��Size
        float X = -areaSizeX + (brickSize.x * width);
        float Y = brickSize.y * hight;
        float Z = areaSizeZ - (brickSize.z * length);
        Vector3 position = new Vector3(X, Y + initialFallHight, Z);
                
        //�]�w��m�P����
        birck.SetParent(transform);
        birck.localPosition = position;
        birck.localRotation = Quaternion.Euler(Vector3.zero);
        brick_List.Add(birck.gameObject);//�����سy�j��

        //�������U�j��
        BrickFall brickFall = new BrickFall();
        brickFall.obj = birck;//���U����
        brickFall.targetPositionY = Y;//�ؼЦ�mY
        brickFall_List.Add(brickFall);//�������U�j��

        //�W�[����
        if(buildCountTextObject.OnSetScore())
        {
            foreach (var brick in brick_List)
            {
                brick.SetActive(false);//�����j������
            }

            //�M������
            brickFall_List.Clear();
            brick_List.Clear();
            OnInitial();//��l��
        }
    }
}

/// <summary>
/// �j�����U
/// </summary>
class BrickFall
{
    public Transform obj;//���U����
    public float targetPositionY;//�ؼЦ�mY

    const float fallSpeed = 10;//���U�t��

    /// <summary>
    /// ���U
    /// </summary>
    public void OnFall()
    {
        if(obj.position.y > targetPositionY)
        {
            obj.position = obj.position + Vector3.down * fallSpeed * Time.deltaTime;            
        }
    }
}
