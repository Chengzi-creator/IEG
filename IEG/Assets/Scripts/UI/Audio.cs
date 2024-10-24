using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioSource _audio;

    private void Awake()
    {
        _toggle.onValueChanged.AddListener(isOn => ControlAudio());
        _slider.onValueChanged.AddListener(value => Volume(value));
    }

    private void ControlAudio()
    {
        if (_toggle.isOn)
        {
            _audio.gameObject.SetActive(true);
        }
        else
        {
            _audio.gameObject.SetActive(false);
        }
    }

    private void Volume(float value)
    {
        _audio.volume = value;
    }
}
