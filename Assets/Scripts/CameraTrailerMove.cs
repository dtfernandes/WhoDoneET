using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrailerMove : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0,-0.0025f,0);
    }
}
