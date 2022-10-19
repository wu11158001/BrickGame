using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �C���޲z����
/// </summary>
public class GameManagement : MonoBehaviour
{
    //Component
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;
    Canvas canvas;

    //AssetBundle
    AssetBundle ab_Player;//���a
    AssetBundle ab_Build;//�سy�ϰ�
    AssetBundle ab_Brick;//�j��
    AssetBundle ab_Score_Text;//���Ƥ�r

    //����
    GameObject playerObject;//���a����
    GameObject buildObject;//�سy�ϰ쪫��
    GameObject brickAreaObject;//�j���ϰ�
    public GameObject brickObject;//�j������
    GameObject buildCount_TextObject;//�سy�ƶq��r����
    public Text score_Text;//���Ƥ�r����

    //�سy�ϰ�
    readonly Vector3[] buildPosistion = {new Vector3(0, 0.1f, 8) };//�سy�ϰ��m

    //�j���ϰ�
    readonly Vector3 brickAreaPosition = new Vector3(15, 0.1f, 8);//�j���ϰ��m

    //����
    public int currentScore;//�ثe����

    private void Awake()
    {
        if(gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        score_Text = GameObject.Find("Score_Text").GetComponent<Text>();//�سy�ƶq��r����
        score_Text.text = $"����: {currentScore}";
    }

    private void Start()
    {
        StartCoroutine(OnLoadAssets());//�귽���J
    }

    /// <summary>
    /// �귽���J
    /// </summary>
    /// <returns></returns>
    IEnumerator OnLoadAssets()
    {
        string path = Application.streamingAssetsPath + "/MyassetBundle";

        //���a����
        AssetBundleCreateRequest request_Player = AssetBundle.LoadFromFileAsync(path + "/prefab/player");
        yield return request_Player;
        ab_Player = request_Player.assetBundle;
        AssetBundleRequest abr_Player = ab_Player.LoadAssetAsync<GameObject>("StickMan");
        yield return abr_Player;
        playerObject = abr_Player.asset as GameObject;
        ab_Player.Unload(false);

        //�سy�ϰ쪫��
        AssetBundleCreateRequest request_Build = AssetBundle.LoadFromFileAsync(path + "/prefab/build");
        yield return request_Build;
        ab_Build = request_Build.assetBundle;
        AssetBundleRequest abr_Build = ab_Build.LoadAssetAsync<GameObject>("Build");
        yield return abr_Build;
        buildObject = abr_Build.asset as GameObject;

        //�j���ϰ쪫��
        AssetBundleRequest abr_BrickArea = abr_Build = ab_Build.LoadAssetAsync<GameObject>("BrickArea");
        yield return abr_Build;
        brickAreaObject = abr_Build.asset as GameObject;
        ab_Build.Unload(false);

        //�j������
        AssetBundleCreateRequest request_Brick = AssetBundle.LoadFromFileAsync(path + "/prefab/brick");
        yield return request_Brick;
        ab_Brick = request_Brick.assetBundle;
        AssetBundleRequest abr_Brick = ab_Brick.LoadAssetAsync<GameObject>("Brick");
        yield return abr_Brick;
        brickObject = abr_Brick.asset as GameObject;        
        ab_Brick.Unload(false);

        //���Ƥ�r����
        AssetBundleCreateRequest request_Score_Text = AssetBundle.LoadFromFileAsync(path + "/prefab/buildcount_text");
        yield return request_Score_Text;
        ab_Score_Text = request_Score_Text.assetBundle;
        AssetBundleRequest abr_Score_Text = ab_Score_Text.LoadAssetAsync<GameObject>("BuildCount_Text");
        yield return abr_Score_Text;
        buildCount_TextObject = abr_Score_Text.asset as GameObject;
        ab_Score_Text.Unload(false);

        OnCreateInitialObject();//�Ыت�l����

        yield return default;
    }

    /// <summary>
    /// �Ыت�l����
    /// </summary>
    void OnCreateInitialObject()
    {
        //�Ыت��a����
        GameObject obj_Player = Instantiate(playerObject, Vector3.zero, Quaternion.identity);
        if (!obj_Player.TryGetComponent<PlayerControl>(out PlayerControl playerControl)) obj_Player.AddComponent<PlayerControl>();//�[�J���a����}��    

        OnCreateBuildArea();//�Ыثسy�ϰ쪫��             
        OnCreateBrickArea();//�Ыؿj���ϰ쪫��
        //OnCreateBrick();//�Ыؿj��
    }

    /// <summary>
    /// �Ыثسy�ϰ쪫��
    /// </summary>
    public void OnCreateBuildArea()
    {
        for (int i = 0; i < buildPosistion.Length; i++)
        {
            //�Ыثسy�ϰ쪫��
            GameObject obj_build = Instantiate(buildObject, buildPosistion[i], Quaternion.identity);
            obj_build.layer = LayerMask.NameToLayer("BuildArea");//����Layer
            if (!obj_build.TryGetComponent<BuildArea>(out BuildArea buildArea)) buildArea = obj_build.AddComponent<BuildArea>();//�[�J���a����}��

            //�سy�ƶq��r����
            GameObject obj_score_Text = Instantiate(buildCount_TextObject);
            obj_score_Text.transform.SetParent(canvas.transform);
            if (!obj_score_Text.TryGetComponent<BuildCountText>(out BuildCountText scoreText)) scoreText = obj_score_Text.AddComponent<BuildCountText>();
            scoreText.SetTarget = obj_build.transform;//�]�w�سy�ƶq��r�ؼ�
            buildArea.SetScoreObject = scoreText.GetComponent<BuildCountText>();//�]�w�سy�ϰ�سy�ƶq��r
        }
    }

    /// <summary>
    /// �Ыؿj���ϰ쪫��
    /// </summary>
    public void OnCreateBrickArea()
    {
        GameObject obj_brickArea = Instantiate(brickAreaObject, brickAreaPosition, Quaternion.identity);
        obj_brickArea.layer = LayerMask.NameToLayer("BrickArea");//����Layer
        if (!obj_brickArea.TryGetComponent<BrickArea>(out BrickArea brickArea)) obj_brickArea.AddComponent<BrickArea>();//�[�J�j���ϰ�}��        
    }    
}
