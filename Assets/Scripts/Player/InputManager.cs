using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    // Unity Actions for each number key
    public UnityAction<int> OnNumberPressed;

    void Update()
    {
        CheckNumberInput();
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
