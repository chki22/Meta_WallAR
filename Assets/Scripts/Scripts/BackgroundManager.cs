using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas(UI) �������� 2���� ����� ���� ��ũ���ϴ� �Ŵ��� ��ũ��Ʈ.
/// bg1, bg2�� Inspector���� �Ҵ��� ��, �� �� ���� �̹���/������� ��.
/// </summary>
public class UIInfiniteBackgroundManager : MonoBehaviour
{
    [Header("Background References")]
    [Tooltip("ȭ���� ���� ù ��° ���")]
    public RectTransform bg1;

    [Tooltip("���� ��ũ���� ���� bg1 ���� �̾���� �� ��° ���")]
    public RectTransform bg2;

    [Header("Scrolling Speed (UI)")]
    [Tooltip("���: ������ �̵� / ����: ���� �̵� (����: px/sec)")]
    public float speed = 200f;

    private float bgWidth;      // ��� �ϳ��� �� (UI ��ǥ��)
    private float canvasWidth;  // Canvas�� ��
    private Canvas parentCanvas;

    void Start()
    {
        // 1) Canvas �� RectTransform ��������
        parentCanvas = GetComponentInParent<Canvas>();
        if (!parentCanvas)
        {
            Debug.LogError("�� ��ũ��Ʈ�� Canvas ���� ������Ʈ���� �մϴ�!");
            return;
        }

        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
        canvasWidth = canvasRect.rect.width;    // Canvas �ʺ� (UI ��ǥ)

        // 2) bg1, bg2�� ��� �����Ǿ����� üũ
        if (!bg1 || !bg2)
        {
            Debug.LogError("bg1, bg2�� �ν����Ϳ��� �Ҵ����ּ���!");
            return;
        }

        // 3) �� ���(RectTransform) �� �ϳ��� ���� ������ ��� (���� ������ �̹���/�������� ����)
        bgWidth = bg1.rect.width;

        // 4) ��� �ʱ� ��ġ ��ġ
        //    - bg1�� ĵ���� �߾�(anchoredPosition = (0,0))�� �д�
        //    - bg2�� bg1 ���ʿ�(bgWidth��ŭ ����) ���δ�
        bg1.anchoredPosition = Vector2.zero;                // (0, 0)
        bg2.anchoredPosition = new Vector2(-bgWidth, 0f);   // ( -��, 0 )

        // �ʿ��ϴٸ� ����(Y) ��ġ�� ���� ����:
        // bg1.anchoredPosition = new Vector2(0, someY);
        // bg2.anchoredPosition = new Vector2(-bgWidth, someY);
    }

    void Update()
    {
        // �� �����Ӹ��� bg1, bg2�� ���ÿ� ���������� �̵� (speed > 0)
        // Ȥ�� ���� �̵� (speed < 0)
        MoveBackground(bg1);
        MoveBackground(bg2);
    }

    /// <summary>
    /// ���� ���(RectTransform)�� �̵� & ��ġ ���ġ
    /// </summary>
    void MoveBackground(RectTransform bg)
    {
        // 1) �̵� (UI ��ǥ���� x�� ����)
        bg.anchoredPosition += Vector2.right * speed * Time.deltaTime;

        // 2) "���� ��" = anchoredPosition.x - (bgWidth * pivot.x)
        //    pivot=0.5 => (bgWidth * 0.5)
        float leftEdgeX = bg.anchoredPosition.x - (bgWidth * bg.pivot.x);

        // 3) ���� "���� ��"�� canvasWidth(������ ��)���� Ŀ���ٸ� (�� ������ ȭ�� ��)
        //    => bgWidth*2��ŭ �������� �̵��Ͽ� �ݴ��� ����
        if (leftEdgeX >= canvasWidth)
        {
            bg.anchoredPosition -= new Vector2(bgWidth * 2f, 0f);
        }

        // (����) ���� �̵�( speed<0 )���� ���� ��ũ���� �ϰ� �ʹٸ�,
        //       "������ ��"�� 0���� �۾����� �� bg.anchoredPosition += new Vector2(bgWidth * 2f, 0)
        //       ���� ������ ���� ������ © �� �ֽ��ϴ�.
    }
}
