using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance;

    //对外提供单例的访问方式
    public CameraManager cameraManager;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public List<GameObject> enemys;

    //内部使用
    [SerializeField]
    private GameObject plaeyrPrefab;
    public Transform playerBornPos;
    [SerializeField]
    private GameObject enemyPrefab;
    public Transform enemyBornPos;

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
        for (int i = 0; i < 4; i++)
        {
            Invoke("InitEnemy", i * 0.3f);
        }

        cameraManager.Init(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitPlayer()
    {
        player = Instantiate(plaeyrPrefab);
        player.transform.position = playerBornPos.position;
    }

    private void InitEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemys.Add(enemy);
        enemy.transform.position = enemyBornPos.position;
    }
}
