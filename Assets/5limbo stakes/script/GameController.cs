using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    // Input fields for bet amount and multiplier
    public TMP_InputField betInput;
    public TMP_InputField multiplierInput;

    // Text displays for balance, profit, win chance, result, and random multiplier
    public TMP_Text balanceText;
    public TMP_Text profitText;
    public TMP_Text winChanceText;
    public TMP_Text resultText;
    public TMP_Text randomMultiplierText;

    // History display and prefab
    public Transform historyDisplay; // Parent panel for multiplier history
    public GameObject historyPrefab; // Prefab for a single history entry

    // Panels for win and game over states
    [SerializeField] private GameObject winPanel; // Panel to activate on win
    [SerializeField] private GameObject gameOverPanel; // Panel to activate on game over
    [SerializeField] private float targetWinningAmount = 2000f; // Target amount to win
    [SerializeField] private float gameOverAmount = 50f; // Amount to trigger game over

    // Balance and history management
    private float balance = 1000f; // Starting balance
    private Queue<GameObject> multiplierHistory = new Queue<GameObject>(); // Queue for history entries
    private const int maxHistory = 6; // Max number of history entries

    void Start()
    {
        // Set default values for Bet Amount and Multiplier Input fields
        betInput.text = "100"; // Default bet amount
        multiplierInput.text = "1.1"; // Default multiplier value

        // Ensure the win and game over panels are initially deactivated
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Add listeners to update values dynamically when inputs change
        betInput.onValueChanged.AddListener(delegate { UpdateDynamicValues(); });
        multiplierInput.onValueChanged.AddListener(delegate { UpdateDynamicValues(); });

        // Initialize the UI
        UpdateDynamicValues();
        UpdateUI();
    }

    // Dynamically updates Profit on Win and Win Chance based on inputs
    public void UpdateDynamicValues()
    {
        if (float.TryParse(betInput.text, out float bet) && float.TryParse(multiplierInput.text, out float multiplier))
        {
            if (bet > 0 && multiplier > 1)
            {
                float winChance = Mathf.Clamp01(1 / multiplier) * 100;
                float profit = bet * (multiplier - 1);

                profitText.text = $"Profit on Win: <color=yellow>{profit:F2}</color>";
                winChanceText.text = $"Win Chance: <color=yellow>{winChance:F2}%</color>";
                return;
            }
        }

        // Default values if inputs are invalid
        profitText.text = "Profit on Win: <color=red>0</color>";
        winChanceText.text = "Win Chance: <color=red>0%</color>";
    }

    // Updates balance UI elements and checks for win or game over conditions
    public void UpdateUI()
    {
        balanceText.text = $"Balance: {balance:F2}";

        // Check if the player has reached the target amount
        if (balance >= targetWinningAmount && winPanel != null)
        {
            winPanel.SetActive(true); // Activate the win panel
        }

        // Check if the player's balance has dropped below the game over amount
        if (balance <= gameOverAmount && gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Activate the game over panel
        }
    }

    // Called when the player presses the "Bet" button
    public void PlaceBet()
    {
        if (float.TryParse(betInput.text, out float bet) && float.TryParse(multiplierInput.text, out float multiplier))
        {
            // Validate the bet and multiplier
            if (bet > balance || bet <= 0 || multiplier <= 1)
            {
                resultText.text = "<color=red>Invalid!</color>";
                return;
            }

            // Determine win or loss based on probability
            float winChance = Mathf.Clamp01(1 / multiplier);
            bool isWin = Random.value <= winChance; // Randomly decide win or loss based on probability

            float randomMultiplier;
            if (isWin)
            {
                // Generate a random multiplier greater than or equal to the chosen multiplier
                randomMultiplier = Random.Range(multiplier, multiplier + 10.0f); // Add a range to make it realistic
                float profit = bet * (multiplier - 1);
                balance += profit;
                resultText.text = $"<color=green>You Win! Multiplier: {randomMultiplier:F2}x</color>";
                AddToHistory(randomMultiplier, true); // Add win to history
            }
            else
            {
                // Generate a random multiplier less than the chosen multiplier
                randomMultiplier = Random.Range(1.0f, multiplier);
                balance -= bet;
                resultText.text = $"<color=red>You Lose! Multiplier: {randomMultiplier:F2}x</color>";
                AddToHistory(randomMultiplier, false); // Add loss to history
            }

            // Display the random multiplier
            randomMultiplierText.text = $"{randomMultiplier:F2}x";

            // Update the UI
            UpdateUI();
        }
        else
        {
            resultText.text = "<color=red>Invalid Input!</color>";
        }
    }

    // Adds a multiplier entry to the history panel
    private void AddToHistory(float multiplier, bool isWin)
    {
        // Create a new history entry from the prefab
        GameObject historyEntry = Instantiate(historyPrefab, historyDisplay);
        TMP_Text entryText = historyEntry.GetComponentInChildren<TMP_Text>();
        entryText.text = $"{multiplier:F2}x";

        // Set background color based on win or loss
        Image background = historyEntry.GetComponentInChildren<Image>();
        background.color = isWin ? Color.green : Color.gray;

        // Add the entry to the history queue
        multiplierHistory.Enqueue(historyEntry);

        // Remove the oldest entry if the history exceeds the limit
        if (multiplierHistory.Count > maxHistory)
        {
            GameObject oldestEntry = multiplierHistory.Dequeue();
            Destroy(oldestEntry);
        }
    }
}
