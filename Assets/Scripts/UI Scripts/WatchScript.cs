using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WatchScript : MonoBehaviour
{
    public GameObject watchHand;

    // Update is called once per frame
    void Update()
    {
        //watchHand.transform.rotation *= Quaternion.Euler(0,0,-30);
        UpdateWatch();
        
    }

    void UpdateWatch() //Cam don't look at this again it's been a long week
    {
       switch(TimeManager.currentHour)
       {
        case 0 or 12 or 24:
        watchHand.transform.rotation = Quaternion.Euler(0,0,0);
        break;

        case 1 or 13:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-30);
        break;

        case 2 or 14:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-60);
        break;

        case 3 or 15:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-90);
        break;

        case 4 or 16:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-120);
        break;

        case 5 or 17:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-150);
        break;

        case 6 or 18:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-180);
        break;

        case 7 or 19:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-210);
        break;

        case 8 or 20:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-240);
        break;

        case 9 or 21:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-270);
        break;

        case 10 or 22:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-300);
        break;

        case 11 or 23:
        watchHand.transform.rotation = Quaternion.Euler(0,0,-330);
        break;

        default:
        watchHand.transform.rotation = Quaternion.Euler(0,0,0);
        break;

       } 
    }
}
