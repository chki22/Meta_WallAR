using UnityEngine;
using UnityEngine.UI;

public class HidingObject : MonoBehaviour
{
    public void OnClick()
    {
        // GameManager���� ó��
        FindObjectOfType<GameManager>().OnObjectClicked(gameObject);
    }
}
