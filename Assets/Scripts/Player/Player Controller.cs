using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerModel), typeof(PlayerVisuals))]
    public class Player_Controller : MonoBehaviour
    {
        [SerializeField] private InputActionReference movementAction;
        [SerializeField] private InputActionReference jumpAction;

        private PlayerModel model;

        void Awake()
        {
            model = GetComponent<PlayerModel>();

            movementAction.action.Enable();
            jumpAction.action.Enable();

            jumpAction.action.started += _ => model.RequestJump();
        }

        void Update()
        {
            Vector2 input = movementAction.action.ReadValue<Vector2>();
            model.SetMovement(input);
        }

        private void OnDestroy()
        {
            jumpAction.action.started -= _ => model.RequestJump();
        }
    
    }
    