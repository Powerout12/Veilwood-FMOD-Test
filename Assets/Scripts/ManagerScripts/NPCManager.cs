using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    //MANAGER FOR NPC QUEST AND DIALOGUE PROGRESSION

    public static NPCManager Instance;

    [Header("Main Quest Progression Bools")]
    public bool rascalWantsFood;
    public bool rascalMentionedKey;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else Instance = this;

    }

}
