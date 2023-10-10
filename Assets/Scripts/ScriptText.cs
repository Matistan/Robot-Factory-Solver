using TMPro;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText(GameManager.text);
    }
}
