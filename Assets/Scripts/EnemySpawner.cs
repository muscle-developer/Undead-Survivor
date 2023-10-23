using System;
using UnityEngine;

// 적의 스폰관련 클래스
public class EnemySpawner : MonoBehaviour
{  
    // 적이 생성될 위치(배열)
    public Transform[] enemySpawnPoint;
    public EnemySpawnData[] enemySpawnData;
    // 적 소환 타이머
    private float timer = 0f;
    //난이도를 설정할 변수
    private int level = 0;

    void Awake()
    {
        // EnemySpawner가 붙어있는 자식 오브젝트에 생성되기 때문에 GetComponentsInChildren를 써준다.
        enemySpawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        // 타이머에 시간을 계속 더해주자
        timer += Time.deltaTime;
        // 10초마다 1레벨이 오르게 설정
        level =  Mathf.FloorToInt(GameManager.Instance.PlayTiem / 10f);
        // Level이 0일때는 1초에 주기로 소환 Level이 1이상 부터는 0.5초 주기로 소환
        if(timer > (level == 0 ? 1f : 0.5f))    
        {
            EnemySpawn(); 
            timer = 0f;
        }
    }

    // 적을 소환하는 함수
    private void EnemySpawn()
    {
        // 소환되는 타이밍도 레벨에 맞게 다른 몬스터가 생성되게 수정
        GameObject enemy = GameManager.Instance.poolManager.GetGameobject(level);
        // 미리 만들어둔 SpawnPoint중 하나에 배치하자
        enemy.transform.position = enemySpawnPoint[UnityEngine.Random.Range(1, enemySpawnPoint.Length)].position;
    }
}

// 직렬화[Serialization]:개체를 저장 혹은 전송하기 위해 변환, 즉 Inspector상에 보여주기 위해서 선언
[Serializable]
// 소환 데이터를 담당하는 클래스
public class EnemySpawnData
{
    // 적 타입
    public int enemyType = 0; 
    // 적 소환 주기 
    public float enemySpawnTime = 0f;
    // 적의 체력
    public float enemyHP = 0f;
    // 적의 이동속도
    public float enemySpeed = 0f;
}
