using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 어디에서도 접근 가능 하도록 Static선언
    public static GameManager Instance;
    [SerializeField]
    private Transform managersObject;
    [SerializeField]
    public Player player;

    public void Awake()
    {
        // 씬이 이동해도 Manager는 고유하게 살아있어야 하기때문에
        DontDestroyOnLoad(managersObject);
        // Static 으로 선언한 변수는 인스펙터에 나타나지 않음으로 Awake에서 초기화 시켜준다.
        GameManager.Instance = this;
    }
}
