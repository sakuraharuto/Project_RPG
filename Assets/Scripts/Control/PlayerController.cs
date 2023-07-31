using System.Collections;
using System.Collections.Generic;
using Combat;
using Movement;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {   
    
        private Camera _camera;
        private Mover _mover;
        private Fighter _fighter;

        // Start is called before the first frame update
        void Start()
        {
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            InteractWithCombat();
            InteractWithMovement();
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    _fighter.Attack(target);
                }
            }
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
            {   
                _mover.MoveTo(hit.point);
            }
        }

        private Ray GetMouseRay()
        {
            return _camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}

