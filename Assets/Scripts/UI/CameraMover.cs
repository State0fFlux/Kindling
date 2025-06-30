using System.Collections;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDuration = 1f;
    private Coroutine moveCoroutine;

    [Header("Scroll Settings")]
    public bool scrollEnabled = false;
    public float scrollSpeed = 5f;
    public float minOffset = -5f;
    public float maxOffset = 5f;

    private Vector3 scrollOrigin;       // World position where scrolling starts
    private float scrollOffsetAmount = 0f; // How far we've scrolled along the local up axis

    public void MoveTo(Transform target)
    {
        scrollEnabled = false;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToRoutine(target));
    }

    private IEnumerator MoveToRoutine(Transform target)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        Quaternion startRot = transform.rotation;
        Quaternion endRot = target.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / moveDuration);

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;

        if (target.name == "CreditsView")
        {
            scrollEnabled = true;
            scrollOrigin = transform.position;
            scrollOffsetAmount = 0f;
        }
    }

    void LateUpdate()
    {
        if (!scrollEnabled) return;

        float scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            float delta = scrollInput * scrollSpeed;
            float newOffset = Mathf.Clamp(scrollOffsetAmount + delta, minOffset, maxOffset);
            float clampedDelta = newOffset - scrollOffsetAmount;

            scrollOffsetAmount = newOffset;
            transform.position += transform.up * clampedDelta;
        }
    }
}
