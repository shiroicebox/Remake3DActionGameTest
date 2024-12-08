using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 左向きを有効にする
        Screen.autorotateToLandscapeLeft = true;
        // 右向きを有効にする
        Screen.autorotateToLandscapeRight = true;

        // 画面の向きを自動回転に設定する
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
