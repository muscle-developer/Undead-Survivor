using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 적의 속도
    [SerializeField]
    private float enemySpeed = 2.5f;
    [SerializeField]
    private float enemyHp = 0f;
    [SerializeField]
    private float enemyMaxHp = 0f;
    // 적의 애니메이션 관리
    public RuntimeAnimatorController[] animatorControllers;
    // 따라갈 목표
    [SerializeField]
    private Rigidbody2D target;
    // 생존 여부
    private bool isLive = false;

    private Rigidbody2D enemyRigidbody;
    private SpriteRenderer enemySpriteRen;
    private Animator enemyAnimator;

    void Awake()
    {    
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemySpriteRen = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // 만약 몬스터가 살아있지 않을 경우 실행 x
        if(!isLive)
            return;

        // 플레이어와 적의 위치거리
        Vector2 directionPosition = target.position - enemyRigidbody.position;
        // 적이 이동헤야할 방향의 거리의 값
        Vector2 nextPosition = directionPosition.normalized * enemySpeed * Time.fixedDeltaTime;
        // 적이 타겟을 향해 이동
        enemyRigidbody.MovePosition(enemyRigidbody.position + nextPosition);
        // 적과 플레이어가 충돌 시 물리 속도의 영향을 주지 않기 위해 0으로 지정
        enemyRigidbody.velocity = Vector2.zero;
    }

    // 모든 Update 함수가 호출된 후, 마지막으로 호출됩니다. 주로 오브젝트를 따라가게 설정한 카메라는 LateUpdate 를 사용
    private void LateUpdate()
    {
        if(!isLive)
            return;

        // 적의 시선(방향)처리 - 적을 기준으로 플레이어가 왼,오 인지 체크해서 바라보게
        enemySpriteRen.flipX = target.position.x < enemyRigidbody.position.x;
    }

    // 스크립트가 활성화 될 때, 호출되는 이벤트 함수
    void OnEnable()
    {
        // enemy가 Target을 찾을 수 있도록
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        // 스크립트가 활성화 되면 적은 살아있는 상태
        isLive = true;
        // 오브젝트 풀링으로 인하여 다시 활성화가 되면 적의 체력도 초기화 시키기
        enemyHp = enemyMaxHp;
    }

    // 적의 초기 속성을 적용(초기화)하는 함수 추가
    public void InitEnemyData(EnemySpawnData enemySpawnData)
    {
        // Spawn Data에 저장되어있는 값들을 Enemy에게 적용(초기값)설정
        // 각 타입에 맞는 적의 애니메이터로 설정
        enemyAnimator.runtimeAnimatorController = animatorControllers[enemySpawnData.enemyType];
        this.enemySpeed = enemySpawnData.enemySpeed;
        // 살아났을 때 적의 hp
        this.enemyMaxHp = enemySpawnData.enemyHP;
        // 현재 체력을 동기화 시켜주기 위해
        this.enemyHp = enemySpawnData.enemyHP;
    }

    // 충돌을 감지하기 위한 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적이 무기와 충돌했는지?
        if(!collision.CompareTag("Bullet"))
            return;

        // Bullet 컴포넌트로 접근하여 대미지를 가져와 피격계산하기
        enemyHp -= collision.GetComponent<Bullet>().damage;

        // 체력을 기준으로 피격인지 사망인지 체크
        if(enemyHp > 0)
        {
            // Live, Hit Action

        }
        else
        {   
            // 적이 죽었을 때
            EnemyDead();
        }
    }

    private void EnemyDead()
    {
        this.gameObject.SetActive(false);
    }
}
