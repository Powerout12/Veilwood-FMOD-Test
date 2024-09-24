using SaveLoadSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSaveData : MonoBehaviour
{
    private PlayerData MyData = new PlayerData();
    public int currentHealth = 10;
  

    void Update()
    {
       /* //TODO: Make this about every 30 seconds rather than every frame

        MyData.PlayerPosition = transform.position;
        MyData.CurrentHealth = currentHealth;

        if (Input.GetKeyDown(KeyCode.R)) //TODO: Either keep this as button or keybind
        {
            SaveLoad.CurrentSaveData.PlayerData = MyData;
            SaveLoad.SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.T)) //TODO: Change this
        {
            SaveLoad.LoadGame();
            MyData = SaveLoad.CurrentSaveData.PlayerData;
            transform.position = MyData.PlayerPosition;
            currentHealth = MyData.CurrentHealth;

        }*/
    } 
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 PlayerPosition;
    public int CurrentHealth;
}