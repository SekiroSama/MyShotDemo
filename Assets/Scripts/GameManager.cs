using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public List<GameObject> enemys;

    [SerializeField]
    private GameObject plaeyrPrefab;
    public Transform playerBornPos;
    [SerializeField]
    private GameObject enemyPrefab;
    public Transform enemyBornPos;
    private void Awake()
    {
        _instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitPlayer();
        InitEnemy();
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
