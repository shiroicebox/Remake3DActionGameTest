using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ��������L���ɂ���
        Screen.autorotateToLandscapeLeft = true;
        // �E������L���ɂ���
        Screen.autorotateToLandscapeRight = true;

        // ��ʂ̌�����������]�ɐݒ肷��
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
