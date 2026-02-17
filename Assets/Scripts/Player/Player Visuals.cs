using System;
using UnityEngine;
public class PlayerVisuals : MonoBehaviour
{
 [SerializeField] private PlayerModel model;

 private void Awake()
 {
  model = GetComponent<PlayerModel>();
  model.OnJumped += HandleJump;
  model.OnMoveStateChanged += HandleMove;
 }
 private void HandleJump()
 {
  // anim.SetTrigger("Jump");
 }

 private void HandleMove(bool isMoving)
 {
  // anim.SetBool("IsMoving", isMoving);
 }
} 
