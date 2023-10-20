using UnityEngine;

public class EnemySpawner : MonoBehaviour
{  
    // 적이 생성될 위치(배열)
    public Transform[] enemySpawnPoint;
    // 적  소환 타이머
    public float timer = 0f;

    void Awake()
    {
        // EnemySpawner가 붙어있는 자식 오브젝트에 생성되기 때문에 GetComponentsInChildren를 써준다.
        enemySpawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        // 타이머에 시간을 계속 더해주자
        timer += Time.deltaTime;
        // 일정 시간이 되면 소환이 되고 소환이 되면 다시 타이머를 0초로 돌려준다.
        if(timer > 0.2f)
        {
            EnemySpawn();
            timer = 0f;
        }
    }

    // 적을 소환하는 함수
    private void EnemySpawn()
    {
        GameObject enemy = GameManager.Instance.poolManager.GetGameobject(Random.Range(1,4));
        // 미리 만들어둔 SpawnPoint중 하나에 배치하자
        enemy.transform.position = enemySpawnPoint[Random.Range(1, enemySpawnPoint.Length)].position;
    }
}
