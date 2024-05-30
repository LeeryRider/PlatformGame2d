using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // Oyuncunun Transform bileþeni

    void Update()
    {
        // Kameranýn pozisyonunu oyuncunun pozisyonuna eþitle
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
