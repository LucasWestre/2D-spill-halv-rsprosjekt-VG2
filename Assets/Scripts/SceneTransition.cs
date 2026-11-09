using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class SceneTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is the Player
        if (collision.CompareTag("Player"))
        {
            // Get the name of the currently active scene (e.g., "Level 2" or "level 1")
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Automatically find the next level name
            string nextSceneName = GetNextLevelName(currentSceneName);

            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private string GetNextLevelName(string currentName)
    {
        // 1. Find where the numbers start in your string
        string numberString = "";
        string prefix = "";

        for (int i = 0; i < currentName.Length; i++)
        {
            if (char.IsDigit(currentName[i]))
            {
                numberString += currentName[i];
            }
            else
            {
                prefix += currentName[i];
            }
        }

        // 2. Convert the text number to an actual integer, add 1, and stitch it back together
        if (int.TryParse(numberString, out int currentLevelNumber))
        {
            int nextLevelNumber = currentLevelNumber + 1;
            return prefix + nextLevelNumber; // Returns "Level 3" if current was "Level 2"
        }

        // Fallback: If something goes wrong or no number is found, return the original name
        Debug.LogError("Could not find a level number in the scene name!");
        return currentName;
    }
}
