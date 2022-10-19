using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物件池
/// </summary>
public class ObjectPool
{
    static ObjectPool objectPool;
    public static ObjectPool Instance => objectPool;

    //紀錄
    List<List<TemporaryObject>> objectPoolObjecys = new List<List<TemporaryObject>>();//物件池物件
    List<GameObject> temporaryRecordObject = new List<GameObject>();//臨時紀錄物件

    /// <summary>
    /// 建構子
    /// </summary>
    public ObjectPool()
    {
        objectPool = this;
    }

    /// <summary>
    /// 創建與紀錄物件
    /// </summary>
    /// <param name="obj">物件</param>
    public int OnCreateAndRecordOnject(GameObject obj)
    {
        //創建物件
        TemporaryObject temporaryObject = new TemporaryObject();
        temporaryObject.obj = GameObject.Instantiate(obj);
        temporaryObject.obj.SetActive(false);

        //紀錄
        temporaryRecordObject.Add(temporaryObject.obj);
        List<TemporaryObject> temporary = new List<TemporaryObject>();
        temporary.Add(temporaryObject);
        objectPoolObjecys.Add(temporary);

        return objectPoolObjecys.Count - 1;//回傳物件編號
    }

    /// <summary>
    /// 激活物件
    /// </summary>
    /// <param name="number">物件編號</param>
    /// <returns></returns>
    public GameObject OnActiveObject(int number)
    {
        //防呆
        if (number < 0 || number > objectPoolObjecys.Count)
        {
            Debug.LogError("編號錯誤!");
            return null;
        }

        List<TemporaryObject> temporary = objectPoolObjecys[number];//取出物件List

        //激活物件
        for (int i = 0; i < temporary.Count; i++)
        {
            if (!temporary[i].obj.activeSelf)
            {
                temporary[i].obj.SetActive(true);
                return temporary[i].obj;
            }
        }

        //超過數量複製物件
        TemporaryObject copy = new TemporaryObject();
        copy.obj = GameObject.Instantiate(temporaryRecordObject[number]);
        copy.obj.SetActive(true);
        objectPoolObjecys[number].Add(copy);
        return copy.obj;
    }
}

/// <summary>
/// 暫存物件Class
/// </summary>
class TemporaryObject
{
    public GameObject obj;
}