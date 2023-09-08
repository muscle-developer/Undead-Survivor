using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector2 inputVector;
    [SerializeField]
    private Rigidbody2D playerRigidBody;
    [SerializeField]
    private float playerSpeed = 1.0f;

    void Awake()
    {
        // Player의 RigidBody 컴포넌트를 선언한 변수에 넣어주기
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // x와 y축에 가로,세로로 이동 할 수 있도록 입력값 받아오기
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
    }   

    // 물리 연산 프레임마다 호출되는 생명주기 함수
    void FixedUpdate()
    {   
        // 이동할 위치
        Vector2 nextVector = inputVector.normalized * playerSpeed * Time.fixedDeltaTime;
        // 위치 이동 - 현재 나의 위치 + 내가 이동할 위치
        playerRigidBody.MovePosition(playerRigidBody.position + nextVector);
    }   
}
