using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    // Unity Actions for each number key
    public UnityAction<int> OnNumberPressed;

    //FOR TOGGLING THE GRID
    public Tilemap structGrid;
    public Color activeColor, hiddenColor;

    void Update()
    {
        CheckNumberInput();

        if(Input.GetKeyDown("g") && structGrid)
        {
            if(structGrid.color == activeColor) structGrid.color = hiddenColor;
            else structGrid.color = activeColor;
        }
    }

    void CheckNumberInput()
    {
        for (int i = 1; i <= 9; i++)
        {
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i);
            if (Input.GetKeyDown(key))
            {
                // Invoke the action and pass the number that was pressed
                OnNumberPressed?.Invoke(i);
            }
        }
    }
}
