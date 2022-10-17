using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �M��l����
/// </summary>
public static class FindChildMethod
{
    /// <summary>
    /// �M��l����
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="serchObj">�j�M����</param>
    /// <param name="serchName">�j�M�W��</param>
    /// <returns></returns>
    public static T OnFindChild<T>(this Transform serchObj, string serchName) where T: Component
    {
        for (int i = 0; i < serchObj.childCount; i++)
        {
            //�l����U�٦��l����
            if(serchObj.GetChild(i).childCount > 0)
            {
                var obj = serchObj.GetChild(i).OnFindChild<T>(serchName);
                if (obj != null) return obj.GetComponent<T>();
            }

            //��쪫��
            if(serchObj.GetChild(i).name == serchName)
            {
                return serchObj.GetChild(i).GetComponent<T>();
            }
        }

        return default;
    }
}
