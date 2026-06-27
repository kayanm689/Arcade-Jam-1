using UnityEngine;

public class PlayerVoices : MonoBehaviour
{
    [Header("Player Selection")]
    [Tooltip("Set to 1 for Player One, or 2 for Player Two")]
    [SerializeField] private string playerNumber = "1";

    [Header("Audio Components")]
    [SerializeField] private AudioSource audioSource;

    [Header("Voice Clips")]
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip gameOverClip;

    public int _lastKnownHealth = 4;
    private bool _playedGameOverSound = false;

    private void Start()
    {
        // Setup AudioSource fallback
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.Stop();
            }
        }

        // Initialize our health tracker based on what GameState currently says
        if (GameState.Instance != null)
        {
            _lastKnownHealth = (playerNumber == "1") ? GameState.Instance.playerOneHealth : GameState.Instance.playerTwoHealth;
        }
    }

    private void Update()
    {
        // Safe check to make sure GameState exists in the scene
        if (GameState.Instance == null) return;

        // 1. Get the current health from the global GameState singleton
        int currentHealth = (playerNumber == "1") ? GameState.Instance.playerOneHealth : GameState.Instance.playerTwoHealth;

        // 2. Detect Damage: If current health is lower than it was last frame
        if (currentHealth < _lastKnownHealth)
        {
            // Only play hit sound if they didn't just drop to/below 0 (saving room for death sound)
            if (currentHealth > 0)
            {
                PlayRandomHitVoice();
            }

            // Update our tracker so it doesn't loop infinitely
            _lastKnownHealth = currentHealth;
        }

        // 3. Detect Death: If health hits 0 and game state switches to GameOver
        if (currentHealth <= 0 && GameState.Instance.gameState == GameState.GameStateEnum.GameOver)
        {
            if (!_playedGameOverSound)
            {
                PlayGameOverVoice();
            }
        }
    }

    private void PlayRandomHitVoice()
    {
        if (hitClips == null || hitClips.Length == 0 || audioSource == null) return;

        int randomIndex = Random.Range(0, hitClips.Length);
        if (hitClips[randomIndex] != null)
        {
            audioSource.PlayOneShot(hitClips[randomIndex]);
        }
    }

    private void PlayGameOverVoice()
    {
        if (gameOverClip != null && audioSource != null)
        {
            _playedGameOverSound = true;
            audioSource.PlayOneShot(gameOverClip);
        }
    }
}