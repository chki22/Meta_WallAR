using UnityEngine;

public class HidingObject : MonoBehaviour
{
    public void OnClick()
    {
        // GameManager에서 처리
        FindObjectOfType<GameManagerPlant>().OnObjectClicked(gameObject);
    }
}
