using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector2 inputVector;
    [SerializeField]
    private Rigidbody2D playerRigidBody;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private float playerSpeed = 5.0f;

    void Awake()
    {
        // Player의 RigidBody 컴포넌트를 선언한 변수들 초기화
        playerRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer >();
        playerAnimator = GetComponent<Animator>();
    }

    // void Update()
    // {
    //     // x와 y축에 가로,세로로 이동 할 수 있도록 입력값 받아오기
    //     inputVector.x = Input.GetAxis("Horizontal");
    //     inputVector.y = Input.GetAxis("Vertical");
    // }   

    // 물리 연산 프레임마다 호출되는 생명주기 함수
    void FixedUpdate()
    {   
        // 이동할 위치
        Vector2 nextVector = inputVector * playerSpeed * Time.fixedDeltaTime;
        // 위치 이동 - 현재 나의 위치 + 내가 이동할 위치
        playerRigidBody.MovePosition(playerRigidBody.position + nextVector);
    }   
    
    // 프레임이 종료 되기 전 호출되는 함수
    void LateUpdate()
    {   
        // 움직이는 모션을 주기 위해 파라메터와 동일한 타입의 함수 호출, SetFloat("파라메터 이름", 반영할 값) , magnitude -> 벡터의 길이를 반환
        playerAnimator.SetFloat("Speed", inputVector.magnitude);

        // 가로 x의 움직임이 - , + 가 될 떼 
        // 현재 플레이어는 우측을 바라보기 때문에 - 값이 됐을 때만 flipX 를 true로 만들어준다.
        if(inputVector.x != 0)
            spriteRenderer.flipX = inputVector.x < 0;
    }

    // Input System 에서 제공하는 이동함수
    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
}
