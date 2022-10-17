using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 尋找子物件
/// </summary>
public static class FindChildMethod
{
    /// <summary>
    /// 尋找子物件
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="serchObj">搜尋物件</param>
    /// <param name="serchName">搜尋名稱</param>
    /// <returns></returns>
    public static T OnFindChild<T>(this Transform serchObj, string serchName) where T: Component
    {
        for (int i = 0; i < serchObj.childCount; i++)
        {
            //子物件下還有子物件
            if(serchObj.GetChild(i).childCount > 0)
            {
                var obj = serchObj.GetChild(i).OnFindChild<T>(serchName);
                if (obj != null) return obj.GetComponent<T>();
            }

            //找到物件
            if(serchObj.GetChild(i).name == serchName)
            {
                return serchObj.GetChild(i).GetComponent<T>();
            }
        }

        return default;
    }
}
