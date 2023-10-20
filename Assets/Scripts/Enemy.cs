using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 적의 속도
    [SerializeField]
    private float enemySpeed = 2.5f;
    // 따라갈 목표
    [SerializeField]
    private Rigidbody2D target;
    // 생존 여부
    private bool isLive = false;

    private Rigidbody2D enemyRigidbody;
    private SpriteRenderer enemySpriteRen;

    void Awake()
    {    
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemySpriteRen = GetComponent<SpriteRenderer>();
        isLive = true;
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
    }
}
