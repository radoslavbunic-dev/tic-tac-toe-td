using System.Collections;
using UnityEngine;

public class WinLine : MonoBehaviour
{
    [SerializeField] Transform line;
    [SerializeField] float scaleDuration = 0.25f;
    [SerializeField] float regularLineOffset;
    [SerializeField] float diagonalLineOffset;
    [SerializeField] float lengthOffset = 0.5f;

    Coroutine animationRoutine;

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
        Vector3 direction = end - start;
        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            yield break;
        }

        direction.Normalize();
        float offset = GetOffset(start, end);
        Vector3 offsetVector = direction * offset;
        Vector3 adjustedStart = start + offsetVector;
        Vector3 adjustedEnd = end + offsetVector;
        Vector3 delta = adjustedEnd - adjustedStart;
        float targetLength = delta.magnitude * lengthOffset;
        if (targetLength <= Mathf.Epsilon)
        {
            yield break;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.position = adjustedStart;
        line.rotation = Quaternion.Euler(0f, 0f, angle);

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
    }

    float GetOffset(Vector3 start, Vector3 end)
    {
        Vector3 delta = end - start;
        bool isDiagonal = Mathf.Approximately(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        if (isDiagonal)
        {
            return diagonalLineOffset;
        }

        return regularLineOffset;
    }
}
