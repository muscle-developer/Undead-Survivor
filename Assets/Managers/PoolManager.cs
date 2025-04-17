using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] prefabs;
    // 풀 담당을 하는 리스트
    [SerializeField]
    private List<GameObject>[] pools;

    void Awake()
    {
        // 풀 리스트에 Prefab의 개수만큼 넣어주고
        pools = new List<GameObject>[prefabs.Length];

        // 풀 리스트들을 모두 초기화 시켜주자
        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
 
    // Gameobject 를 Return 하는 함수
    // 가져올 오브젝트 종류를 결정하는 매개 변수 index
    public GameObject GetGameobject(int index)
    { 
        // 빈 게임오브젝트 생성
        GameObject selectGameobject = null;

        // 비활성화 된 게임오브젝트 접근 - pools 에 들어있는 Prefab 갯수만큼 접근
        foreach(var tmp in pools[index])
        {
            if(!tmp.activeSelf)
            {   
                // Prefab 오브젝트가 비활성화 되어있는게 있으면 빈 게임 오브젝트에 넣어주고 활성화 시켜주기
                selectGameobject = tmp;
                selectGameobject.SetActive(true);
                break;
            }
        }

        // 만약 못찾았을(비어있는) 경우 (selectGameobject 가 null인 경우)
        if(!selectGameobject)
        {
            // 새롭게 생성해 selectGameobject 에 넣어주자(pool Manager에 생성되게)
            selectGameobject = Instantiate(prefabs[index], transform);
            // 그 후 풀리스트에 넣어주자(List.Add를 함으로 써 List에 추가된다.)
            pools[index].Add(selectGameobject); 
        }

        return selectGameobject;          
    }
}
