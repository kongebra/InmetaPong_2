using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(0, verticalInput * speed * Time.deltaTime, 0);
    }
}