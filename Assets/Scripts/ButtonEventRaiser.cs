using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEventRaiser : MonoBehaviour
{
    [SerializeField]
    private GameEvent _event;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();

        if (_event != null)
        {
            _button.onClick.AddListener(_event.Raise);
        }
    }
}
