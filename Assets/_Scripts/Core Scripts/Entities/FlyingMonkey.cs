using System.Collections;
using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Behaviors;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using MonkeyJam._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Entities
{
    public class FlyingMonkey : Core
    {
        [Header("Data")] 
        public EntityData Data;

        [Header("Behaviors")] 
        public FlingItem FlingItem;
        public IdleState Idle;
        public HurtState Hurt;
        public DeathState Dead;

        void Start()
        {
            Data = GetComponent<EntityData>();

            // Set up the instances of the Entity
            SetUpInstances();
            
            // Set machine state.
            Machine = new Machine();
            Machine.Set(FlingItem);
        }

        void Update()
        {
            if (Data.IsEntityDead())
            {
                // Set the death animation for the entity 
                Machine.Set(Dead);
                return;
            }

            // Keep looking if there is a target
            FlingItem.FindTarget();

            SelectState();
            Machine.CurrentState.Do();
        }

        private void SelectState()
        {
            if (Data.IsTakingDamage)
            {
                Machine.Set(Hurt, true);
                StartCoroutine(ResetPain());
            }
            else if (FlingItem.HasTarget)
            {
                Machine.Set(FlingItem, true);
            }
            else
            {
                Machine.Set(Idle, true);
            }
        }

        IEnumerator ResetPain()
        {
            yield return new WaitForSeconds(0.5f);
            Data.ResetIsTakingDamage();
        }
    }
}