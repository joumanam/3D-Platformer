using UnityEngine;

public class CoinFlyEffect : MonoBehaviour
{
    public RectTransform targetUI;
    public float speed = 800f;

    private RectTransform rectTransform;
    private bool isMoving = false;

    public void StartFly(Vector3 worldPosition, Canvas canvas)
    {
        rectTransform = GetComponent<RectTransform>();

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPoint, null, out Vector2 localPos
        );

        rectTransform.anchoredPosition = localPos;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || targetUI == null || rectTransform == null)
            return;

        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            targetUI.anchoredPosition,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(rectTransform.anchoredPosition, targetUI.anchoredPosition) < 5f)
        {
            CoinManager.Instance.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
