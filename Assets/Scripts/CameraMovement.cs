using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt( new Vector3( 0, 0, 0 ) );
        transform.Translate( Vector3.right * Time.deltaTime );

        transform.Translate( Vector3.back * ( Input.mouseScrollDelta.y * Time.deltaTime * 100 ) );
    }
}
