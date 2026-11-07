using System.Collections;
using UnityEngine;
using System;

public class ShiftTransition : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float flyDuration = 3f;
    [SerializeField] private float flyHeight = 2.5f;
    [SerializeField] private float progress = 0f;
    [SerializeField] GameObject lightSpherePrefab;

    public event Action OnTransitionComplete;
    public GameObject StartTransition(Vector3 start, Vector3 end)
    {
        Debug.Log("2. StartTransition wurde aufgerufen");
        progress = 0f;
        startPosition = start;
        endPosition = end;
        GameObject lightSphere = Instantiate(lightSpherePrefab, startPosition, lightSpherePrefab.transform.rotation);
        StartCoroutine(FlytoTarget(lightSphere));
        return lightSphere;
    }

    IEnumerator FlytoTarget(GameObject lightSphere)
    {
        while(progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.unscaledDeltaTime / flyDuration);
            float height = Mathf.Sin(progress * Mathf.PI) * flyHeight;
            Vector3 position = Vector3.Lerp(startPosition, endPosition, progress);
            position.y += height;
            lightSphere.transform.position = position;
            yield return null;
        }
        lightSphere.transform.position = endPosition;
        Debug.Log("3. Flug abgeschlossen");
        OnTransitionComplete?.Invoke();
        Destroy(lightSphere);
    }
}
