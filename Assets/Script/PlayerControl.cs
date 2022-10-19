using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a����
/// </summary>
public class PlayerControl : MonoBehaviour
{
    //Component
    Animator animator;
    BoxCollider boxCollider;

    //��J��
    float horizontal;//����
    float vertical;//����
    Vector2 mouseDownPosition;//�ƹ��I����m

    //����
    const float moveSpeed = 10;//���ʳt��

    //�V�q
    Vector3 objectForward;//����e��V�q

    //�j��
    const float moveBrickSpeed = 0.1f;//���ʿj���t��
    const int maxAvailableQuantity = 50;//�̤j�i�֦��ƶq
    int currentBrick;//�ثe�֦��j��
    Transform brickParent;//�j��������
    readonly Vector3 initialBrickPosition = new Vector3(0, 1f, -0.3f);////�j���\���m    
    float dorpBrickCountdown;//��U�j��(�p�ɾ�)    

    private void Awake()
    {
        //Component
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        //�V�q
        objectForward = transform.forward;

        brickParent = FindChildMethod.OnFindChild<Transform>(transform, "Bricks");//�j���\���m

        CameraControl.Instance.SetTargetObject = transform;//�]�w��v�����H����
    }

    private void Update()
    {
        OnMoveControl();//���ʱ���        
        OnCollsion();//�I��        
        OnExitGame();//���}�C��
    }

    /// <summary>
    /// ���}�C��
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
    /// ���ʱ���
    /// </summary>
    void OnMoveControl()
    {
        //��L��J��
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");            
        OnMouseInput();//�ƹ���J

        //�`��J��
        float inputValue = Mathf.Clamp(Mathf.Abs(horizontal) + Mathf.Abs(vertical), 0, 1);
        
        //��V
        Vector3 v3_horizontal = Vector3.Cross(Vector3.up, objectForward * horizontal);
        Vector3 v3_vertical = Vector3.Cross(Vector3.right, Vector3.up * vertical);
        transform.forward = Vector3.RotateTowards(transform.forward, v3_horizontal + v3_vertical, 1, 0);

        //����
        transform.position = transform.position + transform.forward * inputValue * moveSpeed * Time.deltaTime;      

        animator.SetFloat("Run", inputValue);
    }

    /// <summary>
    /// �ƹ���J
    /// </summary>
    void OnMouseInput()
    {        
        //�����ƹ��I����m
        if (Input.GetMouseButtonDown(0)) mouseDownPosition = Input.mousePosition;

        //�ƹ��즲
        if (Input.GetMouseButton(0))
        {
            horizontal = Input.mousePosition.x - mouseDownPosition.x;
            vertical = Input.mousePosition.y - mouseDownPosition.y;
        }   
    }

    /// <summary>
    /// �I��
    /// </summary>
    void OnCollsion()
    {
        Collider[] obj_buildArea = OnCollisionArea("BuildArea");//�سy�ϰ�I��
        Collider[] obj_brickArea = OnCollisionArea("BrickArea");//�j���ϰ�I��        

        OnAreaBehavior(colliders: obj_buildArea, action: OnDropBrick);//�سy�ϰ�I��
        OnAreaBehavior(colliders: obj_brickArea, action: OnTakeBruck);//�j���ϰ�I��
    }

    /// <summary>
    /// �I���ϰ�
    /// </summary>
    /// <param name="layer">�I��Layer</param>
    Collider[] OnCollisionArea(string layer)
    {
        return Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size, Quaternion.Euler(transform.localEulerAngles), 1 << LayerMask.NameToLayer(layer));
    }

    /// <summary>
    /// �I���ϰ�欰
    /// </summary>
    /// <param name="colliders">�I���ϰ�</param>
    /// <param name="action">����[��</param>
    void OnAreaBehavior(Collider[] colliders, Action<Transform> action)
    {
        if(colliders.Length > 0)
        {
            dorpBrickCountdown -= Time.deltaTime;//��U�j��(�p�ɾ�)
            if (dorpBrickCountdown <= 0)
            {
                dorpBrickCountdown = moveBrickSpeed;//���s��U�j��(�p�ɾ�)
                action.Invoke(colliders[0].transform);
            }
        }
    }

    /// <summary>
    /// ��U�j��
    /// </summary>
    /// <param name="area">�سy�ϰ�</param>
    void OnDropBrick(Transform area)
    {
        Transform obj_bricks = FindChildMethod.OnFindChild<Transform>(transform, "Bricks");
        if (obj_bricks.childCount > 0)
        {
            area.GetComponent<BuildArea>().OnBuildPosition(obj_bricks.GetChild(obj_bricks.childCount - 1));
            currentBrick--;//�ثe�֦��j���ƶq
        }
    }

    /// <summary>
    /// ���_�j��
    /// </summary>
    /// <param name="area"></param>
    void OnTakeBruck(Transform area)
    {
        if (currentBrick < maxAvailableQuantity)//�ثe�֦��j���ƶq
        {
            Transform brick = area.GetComponent<BrickArea>().OnGetBrick();//����j��
            if (brick == null) return;
            brick.SetParent(brickParent);//�]�wParent
            Vector3 brickSize = brick.GetComponent<MeshFilter>().mesh.bounds.size;//�j��Size
            brick.transform.localPosition = new Vector3(initialBrickPosition.x, initialBrickPosition.y + (brickSize.y / 3 * currentBrick), initialBrickPosition.z);//��m
            brick.localRotation = Quaternion.Euler(Vector3.zero);//����

            currentBrick++;//�ثe�֦��j���ƶq
        }
    }
}
