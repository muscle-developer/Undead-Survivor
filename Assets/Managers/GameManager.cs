using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 어디에서도 접근 가능 하도록 Static선언
    public static GameManager Instance;
    // 내가 플레이한 시간
    private float playTiem = 0f;
    public float PlayTiem { get => playTiem; }
    // 최대 플레이 할 수 있는 시간
    [SerializeField]
    private float maxPlayTime = 2 * 10f; 
    [SerializeField]
    private Transform managersObject;
    [SerializeField]
    public Player player;
    public PoolManager poolManager;

    public void Awake()
    {
        // 씬이 이동해도 Manager는 고유하게 살아있어야 하기때문에
        DontDestroyOnLoad(managersObject);
        // Static 으로 선언한 변수는 인스펙터에 나타나지 않음으로 Awake에서 초기화 시켜준다.
        GameManager.Instance = this;
    }

    void Update()
    {
        playTiem += Time.deltaTime;

        if(playTiem >= maxPlayTime)
            playTiem = maxPlayTime;
    }
}
