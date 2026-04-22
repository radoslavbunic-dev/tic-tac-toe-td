using System.Collections;
using UnityEngine;

public class WinLine : MonoBehaviour
{
    [SerializeField] Transform line;
    [SerializeField] GameObject effect;
    [SerializeField] float scaleDuration = 0.25f;
    [SerializeField] float regularLineOffset;
    [SerializeField] float diagonalLineOffset;
    [SerializeField] float lengthOffset = 0.5f;

    Coroutine animationRoutine;

    void Awake()
    {
        effect.SetActive(false);
    }

    public void Play(Vector3 start, Vector3 end)
    {
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }
        animationRoutine = StartCoroutine(Animate(start, end));
    }

    IEnumerator Animate(Vector3 start, Vector3 end)
    {
        Transform lineParent = line.parent;
        Vector3 localStart = start;
        Vector3 localEnd = end;
        if (lineParent != null)
        {
            localStart = lineParent.InverseTransformPoint(start);
            localEnd = lineParent.InverseTransformPoint(end);
        }

        Vector3 direction = localEnd - localStart;
        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            yield break;
        }

        direction.Normalize();
        float offset = GetOffset(localStart, localEnd);
        Vector3 offsetVector = direction * offset;
        Vector3 adjustedStart = localStart + offsetVector;
        Vector3 adjustedEnd = localEnd + offsetVector;
        Vector3 delta = adjustedEnd - adjustedStart;
        float targetLength = delta.magnitude * lengthOffset;
        if (targetLength <= Mathf.Epsilon)
        {
            yield break;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.localPosition = adjustedStart;
        line.localRotation = Quaternion.Euler(0f, 0f, angle);

        Vector3 scale = line.localScale;
        scale.x = 0f;
        line.localScale = scale;

        float elapsed = 0f;
        float duration = scaleDuration <= 0f ? 0.0001f : scaleDuration;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            scale.x = Mathf.Lerp(0f, targetLength, t);
            line.localScale = scale;
            yield return null;
        }

        scale.x = targetLength;
        line.localScale = scale;

        Transform effectTransform = effect.transform;
        effectTransform.localPosition = adjustedStart;
        effectTransform.localRotation = line.localRotation;
        effectTransform.localScale = Vector3.one * line.localScale.x;
        effect.SetActive(true);
    }

    float GetOffset(Vector3 start, Vector3 end)
    {
        Vector3 delta = end - start;
        float absX = Mathf.Abs(delta.x);
        float absY = Mathf.Abs(delta.y);
        float axisEpsilon = 0.0001f;
        if (absX <= axisEpsilon || absY <= axisEpsilon)
        {
            return regularLineOffset;
        }

        float min = Mathf.Min(absX, absY);
        float max = Mathf.Max(absX, absY);
        float diagonalRatio = min / max;
        bool isDiagonal = diagonalRatio >= 0.98f;
        if (isDiagonal)
        {
            return diagonalLineOffset;
        }

        return regularLineOffset;
    }
}
