using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
    // 어디에서도 접근 가능 하도록 Static선언
    public static GameManager Instance;
    [Header("Game Control")]
    public bool isLive = false;
    [SerializeField]
    // 내가 플레이한 시간
    private float playTiem = 0f;
    public float PlayTiem { get => playTiem; }
    // 최대 플레이 할 수 있는 시간
    public float maxPlayTime = 2 * 10f; 

    [Header("Player Info")]
    // 몬스터 처치 시 얻는 데이터(레벨, 킬수, 경험치, 체력)
    public int hp;
    public int maxHP = 100;
    public int level;
    public int kill;
    public int exp;
    // 각 레벨의 필요경험치를 보관할 변수
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600};

    [Header("Game Object")]
    [SerializeField]
    private Transform managersObject;
    [SerializeField]
    public Player player;
    public PoolManager poolManager;
    public UILevelUp uiLevelUp;

    public void Awake()
    {
        // 씬이 이동해도 Manager는 고유하게 살아있어야 하기때문에
        DontDestroyOnLoad(managersObject);
        // Static 으로 선언한 변수는 인스펙터에 나타나지 않음으로 Awake에서 초기화 시켜준다.
        GameManager.Instance = this;
    }

    void Start()
    {
        // 시작시 MaxHP로 초기화
        hp = maxHP;
        uiLevelUp.BaseWeapon(1);
    }

    void Update()
    {
        if(!isLive)
            return;

        playTiem += Time.deltaTime;

        if(playTiem >= maxPlayTime)
            playTiem = maxPlayTime;
    }

    // 경험치 증가 함수
    public void GetExp()
    {
        exp ++;
        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level ++;
            exp = 0;
            uiLevelUp.ShowLevelUpPopup();
        }
    }   

    // 일시정지, 재시작 함수
    public void GameResume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void GameStop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
}
