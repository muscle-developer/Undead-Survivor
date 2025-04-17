using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // 스캔할 범위
    public float scanRange;
    // 목표 레이어는 무엇인지?
    public LayerMask targetLayer;
    // 스캔 결과(Hit 한 Target들)
    public RaycastHit2D[] targets;
    // 가장 가까운 목표
    public Transform nearestTarget;

    void FixedUpdate()
    {
        // CircleCastAll(1.캐스팅 시작 위치, 2.원의 반지름, 3.쏘는 방향, 4.쏘는 방향의 길이, 5.대상 레이어)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // 지속적으로 가장 가까운 목표를 찾자
        nearestTarget = GetNearest();
    }


    // 가장 가까운 목표를 찾는 함수
    private Transform GetNearest()
    {
        Transform result = null;
        // 최소한의 거리를 체크하기위한 변수
        float diff = 100;

        // 캐스트함수와 충돌된게 있는지 체크 
        foreach(var tmp in targets)
        {   
            // 플레이어, 묙포 위치의 거리를 가져오자
            Vector3 myPosition = transform.position;
            Vector3 targetPosition = tmp.transform.position;
            float positionDiff = Vector3.Distance(myPosition, targetPosition);

            // 모든 적들의 거리를 계산한 후 제일 가까이 있는 목표의 위치를 가져오자
            if(positionDiff < diff)
            {
                diff = positionDiff;
                result = tmp.transform;
            }
        }

        // 그 후 가장 가까운 목표 위치를 반환
        return result;
    }
}
