using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Attached to virtual AR buttons (e.g., 'ON' and 'OFF')
/// to issue RESTful HTTP requests to Blynk Cloud.
/// </summary>
public class ClickUrl : MonoBehaviour
{
    [Tooltip("Target Blynk Cloud REST API URL (e.g. https://blynk.cloud/external/api/update?token=TOKEN&V0=1)")]
    public string url;

    public void open()
    {
        if (!string.IsNullOrEmpty(url))
        {
            StartCoroutine(GetRequest(url));
        }
        else
        {
            Debug.LogWarning("[AR-IoT] Target URL is empty. Assign URL in Inspector.");
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"[AR-IoT] HTTP Error ({uri}): {webRequest.error}");
            }
            else
            {
                Debug.Log($"[AR-IoT] Command successfully dispatched. Response: {webRequest.downloadHandler.text}");
            }
        }
    }
}
