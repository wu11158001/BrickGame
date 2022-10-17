using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    GameObject brickObject;//�j������
    GameObject score_TextObject;//���Ƥ�r����

    //�سy�ϰ�
    readonly Vector3[] buildPosistion = {new Vector3(0, 0.1f, 8) };//�سy�ϰ��m
    const int brickCount = 50;//�j���ƶq

    //�j��
    Transform brickParent;//�j���I���m
    readonly Vector3 initialBrickPosition = new Vector3(0, 0.85f, -0.3f);//��l�j����m
    Vector3 brickSize;//�j������Size

    /// <summary>
    /// ����j������Size
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
        AssetBundleCreateRequest request_Score_Text = AssetBundle.LoadFromFileAsync(path + "/prefab/score_text");
        yield return request_Score_Text;
        ab_Score_Text = request_Score_Text.assetBundle;
        AssetBundleRequest abr_Score_Text = ab_Score_Text.LoadAssetAsync<GameObject>("Score_Text");
        yield return abr_Score_Text;
        score_TextObject = abr_Score_Text.asset as GameObject;
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
        brickParent = FindChildMethod.OnFindChild<Transform>(obj_Player.transform, "Bricks");//�M�䪱�aBricks�l����

        OnCreateBuildArea();//�Ыثسy�ϰ쪫��     
        OnCreateBrick();//�Ыؿj��
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
            obj_build.layer = LayerMask.NameToLayer("Build");//����Layer
            if (!obj_build.TryGetComponent<BuildArea>(out BuildArea buildArea)) buildArea = obj_build.AddComponent<BuildArea>();//�[�J���a����}��

            //�Ыؤ��Ƥ�r����
            GameObject obj_score_Text = Instantiate(score_TextObject);
            obj_score_Text.transform.SetParent(canvas.transform);
            if (!obj_score_Text.TryGetComponent<ScoreText>(out ScoreText scoreText)) scoreText = obj_score_Text.AddComponent<ScoreText>();
            scoreText.SetTarget = obj_build.transform;//�]�w���Ƥ�r�ؼ�
            buildArea.SetScoreObject = scoreText.GetComponent<ScoreText>();//�]�w�سy�ϰ���ƪ���
        }
    }

    /// <summary>
    /// �Ыؿj��
    /// </summary>
    public void OnCreateBrick()
    {        
        for (int i = 0; i < brickCount; i++)
        {
            GameObject obj_Brick = Instantiate(brickObject);
            obj_Brick.layer = LayerMask.NameToLayer("Brick");//����Layer
            if (brickParent != null) obj_Brick.transform.SetParent(brickParent);//�]�wParent
            brickSize = brickSize == Vector3.zero ? obj_Brick.GetComponent<MeshFilter>().mesh.bounds.size : brickSize;//�j��Size
            obj_Brick.transform.localPosition = new Vector3(initialBrickPosition.x, initialBrickPosition.y + (brickSize.y / 3 * i), initialBrickPosition.z);//��l��m

        }
    }
}
