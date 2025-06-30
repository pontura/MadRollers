using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VerticalSnapStable : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;
    public float snapDuration = 0.2f;
    public float velocityThreshold = 100f;

    private bool isDragging = false;
    private bool isSnapping = false;
    private bool hasInteracted = false;
    private bool hasSnappedAfterDrag = false;

    void Update()
    {
        if (hasInteracted && !isDragging && !isSnapping && !hasSnappedAfterDrag && scrollRect.velocity.magnitude < velocityThreshold)
        {
            StartCoroutine(SnapOnceToClosest());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        hasInteracted = true;
        hasSnappedAfterDrag = false;

        if (isSnapping)
        {
            StopAllCoroutines();
            isSnapping = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    IEnumerator SnapOnceToClosest()
    {
        isSnapping = true;

        yield return null;

        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        int itemCount = content.childCount;
        if (itemCount == 0)
        {
            isSnapping = false;
            yield break;
        }

        float closestDistance = float.MaxValue;
        int closestIndex = -1;

        Vector2 viewportCenterLocal = viewport.rect.center;

        // ¡Calculamos el ítem más cercano solo una vez!
        for (int i = 0; i < itemCount; i++)
        {
            RectTransform item = content.GetChild(i) as RectTransform;
            Vector2 itemWorld = item.position;
            Vector2 itemLocal = viewport.InverseTransformPoint(itemWorld);
            float distance = Mathf.Abs(itemLocal.y - viewportCenterLocal.y);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        if (closestIndex == -1)
        {
            isSnapping = false;
            yield break;
        }

        // Calcular posición normalizada
        float step = 1f / Mathf.Max(1, itemCount - 1);
        float targetNormalized = 1f - (step * closestIndex); // vertical: 1 = top

        float start = scrollRect.verticalNormalizedPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / snapDuration;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, targetNormalized, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = targetNormalized;
        scrollRect.velocity = Vector2.zero;

        isSnapping = false;
        hasSnappedAfterDrag = true; // 🔒 Bloquea futuros snaps hasta nuevo drag
    }
}
