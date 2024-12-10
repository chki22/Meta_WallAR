using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWithinCanvas : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // 현재 UI 오브젝트의 RectTransform
    private Canvas canvas; // 오브젝트가 속한 Canvas
    private RectTransform canvasRectTransform; // Canvas의 RectTransform
    private Vector2 offset; // 마우스 클릭 시 오브젝트와 마우스 위치 간의 거리

    void Start()
    {
        // RectTransform과 Canvas 초기화
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 클릭 시 마우스와 오브젝트 중심 간의 거리 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        offset = rectTransform.localPosition - (Vector3)localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 위치를 기준으로 오브젝트 이동
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            // 새 위치 계산 (마우스 위치 + 초기 offset)
            Vector3 newPosition = localPoint + offset;

            // Canvas 경계 내로 제한
            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(newPosition.x, -canvasRectTransform.rect.width / 2 + rectTransform.rect.width / 2, canvasRectTransform.rect.width / 2 - rectTransform.rect.width / 2),
                Mathf.Clamp(newPosition.y, -canvasRectTransform.rect.height / 2 + rectTransform.rect.height / 2, canvasRectTransform.rect.height / 2 - rectTransform.rect.height / 2),
                rectTransform.localPosition.z
            );

            rectTransform.localPosition = clampedPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 시 추가 작업 가능 (필요하면 구현)
    }
}
