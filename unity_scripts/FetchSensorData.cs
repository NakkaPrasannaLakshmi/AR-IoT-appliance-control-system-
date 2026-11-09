using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

/// <summary>
/// Fetches real-time environmental telemetry (temperature and humidity)
/// from Blynk Cloud API and updates spatial AR text overlays.
/// </summary>
public class FetchSensorData : MonoBehaviour
{
    [Header("Blynk Cloud REST Endpoints")]
    [Tooltip("API endpoint to fetch Temperature from pin V1")]
    public string temperatureUrl = "https://blynk.cloud/external/api/get?token=YOUR_TOKEN_ID&pin=V1";

    [Tooltip("API endpoint to fetch Humidity from pin V2")]
    public string humidityUrl = "https://blynk.cloud/external/api/get?token=YOUR_TOKEN_ID&pin=V2";

    [Header("AR TextMeshPro Display Elements")]
    public TMP_InputField TextFieldTemp;
    public TMP_InputField InputFieldHum;

    /// <summary>
    /// Invoked upon tapping the virtual AR 'DATA' button.
    /// </summary>
    public void OnButtonClick()
    {
        StartCoroutine(FetchTemperatureData());
        StartCoroutine(FetchHumidityData());
    }

    IEnumerator FetchTemperatureData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(temperatureUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("[AR-IoT] Error fetching temperature data: " + webRequest.error);
            }
            else
            {
                string temperatureData = webRequest.downloadHandler.text.Trim();
                if (TextFieldTemp != null)
                {
                    TextFieldTemp.text = temperatureData + " °C";
                }
                Debug.Log("[AR-IoT] Temperature Data Received: " + temperatureData);
            }
        }
    }

    IEnumerator FetchHumidityData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(humidityUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("[AR-IoT] Error fetching humidity data: " + webRequest.error);
            }
            else
            {
                string humidityData = webRequest.downloadHandler.text.Trim();
                if (InputFieldHum != null)
                {
                    InputFieldHum.text = humidityData + " %";
                }
                Debug.Log("[AR-IoT] Humidity Data Received: " + humidityData);
            }
        }
    }
}
