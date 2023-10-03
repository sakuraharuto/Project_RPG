using System.Collections;
using System.Collections.Generic;
using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {   
    
        private Camera _camera;
        private Mover _mover;
        private Fighter _fighter;
        private Health _health;

        // Start is called before the first frame update
        void Start()
        {
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _camera = Camera.main;
            _health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_health.GetIsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            //print("I have nothing to do.");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                
                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;
                
                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(target.gameObject);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point);
                }
                return true;
            }

            return false;
        }

        private Ray GetMouseRay()
        {
            return _camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}

