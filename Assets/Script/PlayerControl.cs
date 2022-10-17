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
    const float dorpBrickSpeed = 0.1f;//��U�j���t��
    float dorpBrickCountdown;//��U�j��(�p�ɾ�)

    private void Awake()
    {
        //Component
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        //�V�q
        objectForward = transform.forward;

        CameraControl.Instance.SetTargetObject = transform;//�]�w��v�����H����
    }

    private void Update()
    {
        OnMoveControl();//���ʱ���        
        OnCollisionOfBuildArea();//�سy�ϰ�I��
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
    /// �سy�ϰ�I��
    /// </summary>
    void OnCollisionOfBuildArea()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size, Quaternion.Euler(transform.localEulerAngles), 1 << LayerMask.NameToLayer("Build"));

        foreach (var area in colliders)
        {
            dorpBrickCountdown -= Time.deltaTime;//��U�j��(�p�ɾ�)
            if(dorpBrickCountdown <= 0)
            {
                dorpBrickCountdown = dorpBrickSpeed;//���s��U�j��(�p�ɾ�)
                DnDropBrick(area.transform);//��U�j��
            }
        }
    }

    /// <summary>
    /// ��U�j��
    /// </summary>
    /// <param name="area">�سy�ϰ�</param>
    void DnDropBrick(Transform area)
    {
        Transform obj_bricks = FindChildMethod.OnFindChild<Transform>(transform, "Bricks");
        if (obj_bricks.childCount > 0)
        {
            area.GetComponent<BuildArea>().OnBuildPosition(obj_bricks.GetChild(obj_bricks.childCount - 1));            
        }
    }
}
