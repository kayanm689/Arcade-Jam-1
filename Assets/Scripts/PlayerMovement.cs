using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Movement speed modifier for the player
    public float speed = 5;
    
    // Reference to the 2D Rigidbody component for physics-based movement
    private Rigidbody2D _rigidbody2D;
    
    // Reference to the player actions script to determine player index/ID
    private PlayerActions _playerActions;

    private Animator _animator;

    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    private void Start() {
        // Cache the Rigidbody2D and PlayerActions components attached to this GameObject
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerActions = GetComponent<PlayerActions>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update() {
        // Only allow movement if the game is currently in a match
        if (GameState.Instance.gameState != GameState.GameStateEnum.InMatch) return;
        
        // Get horizontal input value (-1, 0, or 1) using the player's specific control axis
        float horizontal = Input.GetAxisRaw(GameState.Instance.horizontalAxis + _playerActions.playerCount);

        if (horizontal > 0)
        {
            _animator.SetBool("IsWalking", true);
            _spriteRenderer.flipX = false;
        }
        else if(horizontal < 0)
        {

            _animator.SetBool("IsWalking", true);
            _spriteRenderer.flipX = true;
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
        // Apply horizontal velocity while preserving the current vertical velocity
        _rigidbody2D.velocity = new Vector2(horizontal * speed, _rigidbody2D.velocity.y);
    }
}