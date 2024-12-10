using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWithinCanvas : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // ���� UI ������Ʈ�� RectTransform
    private Canvas canvas; // ������Ʈ�� ���� Canvas
    private RectTransform canvasRectTransform; // Canvas�� RectTransform
    private Vector2 offset; // ���콺 Ŭ�� �� ������Ʈ�� ���콺 ��ġ ���� �Ÿ�

    void Start()
    {
        // RectTransform�� Canvas �ʱ�ȭ
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Ŭ�� �� ���콺�� ������Ʈ �߽� ���� �Ÿ� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        offset = rectTransform.localPosition - (Vector3)localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���콺 ��ġ�� �������� ������Ʈ �̵�
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            // �� ��ġ ��� (���콺 ��ġ + �ʱ� offset)
            Vector3 newPosition = localPoint + offset;

            // Canvas ��� ���� ����
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
        // �巡�� ���� �� �߰� �۾� ���� (�ʿ��ϸ� ����)
    }
}
