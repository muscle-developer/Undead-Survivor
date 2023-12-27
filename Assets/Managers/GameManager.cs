using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

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
    // 캐릭터를 고르기 위한 아이디
    public int playerId;
    // 몬스터 처치 시 얻는 데이터(레벨, 킬수, 경험치, 체력)
    public float hp;
    public float maxHP = 100;
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
    public GameResultPopup uiResultPopup;
    public GameObject enemyCleaner;

    public void Awake()
    {
        // 씬이 이동해도 Manager는 고유하게 살아있어야 하기때문에
        DontDestroyOnLoad(managersObject);
        // Static 으로 선언한 변수는 인스펙터에 나타나지 않음으로 Awake에서 초기화 시켜준다.
        GameManager.Instance = this;
    }

    public void GameStart(int id)
    {
        playerId = id; 
        // 시작시 MaxHP로 초기화
        hp = maxHP;
        player.gameObject.SetActive(true);
        uiLevelUp.BaseWeapon(playerId);
        GameResume();
    }

    public void GameOver()  
    {
        StartCoroutine(GameOverCoroutine());
    }

    // 게임오버 시 딜레이를 주기 위한 코루틴 함수
    private IEnumerator GameOverCoroutine()
    {
        // 기본값 세팅(플레이 초기 설정값)
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResultPopup.gameObject.SetActive(true);
        uiResultPopup.GameLose();
        GameStop();
    } 

    public void GameVictory()  
    {
        StartCoroutine(GameVictoryCoroutine());
    }

    // 게임승리 시 딜레이를 주기 위한 코루틴 함수
    private IEnumerator GameVictoryCoroutine()
    {
        // 기본값 세팅(플레이 초기 설정값)
        isLive = false;
        enemyCleaner.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResultPopup.gameObject.SetActive(true);
        uiResultPopup.GameVictory();
        GameStop();
    } 

    void Update()
    {
        if(!isLive)
            return;

        playTiem += Time.deltaTime;

        if(playTiem >= maxPlayTime)
        {
            playTiem = maxPlayTime;
            GameVictory();
        }
    }

    // 경험치 증가 함수
    public void GetExp()
    {   
        // 게임에서 이겼을 때 EnemyCleaner 오브젝트가 활성화 되면서 경험치를 얻는것을 방지하기 위한처리
        if(!isLive)
            return;

        exp ++;
        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level ++;
            exp = 0;
            uiLevelUp.ShowLevelUpPopup();
        }
    }   

    // 시간 일시정지, 재시작 함수
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

    // 플레이어가 죽었을 때 초기 화면으로 설정하는 함수
    public void GameReTry()
    {
        SceneManager.LoadScene("MainScene");
    }
}
