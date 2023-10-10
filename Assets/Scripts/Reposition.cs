using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Reposition : MonoBehaviour
{
    // 오브젝트 충돌을 감지하는 함수 
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 오브젝트에 Tag가 Area일때만 실행되도록 
        if(!collision.CompareTag("Area"))
            return;

        // 타일맵의 위치와 플레이어의 위치 변수 생성
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 tilePos = this.transform.position;

        // 플레이어 위치 - 타일맵 위치를 함으로 써 x, y축의 거리를 계산한다.
        // Mathf.Abs - 절대값(값이 음수일 때 양수 값으로 만들기 위해)
        float diffx = Mathf.Abs(playerPos.x - tilePos.x);
        float diffy = Mathf.Abs(playerPos.y - tilePos.y);

        // 플레이어의 이동 방향을 저장하기 위한 변수 추가
        Vector3 playerDirection = GameManager.Instance.player.InputVector;

        // 삼항 연산자를 사용한 조건문
        // (조건) ? (true일 때 값) : (false일 때 값)
        // Normalized를 함으로 써 대각선으로 이동 시 1값 보다 작다.
        float direcitonX = playerDirection.x < 0 ? -1 : 1;
        float direcitonY = playerDirection.y < 0 ? -1 : 1;

        // 타일맵이 어떤 태그랑 충돌하는지 체크하기 위해서
        switch(transform.tag)
        {   
            // 타일맵일 경우
            case "Ground":
                // x, y축으로 이동했을 경우 x, y의 방향으로 타일맵을 재배치 하기
                if(diffx > diffy)
                    this.transform.Translate(Vector3.right * direcitonX * 40f);
                else if(diffx < diffy)
                    this.transform.Translate(Vector3.up * direcitonY * 40f);
                break;
            // 적일 경우
            case "Enemy":
                break;
        }
         
    }
}
