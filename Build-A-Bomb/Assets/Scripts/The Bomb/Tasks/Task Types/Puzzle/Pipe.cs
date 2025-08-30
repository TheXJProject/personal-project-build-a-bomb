using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pipe : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] RectTransform symbol;
    [SerializeField] Image light_;
    [SerializeField] Image symbolLight_;

    // Runtime Variables:
    int symbolPosition = 0;
    [HideInInspector] public bool inCorrectPos = false;
    [HideInInspector] public int orderNumber;
    PuzzleLogic mainPuzzle;

    public void Setup()
    {
        // Search all parents (including immediate parent) for MyScript
        mainPuzzle = GetComponentInParent<PuzzleLogic>();

        // Error check
        if (mainPuzzle == null)
        {
            Debug.LogWarning("No parent has MyScript attached.");
        }
    }

    public void SetStartPosition(int startPos)
    {
        // Set start position
        symbolPosition = startPos % mainPuzzle.maxPositions;

        // Use the position to set the rotation
        float angle = 360 * symbolPosition / mainPuzzle.maxPositions;
        symbol.localRotation = Quaternion.Euler(0, 0, -angle);

        // Check if starting in correct position
        CheckCorrectPos();
    }

    public void RotateOnePos(BaseEventData data)
    {
        if (mainPuzzle.statInteract.isBeingSolved)
        {
            // check for a left click
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                // Increase position by one and wrap if needs
                symbolPosition = (symbolPosition + 1) % mainPuzzle.maxPositions;

                // Use the position to set the rotation
                float angle = 360 * symbolPosition / mainPuzzle.maxPositions;
                symbol.localRotation = Quaternion.Euler(0, 0, -angle);

                // Call the next one along to be changed aswell
                mainPuzzle.SetSeconPos(orderNumber);

                // Check if we are in the correct position
                CheckCorrectPos();
            }
        }
    }

    public void RotateOnePos()
    {
        if (mainPuzzle.statInteract.isBeingSolved)
        {
            // Increase position by one and wrap if needs
            symbolPosition = (symbolPosition + 1) % mainPuzzle.maxPositions;

            // Use the position to set the rotation
            float angle = 360 * symbolPosition / mainPuzzle.maxPositions;
            symbol.localRotation = Quaternion.Euler(0, 0, -angle);

            // Check if we are in the correct position
            CheckCorrectPos();
        }
    }

    void CheckCorrectPos()
    {
        // Check if symbol is in correct position and adjust light to show
        inCorrectPos = (symbolPosition == PuzzleLogic.finalPosition);
        if (inCorrectPos)
        {
            light_.color = Color.green;
            symbolLight_.color = Color.green;
        }
        else
        {
            light_.color = Color.red;
            symbolLight_.color = Color.yellow;
        }
    }
}
