using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        // 읽기 전용으로 
        // 남자캐릭터를 선택할경우 이동속도 1.1f 여자캐릭터 경우 1f
        get { return GameManager.Instance.playerId == 0 ? 1.1f : 1f; } 
    }

    // 근접무기의 속도
    public static float WeaponSpeed
    {
        // 여자캐릭터를 선택할경우 근접무기 공격속도 1.1f 남자캐릭터 경우 1f
        get { return GameManager.Instance.playerId == 1 ? 1.1f : 1f; } 
    }

    // 원거리무기 속도
    public static float WeaponRate
    {
        // 여자캐릭터를 선택할경우 원거리 무기 공격속도 0.9f 남자캐릭터 경우 1f
        get { return GameManager.Instance.playerId == 1 ? 0.9f : 1f; } 
    }

    // 예를 들어 여러가지 캐릭터가 더 추가된다면 위처럼 똑같이 더 추가해서 관리해주면 된다.
}
