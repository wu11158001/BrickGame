using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    GameObject brickObject;//磚塊物件
    GameObject score_TextObject;//分數文字物件

    //建造區域
    readonly Vector3[] buildPosistion = {new Vector3(0, 0.1f, 8) };//建造區域位置
    const int brickCount = 50;//磚塊數量

    //磚塊
    Transform brickParent;//磚塊背放位置
    readonly Vector3 initialBrickPosition = new Vector3(0, 0.85f, -0.3f);//初始磚塊位置
    Vector3 brickSize;//磚塊物件Size

    /// <summary>
    /// 獲取磚塊物件Size
    /// </summary>
    public Vector3 GetBrickSize => brickSize;

    private void Awake()
    {
        if(gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
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
        AssetBundleCreateRequest request_Score_Text = AssetBundle.LoadFromFileAsync(path + "/prefab/score_text");
        yield return request_Score_Text;
        ab_Score_Text = request_Score_Text.assetBundle;
        AssetBundleRequest abr_Score_Text = ab_Score_Text.LoadAssetAsync<GameObject>("Score_Text");
        yield return abr_Score_Text;
        score_TextObject = abr_Score_Text.asset as GameObject;
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
        brickParent = FindChildMethod.OnFindChild<Transform>(obj_Player.transform, "Bricks");//尋找玩家Bricks子物件

        OnCreateBuildArea();//創建建造區域物件     
        OnCreateBrick();//創建磚塊
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
            obj_build.layer = LayerMask.NameToLayer("Build");//物件Layer
            if (!obj_build.TryGetComponent<BuildArea>(out BuildArea buildArea)) buildArea = obj_build.AddComponent<BuildArea>();//加入玩家控制腳本

            //創建分數文字物件
            GameObject obj_score_Text = Instantiate(score_TextObject);
            obj_score_Text.transform.SetParent(canvas.transform);
            if (!obj_score_Text.TryGetComponent<ScoreText>(out ScoreText scoreText)) scoreText = obj_score_Text.AddComponent<ScoreText>();
            scoreText.SetTarget = obj_build.transform;//設定分數文字目標
            buildArea.SetScoreObject = scoreText.GetComponent<ScoreText>();//設定建造區域分數物件
        }
    }

    /// <summary>
    /// 創建磚塊
    /// </summary>
    public void OnCreateBrick()
    {        
        for (int i = 0; i < brickCount; i++)
        {
            GameObject obj_Brick = Instantiate(brickObject);
            obj_Brick.layer = LayerMask.NameToLayer("Brick");//物件Layer
            if (brickParent != null) obj_Brick.transform.SetParent(brickParent);//設定Parent
            brickSize = brickSize == Vector3.zero ? obj_Brick.GetComponent<MeshFilter>().mesh.bounds.size : brickSize;//磚塊Size
            obj_Brick.transform.localPosition = new Vector3(initialBrickPosition.x, initialBrickPosition.y + (brickSize.y / 3 * i), initialBrickPosition.z);//初始位置

        }
    }
}
