using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����
/// </summary>
public class ObjectPool
{
    static ObjectPool objectPool;
    public static ObjectPool Instance => objectPool;

    //����
    List<List<TemporaryObject>> objectPoolObjecys = new List<List<TemporaryObject>>();//���������
    List<GameObject> temporaryRecordObject = new List<GameObject>();//�{�ɬ�������

    /// <summary>
    /// �غc�l
    /// </summary>
    public ObjectPool()
    {
        objectPool = this;
    }

    /// <summary>
    /// �ЫػP��������
    /// </summary>
    /// <param name="obj">����</param>
    public int OnCreateAndRecordOnject(GameObject obj)
    {
        //�Ыت���
        TemporaryObject temporaryObject = new TemporaryObject();
        temporaryObject.obj = GameObject.Instantiate(obj);
        temporaryObject.obj.SetActive(false);

        //����
        temporaryRecordObject.Add(temporaryObject.obj);
        List<TemporaryObject> temporary = new List<TemporaryObject>();
        temporary.Add(temporaryObject);
        objectPoolObjecys.Add(temporary);

        return objectPoolObjecys.Count - 1;//�^�Ǫ���s��
    }

    /// <summary>
    /// �E������
    /// </summary>
    /// <param name="number">����s��</param>
    /// <returns></returns>
    public GameObject OnActiveObject(int number)
    {
        //���b
        if (number < 0 || number > objectPoolObjecys.Count)
        {
            Debug.LogError("�s�����~!");
            return null;
        }

        List<TemporaryObject> temporary = objectPoolObjecys[number];//���X����List

        //�E������
        for (int i = 0; i < temporary.Count; i++)
        {
            if (!temporary[i].obj.activeSelf)
            {
                temporary[i].obj.SetActive(true);
                return temporary[i].obj;
            }
        }

        //�W�L�ƶq�ƻs����
        TemporaryObject copy = new TemporaryObject();
        copy.obj = GameObject.Instantiate(temporaryRecordObject[number]);
        copy.obj.SetActive(true);
        objectPoolObjecys[number].Add(copy);
        return copy.obj;
    }
}

/// <summary>
/// �Ȧs����Class
/// </summary>
class TemporaryObject
{
    public GameObject obj;
}