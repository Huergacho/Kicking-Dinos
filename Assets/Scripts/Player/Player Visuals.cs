using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private PlayerModel model;
    private Animator _animator;
    bool _isGrounded;

    [SerializeField] private ParticleSystem kickParticles;
    [SerializeField] private ParticleSystem runParticles;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (model == null) model = GetComponentInParent<PlayerModel>();
        model.OnMoveStateChanged += HandleMove;
        model.OnGroundStateChanged += HandleJump;
        model.OnKick += HandleKick;
    }

    private void HandleKick()
    {
        Instantiate(kickParticles, model.attackPoint.position, Quaternion.identity);
        _animator.Play("Player Kick");
    }

    private void HandleJump(bool isGrounded)
    {
        _isGrounded = isGrounded;
        if (!_isGrounded)
        {
            _animator.Play("Player Jump");
            runParticles.Stop();
        }
    }

    private void HandleMove(bool isMoving, float movementDir)
    {
        if (movementDir < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (movementDir > 0) transform.localScale = Vector3.one;
        
        if (isMoving && _isGrounded)
        {
            _animator.Play("Player move");
            if (!runParticles.isPlaying) 
            {
                runParticles.Play();
            }
        }
        else
        {
            if (runParticles.isPlaying)
            {
                runParticles.Stop();
            }
        }
    }
}