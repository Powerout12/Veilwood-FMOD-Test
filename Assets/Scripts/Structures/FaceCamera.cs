using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerCam>().transform;
        StartCoroutine("FacePlayer");
    }

    void Update()
    {
        
    }

    IEnumerator FacePlayer()
    {
        do
        {
            Vector3 fwd = player.forward; 
            fwd.y = 0; 
            if (fwd != Vector3.zero) transform.rotation = Quaternion.LookRotation(fwd);
            yield return new WaitForSeconds(0.2f);
        }
        while(gameObject.activeSelf);
    }


}
