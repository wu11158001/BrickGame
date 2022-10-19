using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 遊戲管理中心
/// </summary>
public class GameManagement : MonoBehaviour
{
    //Component
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;
    Canvas canvas;

    //AssetBundle
    AssetBundle ab_Player;//玩家
    AssetBundle ab_Build;//建造區域
    AssetBundle ab_Brick;//磚塊
    AssetBundle ab_Score_Text;//分數文字

    //物件
    GameObject playerObject;//玩家物件
    GameObject buildObject;//建造區域物件
    GameObject brickAreaObject;//磚塊區域
    public GameObject brickObject;//磚塊物件
    GameObject buildCount_TextObject;//建造數量文字物件
    public Text score_Text;//分數文字物件

    //建造區域
    readonly Vector3[] buildPosistion = {new Vector3(0, 0.1f, 8) };//建造區域位置

    //磚塊區域
    readonly Vector3 brickAreaPosition = new Vector3(15, 0.1f, 8);//磚塊區域位置

    //分數
    public int currentScore;//目前分數

    private void Awake()
    {
        if(gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        score_Text = GameObject.Find("Score_Text").GetComponent<Text>();//建造數量文字物件
        score_Text.text = $"分數: {currentScore}";
    }

    private void Start()
    {
        StartCoroutine(OnLoadAssets());//資源載入
    }

    /// <summary>
    /// 資源載入
    /// </summary>
    /// <returns></returns>
    IEnumerator OnLoadAssets()
    {
        string path = Application.streamingAssetsPath + "/MyassetBundle";

        //玩家物件
        AssetBundleCreateRequest request_Player = AssetBundle.LoadFromFileAsync(path + "/prefab/player");
        yield return request_Player;
        ab_Player = request_Player.assetBundle;
        AssetBundleRequest abr_Player = ab_Player.LoadAssetAsync<GameObject>("StickMan");
        yield return abr_Player;
        playerObject = abr_Player.asset as GameObject;
        ab_Player.Unload(false);

        //建造區域物件
        AssetBundleCreateRequest request_Build = AssetBundle.LoadFromFileAsync(path + "/prefab/build");
        yield return request_Build;
        ab_Build = request_Build.assetBundle;
        AssetBundleRequest abr_Build = ab_Build.LoadAssetAsync<GameObject>("Build");
        yield return abr_Build;
        buildObject = abr_Build.asset as GameObject;

        //磚塊區域物件
        AssetBundleRequest abr_BrickArea = abr_Build = ab_Build.LoadAssetAsync<GameObject>("BrickArea");
        yield return abr_Build;
        brickAreaObject = abr_Build.asset as GameObject;
        ab_Build.Unload(false);

        //磚塊物件
        AssetBundleCreateRequest request_Brick = AssetBundle.LoadFromFileAsync(path + "/prefab/brick");
        yield return request_Brick;
        ab_Brick = request_Brick.assetBundle;
        AssetBundleRequest abr_Brick = ab_Brick.LoadAssetAsync<GameObject>("Brick");
        yield return abr_Brick;
        brickObject = abr_Brick.asset as GameObject;        
        ab_Brick.Unload(false);

        //分數文字物件
        AssetBundleCreateRequest request_Score_Text = AssetBundle.LoadFromFileAsync(path + "/prefab/buildcount_text");
        yield return request_Score_Text;
        ab_Score_Text = request_Score_Text.assetBundle;
        AssetBundleRequest abr_Score_Text = ab_Score_Text.LoadAssetAsync<GameObject>("BuildCount_Text");
        yield return abr_Score_Text;
        buildCount_TextObject = abr_Score_Text.asset as GameObject;
        ab_Score_Text.Unload(false);

        OnCreateInitialObject();//創建初始物件

        yield return default;
    }

    /// <summary>
    /// 創建初始物件
    /// </summary>
    void OnCreateInitialObject()
    {
        //創建玩家物件
        GameObject obj_Player = Instantiate(playerObject, Vector3.zero, Quaternion.identity);
        if (!obj_Player.TryGetComponent<PlayerControl>(out PlayerControl playerControl)) obj_Player.AddComponent<PlayerControl>();//加入玩家控制腳本    

        OnCreateBuildArea();//創建建造區域物件             
        OnCreateBrickArea();//創建磚塊區域物件
        //OnCreateBrick();//創建磚塊
    }

    /// <summary>
    /// 創建建造區域物件
    /// </summary>
    public void OnCreateBuildArea()
    {
        for (int i = 0; i < buildPosistion.Length; i++)
        {
            //創建建造區域物件
            GameObject obj_build = Instantiate(buildObject, buildPosistion[i], Quaternion.identity);
            obj_build.layer = LayerMask.NameToLayer("BuildArea");//物件Layer
            if (!obj_build.TryGetComponent<BuildArea>(out BuildArea buildArea)) buildArea = obj_build.AddComponent<BuildArea>();//加入玩家控制腳本

            //建造數量文字物件
            GameObject obj_score_Text = Instantiate(buildCount_TextObject);
            obj_score_Text.transform.SetParent(canvas.transform);
            if (!obj_score_Text.TryGetComponent<BuildCountText>(out BuildCountText scoreText)) scoreText = obj_score_Text.AddComponent<BuildCountText>();
            scoreText.SetTarget = obj_build.transform;//設定建造數量文字目標
            buildArea.SetScoreObject = scoreText.GetComponent<BuildCountText>();//設定建造區域建造數量文字
        }
    }

    /// <summary>
    /// 創建磚塊區域物件
    /// </summary>
    public void OnCreateBrickArea()
    {
        GameObject obj_brickArea = Instantiate(brickAreaObject, brickAreaPosition, Quaternion.identity);
        obj_brickArea.layer = LayerMask.NameToLayer("BrickArea");//物件Layer
        if (!obj_brickArea.TryGetComponent<BrickArea>(out BrickArea brickArea)) obj_brickArea.AddComponent<BrickArea>();//加入磚塊區域腳本        
    }    
}
