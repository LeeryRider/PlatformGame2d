using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // Oyuncunun Transform bile�eni

    void Update()
    {
        // Kameran�n pozisyonunu oyuncunun pozisyonuna e�itle
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
