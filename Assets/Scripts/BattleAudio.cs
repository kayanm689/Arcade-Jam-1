using UnityEngine;

public class BattleAudio : MonoBehaviour
{
    [Header("Audio Source Setup")]
    [Tooltip("Drag your AudioSource here. If left empty, the script will automatically grab or add one.")]
    [SerializeField] private AudioSource audioSource;

    [Header("Battle Sound Pool")]
    [Tooltip("Set the size to whatever you want and drop in your combat/voice clips!")]
    [SerializeField] private AudioClip[] battleClips;

    private void Awake()
    {
        // Automatically find or create an AudioSource component if the slot is empty
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            PlayRandomBattleSound();
        }
    }

    /// <summary>
    /// Call this function from any script whenever a battle event (hit, attack, clash) happens.
    /// </summary>
    public void PlayRandomBattleSound()
    {
        // Safety check to ensure clips are actually assigned
        if (battleClips == null || battleClips.Length == 0)
        {
            Debug.LogWarning($"[BattleAudio on {gameObject.name}]: No audio clips assigned in the battleClips array!");
            return;
        }

        // Pick a random element from the pool
        int randomIndex = Random.Range(0, battleClips.Length);

        // Play the selected clip safely
        if (battleClips[randomIndex] != null && audioSource != null)
        {
            audioSource.PlayOneShot(battleClips[randomIndex]);
        }
        else
        {
            Debug.LogError($"[BattleAudio on {gameObject.name}]: The clip at index {randomIndex} is missing or null.");
        }
    }
}
