using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class NPC : MonoBehaviour, IInteractable
{
    //[SerializeField] private GameObject interactObject;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public DialogueText dialogueText;
    public DialogueController dialogueController;
    public AudioClip happy, sad, neutral, angry, confused, shocked;

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public abstract void Interact(PlayerInteraction interactor, out bool interactSuccessful);
}
