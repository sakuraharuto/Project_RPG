using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Core;
using UnityEngine;
using Movement;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttack = 1f;
        [SerializeField] private float weaponDamage = 5f;
        private Transform _target;
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private float timeSinceLastAttack = 0;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttack)
            {   
                // This will trigger the Hit() event
                GetComponent<Animator>().SetTrigger(Attack1);
                timeSinceLastAttack = 0;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {   
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.transform;
            print("Come get some");
        }

        public void Cancel()
        {
            _target = null;
        }
        
        // Animation Event
        void Hit()
        {
            Health healthComp = _target.GetComponent<Health>();
            healthComp.TakeDamage(weaponDamage);
        }
    }
}

