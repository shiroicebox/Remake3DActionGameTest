using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashControler : MonoBehaviour
{
    public ZoomAction _action;

    private Image _img;
    // Start is called before the first frame update
    void Start()
    {
        _img = GetComponent<Image>();
        _img.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        _img.color = Color.Lerp(_img.color, Color.clear, Time.deltaTime);
    }

    public void ShutterEffect()
    {
        _img.color = new Color(1, 1, 1, 1);
    }
}
