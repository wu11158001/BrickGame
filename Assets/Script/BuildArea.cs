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
    const int buildLength = 5;//�سy����    
    [SerializeField]float areaSizeX;//�سy�ϰ쪫��SizeX
    float areaSizeZ;//�سy�ϰ쪫��SizeZ

    //�ثe�سy���n
    int width;//�ثe�سy�e��
    int length;//�ثe�سy����    
    float hight;//�ثe�سy����

    //�j�����U
    const float initialFallHight = 5;//��l���U����
    List<BrickFall> brickFall_List = new List<BrickFall>();//�������U�j��

    //���ƪ���
    ScoreText scoreObject;
    
    private void Awake()
    {
        //�سy���n
        areaSizeX = GetComponent<MeshFilter>().mesh.bounds.size.x / 3;//�سy�ϰ쪫��SizeX
        areaSizeZ = GetComponent<MeshFilter>().mesh.bounds.size.z / 3;//�سy�ϰ쪫��SizeZ

        //�ثe�سy���n
        width = -1;//�ثe�سy�e��
        hight = 0.5f;//�ثe�سy����
    }

    private void Update()
    {
        OnBrickFall();//�j�����U
    }

    /// <summary>
    /// �]�w���ƪ���
    /// </summary>
    public ScoreText SetScoreObject { set { scoreObject = value; } }

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
        Vector3 brickSize = GameManagement.Instance.GetBrickSize;//����j��Size
        float X = -areaSizeX + (brickSize.x * width);
        float Y = brickSize.y * hight;
        float Z = areaSizeZ - (brickSize.z * length);
        Vector3 position = new Vector3(X, Y + initialFallHight, Z);
                
        //�]�w��m�P����
        birck.SetParent(transform);
        birck.localPosition = position;
        birck.localRotation = Quaternion.Euler(Vector3.zero);

        //����
        BrickFall brickFall = new BrickFall();
        brickFall.obj = birck;//���U����
        brickFall.targetPositionY = Y;//�ؼЦ�mY
        brickFall_List.Add(brickFall);//�������U�j��

        //�W�[����
        scoreObject.OnSetScore();
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
