using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Scripts.Core_Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput playerInputSystem;

        private Vector2 movement = new Vector2(0, 0);
        private float basicAttack;
        private float heavyAttack;

        private InputManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
                Destroy(gameObject);
        }

        private void Start()
        {
            playerInputSystem = FindObjectOfType<PlayerInput>();
        }

        private void Update()
        {
            if (playerInputSystem == null)
            {
                playerInputSystem = FindObjectOfType<PlayerInput>();
            }
        }

        public Vector2 GetMoveInput()
        {
            movement = playerInputSystem.actions["Move"].ReadValue<Vector2>();
            return movement;
        }

        public float GetBasicAttackInput()
        {
            basicAttack = playerInputSystem.actions["Basic Attack"].ReadValue<float>();
            return basicAttack;
        }

        public float GetHeavyAttackInput()
        {
            heavyAttack = playerInputSystem.actions["Heavy Attack"].ReadValue<float>();
            return heavyAttack;
        }

        public float GetPauseInput()
        {
            return playerInputSystem.actions["Pause"].ReadValue<float>();
        }
    }
}
