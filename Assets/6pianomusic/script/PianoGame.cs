using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PianoGame : MonoBehaviour
{
    [SerializeField] GameObject cam;
    public AudioClip CClip, DClip, EClip, FClip, GClip, AClip, BClip;  // Assign these clips in the inspector
    //public Text notesText; // UI text to display the notes
    public UnityEngine.UI.Button playButton; // UI Button to start playing the song
    //public UnityEngine.UI.Button[] pianoButtons; // Array of buttons for piano keys (Q, W, E, R, T, Y, U)
    [SerializeField] canvasScript canvasSc;
    [SerializeField] List<KeyController> keys;
    private Dictionary<string, AudioClip> noteAudioClips;
    private List<(string note, float duration)> song = new List<(string note, float duration)> { };  // The song played by the system
    [SerializeField]private List<string> songNotes;
    [SerializeField]private List<float> songDuration;

    public List<string> playerInput;  // Store player's input

    private bool gameStarted = false;
    private bool playerTurn = false;
    private bool isCorrect = true;

    void Start()
    {
       
        // Initialize the dictionary to map notes to their audio clips
        noteAudioClips = new Dictionary<string, AudioClip>
        {
            { "C", CClip },
            { "D", DClip },
            { "E", EClip },
            { "F", FClip },
            { "G", GClip },
            { "A", AClip },
            { "B", BClip }
        };

        //Example song(with notes and duration)
        //song = new List<(string, float)>
        //{
        //    ("C", 0.5f),
        //    ("E", 0.5f),
        //    ("G", 0.5f),
        //    ("A", 1.0f)
        //};
        for (int i =0; i < songNotes.Count; i++) 
        {
            song.Add((songNotes[i], songDuration[i]));
        }

        // Initialize player input
        playerInput = new List<string>();

       
       
    }
    private void Update()
    {
        if (playerInput.Count == song.Count)
        {
            for (int i = 0; i < playerInput.Count; i++)
            {
                if (playerInput[i] != song[i].note)
                {
                    isCorrect = false;
                    canvasSc.state1 = canvasScript.state.fail;
                }
            }
            if (isCorrect)
            {
                Debug.Log("Success");
                canvasSc.state1 = canvasScript.state.success;
            }
            playerInput.Clear();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onlyplaysong(); // Call the function when the space key is pressed
        }
    }
    public void StartSong()
    {
        if (gameStarted)
        {
            // If the game was already started, reset the player input
            playerInput.Clear();
        }

        gameStarted = true;
        playerTurn = false;
        isCorrect = true;
        StartCoroutine(PlaySongWithNotes());

    }

    public void onlyplaysong()
    {
        
        StartCoroutine(PlaySongWithNotes());
        playerInput.Clear();// Play the notes sequence
    }

    IEnumerator PlaySongWithNotes()
    {
        foreach (var note in song)
        {
            //DisplayNote(note.note);  // Show the note on screen
            PlayNote(note.note);     // Play the note sound
            if (note.note == "C") 
                keys[0].isPressed = true;
            else if (note.note == "D")
                keys[1].isPressed = true;
            else if (note.note == "E")
                keys[2].isPressed = true;
            else if (note.note == "F")
                keys[3].isPressed = true;
            else if (note.note == "G")
                keys[4].isPressed = true;
            else if (note.note == "A")
                keys[5].isPressed = true;
            else if (note.note == "B")
                keys[6].isPressed = true;
            yield return new WaitForSeconds(note.duration);  // Wait for the note's duration
            if (note.note == "C")
                keys[0].isPressed = false;
            else if (note.note == "D")
                keys[1].isPressed = false;
            else if (note.note == "E")
                keys[2].isPressed = false;
            else if (note.note == "F")
                keys[3].isPressed = false;
            else if (note.note == "G")
                keys[4].isPressed = false;
            else if (note.note == "A")
                keys[5].isPressed = false;
            else if (note.note == "B")
                keys[6].isPressed = false;
        }

        // After the song is played, allow the player to mimic it
        playerTurn = true;
        //canvas enum toggle
        canvasSc.state1 = canvasScript.state.none;
        EnablePianoButtons();
    }

    void PlayNote(string note)
    {
        AudioSource.PlayClipAtPoint(noteAudioClips[note], cam.transform.position);
    }

    void EnablePianoButtons()
    {
        // Enable piano buttons for player to mimic the notes
        //foreach (UnityEngine.UI.Button button in pianoButtons)
        //{
        //    button.interactable = true;
        //    button.onClick.RemoveAllListeners();  // Remove any previous listeners
        //}

        // Add listeners to the buttons to record the player’s input
        //pianoButtons[0].onClick.AddListener(() => OnPlayerKeyPressed("C"));
        //pianoButtons[1].onClick.AddListener(() => OnPlayerKeyPressed("D"));
        //pianoButtons[2].onClick.AddListener(() => OnPlayerKeyPressed("E"));
        //pianoButtons[3].onClick.AddListener(() => OnPlayerKeyPressed("F"));
        //pianoButtons[4].onClick.AddListener(() => OnPlayerKeyPressed("G"));
        //pianoButtons[5].onClick.AddListener(() => OnPlayerKeyPressed("A"));
        //pianoButtons[6].onClick.AddListener(() => OnPlayerKeyPressed("B"));
    }

}