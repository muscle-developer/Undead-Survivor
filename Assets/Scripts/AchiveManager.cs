using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AchiveManager : MonoBehaviour
{
    public List<GameObject> lockCharacter = new List<GameObject>();

    // 업적 데이터 열거형으로 작성
    enum Achive { UnLockPotato, UnLockBean }
    private Achive[] achives;
    public GameObject uiNotice;
    
    // 코루틴 함수에서 매번 New Wait... 이런식으로 사용하는 것 보다
    // 미리 선언해서 저장해두는게 최적화에 조금더 도움이 된다.
    WaitForSecondsRealtime wait;

    void Awake()
    {   
        // Enum으로 작성한 데이터들을 사용할 수 있도록 초기화 시켜주기
        achives = (Achive[])Enum.GetValues(typeof(Achive)); 
        wait = new WaitForSecondsRealtime(5);

        // MyData를 가지고 있지 않을 때 초기화 시킨다.
        if(!PlayerPrefs.HasKey("MyData"))
            Init();
    }

    // 업적 저장데이터 초기화 함수
    void Init()
    {
        // MyData라는 Key값에 1(True) 가 저장된다.
        PlayerPrefs.SetInt("MyData", 1);

        // 아직은 업적달성이 없기 때문에 false로 저장
        foreach(var tmp in achives)
        {
            PlayerPrefs.SetInt(tmp.ToString(), 0);
        }
    }

    void Start()
    {
        UnLockCharacter();
    }

    private void UnLockCharacter()
    {
        for(int i = 0; i < lockCharacter.Count; i++)
        {
            // 칭호의 획득 여부를 판단 후
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;

            // 캐릭터가 잠금 상태라면 버튼 눌림 비활성화
            if(!isUnlock)
                lockCharacter[i].GetComponentInParent<Button>().interactable = false;
            else
                lockCharacter[i].GetComponentInParent<Button>().interactable = true;
            
            // 칭호를 획득했을 경우에만 활성화
            lockCharacter[i].SetActive(!isUnlock);
        }    
    }

    // 업적을 달성했는지 주기적 체크
    void LateUpdate()
    {
        foreach(var tmp in achives)
        {
            CheckAchive(tmp);
        }
    }

    // 업적을 달성했는지 확인하는 함수
    private void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch(achive)
        {   
            // 감자농부는 10킬 이상시에 해금
            case Achive.UnLockPotato:
                if(GameManager.Instance.isLive)
                {
                    isAchive = GameManager.Instance.kill >= 10;
                }
            break;
            // 콩농부는 최대플레이시간을 넘겼을 때 해금
            case Achive.UnLockBean:
                isAchive = GameManager.Instance.PlayTiem >= GameManager.Instance.maxPlayTime;
            break;
        }

        // 해금이 안된 상태에서 해금을 할 경우 에만
        if(isAchive && PlayerPrefs.GetInt(achive.ToSafeString()) == 0)
        {
            // 해당 업적을 해금으로 만들어주자.
            PlayerPrefs.SetInt(achive.ToSafeString(), 1);

            // 업적달성 팝업만큼 순회
            for(int i = 0; i < uiNotice.transform.childCount; i++)
            {
                // 몇번째 업적이 달성되었는지 체크 후 코루틴 실행
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeCoroutine());
        }
    }

    // 해금에 성공했을 때 알림창을 활성화 -> 비활성화 하는 코루틴 함수
    IEnumerator NoticeCoroutine()
    {
        uiNotice.SetActive(true);

        yield return wait;
        
        uiNotice.SetActive(false);
    }
}
