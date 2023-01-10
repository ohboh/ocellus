using UnityEngine;
using TMPro;

public class GetName : MonoBehaviour
{
    void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = SystemInfo.deviceName.ToLower();
    }
}
