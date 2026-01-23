using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    //对外提供单例的访问方式
    public CameraManager cameraManager;
    [HideInInspector]
    public PlayerManager player;
    [HideInInspector]
    public List<GameObject> enemys;

    //内部使用
    [SerializeField]
    private GameObject plaeyrPrefab;
    [SerializeField]
    private Transform playerBornPos;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform enemyBornPos;
    [SerializeField]
    private Transform enemyBornPos2;
    [SerializeField]
    private GameObject hitEffPrefab;
    private Coroutine timeStopCoroutine;
    private int maxEnemyCount = 16;

    private void Awake()
    {
        _instance = this;

        // 1. 关闭垂直同步 (必须！否则锁帧无效)
        QualitySettings.vSyncCount = 0;
        // 2. 设置目标帧率 (比如 60 或 -1) -1表示不限制帧率
        Application.targetFrameRate = 60;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitPlayer();

        StartCoroutine(GenrateEnemy());

        cameraManager.Init(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitPlayer()
    {
        player = Instantiate(plaeyrPrefab).GetComponent<PlayerManager>();
        player.transform.position = playerBornPos.position;
    }

    IEnumerator GenrateEnemy()
    {
        for (int i = 0; i < 8; i++)
        {
            InitEnemy(enemyBornPos);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < 8; i++)
        {
            InitEnemy(enemyBornPos2);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void InitEnemy(Transform enemyBornPos)
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemys.Add(enemy);
        enemy.transform.position = enemyBornPos.position;
    }

    public void RemoveEnemy()
    {
        maxEnemyCount--;
        if(maxEnemyCount <= 0)
        {
            Invoke("GameRestart", 5f * Time.timeScale);
            player.PlayWinAnimation();
        }
    }

    public void ShowHitEffect(Vector3 pos)
    {
        //TODO 显示命中效果
        GameObject hitEff = Instantiate(hitEffPrefab);
        hitEff.transform.position = pos;
        Destroy(hitEff, 0.2f);
    }

    public void TimeStop(float time)
    {
        Time.timeScale = 0.0f;
        if(timeStopCoroutine != null) StopCoroutine(timeStopCoroutine);

        timeStopCoroutine = StartCoroutine(ReStartTime(time));
    }

    IEnumerator ReStartTime(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1.0f;
    }

    public void SetTargetFrameRate(int targetFrameRate)
    {
        Application.targetFrameRate = targetFrameRate;
    }

    public void SetTimeScale(float targetScale)
    {
        Time.timeScale = targetScale;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }
}
