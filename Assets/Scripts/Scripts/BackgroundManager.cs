using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas(UI) 기준으로 2장의 배경을 무한 스크롤하는 매니저 스크립트.
/// bg1, bg2를 Inspector에서 할당한 뒤, 둘 다 동일 이미지/사이즈여야 함.
/// </summary>
public class UIInfiniteBackgroundManager : MonoBehaviour
{
    [Header("Background References")]
    [Tooltip("화면을 덮을 첫 번째 배경")]
    public RectTransform bg1;

    [Tooltip("무한 스크롤을 위해 bg1 왼편에 이어붙을 두 번째 배경")]
    public RectTransform bg2;

    [Header("Scrolling Speed (UI)")]
    [Tooltip("양수: 오른쪽 이동 / 음수: 왼쪽 이동 (단위: px/sec)")]
    public float speed = 200f;

    private float bgWidth;      // 배경 하나의 폭 (UI 좌표계)
    private float canvasWidth;  // Canvas의 폭
    private Canvas parentCanvas;

    void Start()
    {
        // 1) Canvas 및 RectTransform 가져오기
        parentCanvas = GetComponentInParent<Canvas>();
        if (!parentCanvas)
        {
            Debug.LogError("이 스크립트는 Canvas 하위 오브젝트여야 합니다!");
            return;
        }

        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
        canvasWidth = canvasRect.rect.width;    // Canvas 너비 (UI 좌표)

        // 2) bg1, bg2가 모두 지정되었는지 체크
        if (!bg1 || !bg2)
        {
            Debug.LogError("bg1, bg2를 인스펙터에서 할당해주세요!");
            return;
        }

        // 3) 두 배경(RectTransform) 중 하나의 폭을 가져와 사용 (둘은 동일한 이미지/사이즈라고 가정)
        bgWidth = bg1.rect.width;

        // 4) 배경 초기 위치 배치
        //    - bg1을 캔버스 중앙(anchoredPosition = (0,0))에 둔다
        //    - bg2는 bg1 왼쪽에(bgWidth만큼 왼쪽) 붙인다
        bg1.anchoredPosition = Vector2.zero;                // (0, 0)
        bg2.anchoredPosition = new Vector2(-bgWidth, 0f);   // ( -폭, 0 )

        // 필요하다면 세로(Y) 위치도 조정 가능:
        // bg1.anchoredPosition = new Vector2(0, someY);
        // bg2.anchoredPosition = new Vector2(-bgWidth, someY);
    }

    void Update()
    {
        // 매 프레임마다 bg1, bg2를 동시에 오른쪽으로 이동 (speed > 0)
        // 혹은 왼쪽 이동 (speed < 0)
        MoveBackground(bg1);
        MoveBackground(bg2);
    }

    /// <summary>
    /// 개별 배경(RectTransform)을 이동 & 위치 재배치
    /// </summary>
    void MoveBackground(RectTransform bg)
    {
        // 1) 이동 (UI 좌표에서 x만 변경)
        bg.anchoredPosition += Vector2.right * speed * Time.deltaTime;

        // 2) "왼쪽 끝" = anchoredPosition.x - (bgWidth * pivot.x)
        //    pivot=0.5 => (bgWidth * 0.5)
        float leftEdgeX = bg.anchoredPosition.x - (bgWidth * bg.pivot.x);

        // 3) 만약 "왼쪽 끝"이 canvasWidth(오른쪽 끝)보다 커졌다면 (즉 완전히 화면 밖)
        //    => bgWidth*2만큼 왼쪽으로 이동하여 반대편에 붙임
        if (leftEdgeX >= canvasWidth)
        {
            bg.anchoredPosition -= new Vector2(bgWidth * 2f, 0f);
        }

        // (참고) 왼쪽 이동( speed<0 )으로 무한 스크롤을 하고 싶다면,
        //       "오른쪽 끝"이 0보다 작아졌을 때 bg.anchoredPosition += new Vector2(bgWidth * 2f, 0)
        //       같은 식으로 별도 로직을 짤 수 있습니다.
    }
}
