using UnityEngine;

public class HidingObject : MonoBehaviour
{
    public void OnClick()
    {
        // GameManager���� ó��
        FindObjectOfType<GameManagerPlant>().OnObjectClicked(gameObject);
    }
}
