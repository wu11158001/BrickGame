using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �j���ϰ�
/// </summary>
public class BrickArea : MonoBehaviour
{
    //�سy���n
    const int buildWidth = 4;//�سy�e��
    const int buildLength = 6;//�سy����    
    const float areaSizeX = 3.5f;//�سy�ϰ쪫��SizeX
    const float buildY = 0.5f;//�سy�ϰ��mY
    const float areaSizeZ = 4.0f;//�سy�ϰ쪫��SizeZ

    //�ثe�سy���n
    [SerializeField]int width;//�ثe�سy�e��
    [SerializeField]int length;//�ثe�سy����    
    [SerializeField]float hight;//�ثe�سy����

    //�ƭ�
    const float createSpeed = 0.3f;//�Ыسt��
    const int maxCount = 50;//�̤j�ƶq
    Vector3 brickSize;//�j��Size

    //����
    [SerializeField]List<Transform> brick_List = new List<Transform>();//�����w�Ыؿj��    

    //�P�_
    bool isReadyCreate;//�O�_�ǳƳЫؿj��

    private void Awake()
    {        
        //�ثe�سy���n
        width = -1;//�ثe�سy�e��
        hight = buildY;//�ثe�سy����
    }

    private void Update()
    {
        if (!isReadyCreate && brick_List.Count < maxCount) StartCoroutine(OnCreateBrick());//�Ыؿj��
    }        

    /// <summary>
    /// ����j��
    /// </summary>
    /// <returns></returns>
    public Transform OnGetBrick()
    {
        //�P�_�j���ϰ�
        if (width > 0) width--;//�ثe�سy�e��
        else
        {
            if (length > 0)
            {
                width = buildWidth - 1;//�ثe�سy�e��
                length--;//�ثe�سy����
            }
            else
            {
                if (hight > buildY)
                {
                    width = buildWidth - 1;//�ثe�سy�e��
                    length = buildLength - 1;//�ثe�سy�e��
                    hight--;//�ثe�سy����
                }                
            }
        }
        if (brick_List.Count == 1) width = -1;

        if (brick_List.Count > 0)
        {         
            //�����w�Ыؿj�����
            Transform brick = brick_List[brick_List.Count - 1];
            brick_List.Remove(brick);
            return brick;
        }
        return default;
    }

    /// <summary>
    /// �Ыؿj��
    /// </summary>
    /// <returns></returns>
    IEnumerator OnCreateBrick()
    {
        isReadyCreate = true;//�O�_�ǳƳЫؿj��

        yield return new WaitForSeconds(createSpeed);

        //�Ыؿj��
        Transform obj_brick = Instantiate(GameManagement.Instance.brickObject.transform);
        obj_brick.gameObject.layer = LayerMask.NameToLayer("Brick");//����Layer
        obj_brick.SetParent(transform);
        brickSize = brickSize == Vector3.zero ? obj_brick.GetComponent<MeshFilter>().mesh.bounds.size : brickSize;//�j��Size
        obj_brick.localPosition = OnBrickPosition();//�j����m

        brick_List.Add(obj_brick);//�����w�Ыؿj��
        isReadyCreate = false;//�O�_�ǳƳЫؿj��
    }

    /// <summary>
    /// �j����m
    /// </summary>
    /// <returns></returns>
    Vector3 OnBrickPosition()
    {
        //�P�_�j���ϰ�
        if (width < buildWidth - 1) width++;//�ثe�سy�e��
        else
        {
            width = 0;//�ثe�سy�e��
            if (length < buildLength - 1) length++;//�ثe�سy����
            else
            {
                length = 0;//�ثe�سy�e��
                hight++;//�ثe�سy����
            }    
        }

        //�j���\���m       
        float X = -areaSizeX + (brickSize.x * 2 * width);
        float Y = brickSize.y * 2 * hight;
        float Z = areaSizeZ - (brickSize.z * 2 * length);        
        return new Vector3(X, Y, Z);
    }
}
