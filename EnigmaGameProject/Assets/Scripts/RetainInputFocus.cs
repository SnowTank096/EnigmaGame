using UnityEngine;
using TMPro; // Remove if using standard legacy UI InputField

public class RetainInputFocus : MonoBehaviour
{
    private TMP_InputField inputField; // Change to 'InputField' if using Legacy UI

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        
        // Listen for when editing finishes (e.g., when Enter is pressed)
        inputField.onEndEdit.AddListener(OnEndEditDetected);
    }

    void OnEndEditDetected(string text)
    {
        // Check if the user closed it with 'Enter' rather than clicking away/Escape
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Process or submit your text here (optional)
            ProcessInput(text);

            // Re-activate and prepare the field for typing immediately
            inputField.ActivateInputField();
            
            // Optional: Keeps the cursor blinking at the very end of the text
            inputField.Select(); 
        }
    }

    void ProcessInput(string input)
    {
        Debug.Log($"Submitted: {input}");
        
        // Clear field if making a chat box, otherwise leave text intact
        // inputField.text = ""; 
    }
}
