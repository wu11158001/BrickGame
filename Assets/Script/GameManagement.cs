using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 笴栏恨瞶いみ
/// </summary>
public class GameManagement : MonoBehaviour
{
    //Component
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;
    ObjectPool objectPool = new ObjectPool();//ン
    Canvas canvas;

    //AssetBundle
    AssetBundle ab_Player;//產
    AssetBundle ab_Build;//硑跋办
    AssetBundle ab_Brick;//縥遏
    AssetBundle ab_Score_Text;//だ计ゅ

    //ン
    GameObject playerObject;//產ン
    GameObject buildObject;//硑跋办ン
    GameObject brickAreaObject;//縥遏跋办
    public GameObject brickObject;//縥遏ン
    GameObject buildCount_TextObject;//硑计秖ゅン
    public Text score_Text;//だ计ゅン

    //硑跋办
    readonly Vector3[] buildPosistion = {new Vector3(0, 0.1f, 8) };//硑跋办竚

    //縥遏跋办
    readonly Vector3 brickAreaPosition = new Vector3(15, 0.1f, 8);//縥遏跋办竚

    //だ计
    public int currentScore;//ヘ玡だ计

    [Header("魁")]
    Dictionary<string, int> objectsNumber = new Dictionary<string, int>();//ン絪腹

    private void Awake()
    {
        if(gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        score_Text = GameObject.Find("Score_Text").GetComponent<Text>();//硑计秖ゅン
        score_Text.text = $"だ计: {currentScore}";
    }

    private void Start()
    {
        StartCoroutine(OnLoadAssets());//戈方更
    }

    /// <summary>
    /// 戈方更
    /// </summary>
    /// <returns></returns>
    IEnumerator OnLoadAssets()
    {
        string path = Application.streamingAssetsPath + "/MyassetBundle";

        //產ン
        AssetBundleCreateRequest request_Player = AssetBundle.LoadFromFileAsync(path + "/prefab/player");
        yield return request_Player;
        ab_Player = request_Player.assetBundle;
        AssetBundleRequest abr_Player = ab_Player.LoadAssetAsync<GameObject>("StickMan");
        yield return abr_Player;
        playerObject = abr_Player.asset as GameObject;
        ab_Player.Unload(false);

        //硑跋办ン
        AssetBundleCreateRequest request_Build = AssetBundle.LoadFromFileAsync(path + "/prefab/build");
        yield return request_Build;
        ab_Build = request_Build.assetBundle;
        AssetBundleRequest abr_Build = ab_Build.LoadAssetAsync<GameObject>("Build");
        yield return abr_Build;
        buildObject = abr_Build.asset as GameObject;

        //縥遏跋办ン
        AssetBundleRequest abr_BrickArea = abr_Build = ab_Build.LoadAssetAsync<GameObject>("BrickArea");
        yield return abr_Build;
        brickAreaObject = abr_Build.asset as GameObject;
        ab_Build.Unload(false);

        //縥遏ン
        AssetBundleCreateRequest request_Brick = AssetBundle.LoadFromFileAsync(path + "/prefab/brick");
        yield return request_Brick;
        ab_Brick = request_Brick.assetBundle;
        AssetBundleRequest abr_Brick = ab_Brick.LoadAssetAsync<GameObject>("Brick");
        yield return abr_Brick;
        brickObject = abr_Brick.asset as GameObject;        
        ab_Brick.Unload(false);

        //だ计ゅン
        AssetBundleCreateRequest request_Score_Text = AssetBundle.LoadFromFileAsync(path + "/prefab/buildcount_text");
        yield return request_Score_Text;
        ab_Score_Text = request_Score_Text.assetBundle;
        AssetBundleRequest abr_Score_Text = ab_Score_Text.LoadAssetAsync<GameObject>("BuildCount_Text");
        yield return abr_Score_Text;
        buildCount_TextObject = abr_Score_Text.asset as GameObject;
        ab_Score_Text.Unload(false);

        OnCreateInitialObject();//承﹍ン

        yield return default;
    }

    /// <summary>
    /// 承﹍ン
    /// </summary>
    void OnCreateInitialObject()
    {
        //承產ン
        GameObject obj_Player = Instantiate(playerObject, Vector3.zero, Quaternion.identity);
        if (!obj_Player.TryGetComponent<PlayerControl>(out PlayerControl playerControl)) obj_Player.AddComponent<PlayerControl>();//產北竲セ    

        OnObjectPool();//ン
        OnCreateBuildArea();//承硑跋办ン             
        OnCreateBrickArea();//承縥遏跋办ン        
    }

    /// <summary>
    /// ン
    /// </summary>
    void OnObjectPool()
    {
        //承ンン
        objectPool = ObjectPool.Instance;//ン龟ㄒて

        int number = 0;//絪腹

        //縥遏
        number = objectPool.OnCreateAndRecordOnject(brickObject);
        objectsNumber.Add("Brick", number);
    }

    /// <summary>
    /// 莉ン絪腹
    /// </summary>
    /// <param name="objName"></param>
    int OnGetObjectNumber(string objName)
    {
        int number = -1;

        foreach (var obj in objectsNumber)
        {
            if (obj.Key == objName)
            {
                number = obj.Value;
            }
        }

        return number;
    }

    /// <summary>
    /// 莉ンン
    /// </summary>
    /// <param name="serchName">碝тン嘿</param>
    /// <returns></returns>
    public GameObject OnGetObjectPool(string serchName)
    {
        GameObject obj = objectPool.OnActiveObject(OnGetObjectNumber(serchName));//縀ン

        return obj;
    }

    /// <summary>
    /// 承硑跋办ン
    /// </summary>
    public void OnCreateBuildArea()
    {
        for (int i = 0; i < buildPosistion.Length; i++)
        {
            //承硑跋办ン
            GameObject obj_build = Instantiate(buildObject, buildPosistion[i], Quaternion.identity);
            obj_build.layer = LayerMask.NameToLayer("BuildArea");//ンLayer
            if (!obj_build.TryGetComponent<BuildArea>(out BuildArea buildArea)) buildArea = obj_build.AddComponent<BuildArea>();//產北竲セ

            //硑计秖ゅン
            GameObject obj_score_Text = Instantiate(buildCount_TextObject);
            obj_score_Text.transform.SetParent(canvas.transform);
            if (!obj_score_Text.TryGetComponent<BuildCountText>(out BuildCountText scoreText)) scoreText = obj_score_Text.AddComponent<BuildCountText>();
            scoreText.SetTarget = obj_build.transform;//砞﹚硑计秖ゅヘ夹
            buildArea.SetBuildCountTextObject = scoreText.GetComponent<BuildCountText>();//砞﹚硑跋办硑计秖ゅ
        }
    }

    /// <summary>
    /// 承縥遏跋办ン
    /// </summary>
    public void OnCreateBrickArea()
    {
        GameObject obj_brickArea = Instantiate(brickAreaObject, brickAreaPosition, Quaternion.identity);
        obj_brickArea.layer = LayerMask.NameToLayer("BrickArea");//ンLayer
        if (!obj_brickArea.TryGetComponent<BrickArea>(out BrickArea brickArea)) obj_brickArea.AddComponent<BrickArea>();//縥遏跋办竲セ        
    }    
}
