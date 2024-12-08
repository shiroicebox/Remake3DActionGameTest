using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ¶Œü‚«‚ğ—LŒø‚É‚·‚é
        Screen.autorotateToLandscapeLeft = true;
        // ‰EŒü‚«‚ğ—LŒø‚É‚·‚é
        Screen.autorotateToLandscapeRight = true;

        // ‰æ–Ê‚ÌŒü‚«‚ğ©“®‰ñ“]‚Éİ’è‚·‚é
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
