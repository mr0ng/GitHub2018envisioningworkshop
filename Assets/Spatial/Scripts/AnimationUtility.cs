using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtility
{

    public static IEnumerator DoSomethingAfterSec(float sec, System.Action func)
    {
        yield return new WaitForSeconds(sec);
        func?.Invoke();
    }


    public static IEnumerator ApplyScaleAnimation(GameObject go, float duration, Vector3 startScale, Vector3 endScale, System.Action<GameObject> actionEachFrame = null)
    {
        float animProgress = 0;
        go.transform.localScale = startScale;
        while (animProgress < duration)
        {
            animProgress += Time.deltaTime;
            float percent = animProgress / duration;
            var scale = Vector3.Lerp(startScale, endScale, percent);
            go.transform.localScale = scale;
            actionEachFrame?.Invoke(go);
            yield return null;
        }
        go.transform.localScale = endScale;
    }

    public static IEnumerator LocalMoveAnimation(GameObject go, float duration, Vector3 startPosition, Vector3 endPosition, System.Action<GameObject> actionEachFrame = null)
    {
        float animProgress = 0;
        go.transform.localPosition = startPosition;
        while (animProgress < duration)
        {
            animProgress += Time.deltaTime;
            float percent = animProgress / duration;
            var position = Vector3.Lerp(startPosition, endPosition, percent);
            go.transform.localPosition = position;
            actionEachFrame?.Invoke(go);
            yield return null;
        }
        go.transform.localPosition = endPosition;
    }

    public static IEnumerator LocalRotateAnimation(GameObject go, float duration, Quaternion startRotation, Quaternion endRotation, System.Action<GameObject> actionEachFrame = null)
    {
        float animProgress = 0;
        go.transform.localRotation = startRotation;
        while (animProgress < duration)
        {
            animProgress += Time.deltaTime;
            float percent = animProgress / duration;
            var rotation = Quaternion.Lerp(startRotation, endRotation, percent);
            go.transform.localRotation = rotation;
            actionEachFrame?.Invoke(go);
            yield return null;
        }
        go.transform.localRotation = endRotation;
    }
}
