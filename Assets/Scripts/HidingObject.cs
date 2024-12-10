using UnityEngine;
using UnityEngine.UI;

public class HidingObject : MonoBehaviour
{
    public void OnClick()
    {
        // GameManager에서 처리
        FindObjectOfType<GameManager>().OnObjectClicked(gameObject);
    }
}
