using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class NPC : MonoBehaviour, IInteractable
{
    //[SerializeField] private GameObject interactObject;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public abstract void Interact(PlayerInteraction interactor, out bool interactSuccessful);
}
