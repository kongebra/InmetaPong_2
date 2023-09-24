using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        Console.WriteLine($"Vertical speed: {verticalInput * speed * Time.deltaTime}");
        transform.Translate(0, verticalInput * speed * Time.deltaTime, 0);
    }
}