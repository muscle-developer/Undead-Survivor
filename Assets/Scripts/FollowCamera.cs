using UnityEngine;

public class FollowCamera : MonoBehaviour
{   
    RectTransform hpRectTransform;
    void Awake()
    {
        hpRectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // hp이미지를 따라가게 만들자(WorldToScreenPoint 함수는 World의 좌표를 Canvas의 좌표로 변환시켜줌)
        hpRectTransform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);
    }
}
