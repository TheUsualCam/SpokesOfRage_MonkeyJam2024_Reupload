using System.Collections;
using Assets._Scripts.Core_Scripts.Managers;
using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Behaviors;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using MonkeyJam._Scripts.Core_Scripts.StateMachine;
using MonkeyJam.Assets._Scripts.Core_Scripts.Managers;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.PlayerRelated
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : Core
    {
        [Header("Player Data")] 
        [SerializeField] private EntityData _data;

        [Header("Behaviours")] 
        public IdleState Idle; 
        public RunState Run;

        [Space]
        public BasicAttackMelee BasicAttack;
        public HeavyAttackMelee HeavyAttack;

        [Space]
        public HurtState Hurt;
        public DeathState Death;

        [Header("Movement Modifiers")] 
        [SerializeField, Range(1f, 10f)] private float _acceleration = 6f;
        [SerializeField, Range(5f, 20f)] private float _maxSpeed = 8f;
        [SerializeField, Range(0.0f, 1f)] private float _groundDecay = 0.4f;

        [Space, SerializeField] private float _stun = 0.5f;
        [SerializeField] private float _attackCooldown = 0.1f;


        [SerializeField] private bool _canAttack = true;

        // Input to get from the game manager
        private InputManager _input;
        private Rigidbody2D _body;

        private Vector2 _moveInput;
        private bool _isAttacking;

        void Start()
        {
            // Get body and input from input manager.
            _body = GetComponent<Rigidbody2D>();
            _input = GameManager.Singleton.InputManager;

            // setup instances
            SetUpInstances();

            // Set state to idle
            Machine = new Machine();
            Machine.Set(Idle);
        }

        void Update()
        {
            // whilst player didn't die, do the state.
            if (!_data.IsEntityDead())
            {
                // don't handle anything once player takes damage
                if (!_data.IsTakingDamage)
                {
                    ProcessInput();
                    FaceInput();
                }

                SelectState();
                Machine.CurrentState.Do();
            }
            else
            {
                // play the death animation
                Machine.Set(Death);

                // enable the game over ui
                if(!GameManager.Singleton.UiManager.GetIsGameOver())
                {
                    GameManager.Singleton.UiManager.GameOver();
                }
            }
        }

        void FixedUpdate()
        {
            if (_data.IsEntityDead()) return;

            // don't handle movement or apply friction if player died
            if (!_data.IsTakingDamage)
            {
                HandleMovement();
                ApplyFriction();
            }
                
            Machine.CurrentState.FixedDo();
        }

        private void SelectState()
        {
            // check if the player is taking damage
            if (_data.IsTakingDamage)
            {
                Machine.Set(Hurt);
                
                // await an 'x' amount of time to attack again
                StartCoroutine(WaitForStunCooldown());
            }

            // if the player isn't already attacking, and can attack do the attack
            if (!_isAttacking && _canAttack && !_data.IsTakingDamage)
            {
                // if player is attacking, play basic attack anim
                if (_input.GetBasicAttackInput() > 0)
                {
                    StartBasicAttack();
                    return; // exit early
                }

                // if player is heavy attacking, play heavy attack anim
                if (_input.GetHeavyAttackInput() > 0)
                {
                    StartHeavyAttack();
                    return; // exit early
                }
            }
            
            // if the player isn't attacking switch to either idle or run
            if (!_isAttacking && !_data.IsTakingDamage)
            {
                // set to idle once the player is not moving anymore
                if (_moveInput == Vector2.zero && Mathf.Abs(Body.velocity.x) < 0.1f && Mathf.Abs(Body.velocity.y) < 0.1f) {
                    Machine.Set(Idle);
                }
                else {
                    // run if otherwise
                    Machine.Set(Run);
                }
            }
        }

        private void StartBasicAttack()
        {
            // flag that the player is attacking
            _isAttacking = true;
                    
            // set combat
            Machine.Set(BasicAttack, true);

            // await an 'x' amount of time before attacking again.
            _canAttack = false;
            StartCoroutine(CompleteAttack());
        }

        private void StartHeavyAttack()
        {
            // flag that the player is attacking
            _isAttacking = true;
                    
            // set heavy attack
            Machine.Set(HeavyAttack, true);
            
            // await an 'x' amount of time before attacking again.
            _canAttack = false;
            StartCoroutine(CompleteAttack());
        }

        private IEnumerator CompleteAttack()
        {
            // await the cooldown
            yield return new WaitForSeconds(_attackCooldown);
            
            // reset flags
            _isAttacking = false;
            _canAttack = true;
        }

        private void ProcessInput()
        {
            // if the player isn't attacking, receive move input
            if (!_isAttacking)
                _moveInput = _input.GetMoveInput();
        }

        private void HandleMovement()
        {
            // don't handle input if nothing is being pressed
            if (_moveInput == Vector2.zero) return;

            // Increment velocity by acceleration, then clamp within max
            Vector2 increment = _moveInput * _acceleration;
            Vector2 newVelocity = _body.velocity + increment;
            newVelocity = Vector2.ClampMagnitude(newVelocity, _maxSpeed);

            // Change velocity
            _body.velocity = newVelocity;
        }

        private void ApplyFriction()
        {
            // if the player is attacking, stand still
            if (_isAttacking) _body.velocity = Vector2.zero;

            // Apply friction only if no input is being pressed.
            if (_moveInput == Vector2.zero)
            {
                _body.velocity = new Vector2(_body.velocity.x * _groundDecay, _body.velocity.y * _groundDecay);
            }
        }

        private void FaceInput()
        {
            // don't face anything if input is zero
            if (_moveInput.x == 0) return;

            // get the direction as a signed value
            float dir = Mathf.Sign(_moveInput.x);
            
            // apply direction to face where the entity wants to go
            transform.localScale = new Vector3(dir, 1, 1);
        }

        IEnumerator WaitForStunCooldown()
        {
            // await cooldown and reset flag of the entity data
            yield return new WaitForSeconds(_stun);
            _data.ResetIsTakingDamage();
        }
    }
}
