using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerCam>().transform;
    }

    void Update()
    {
        Vector3 fwd = player.forward; 
        fwd.y = 0; 
        transform.rotation = Quaternion.LookRotation(fwd);
    }


}
