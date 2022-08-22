using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float speed = 1f; // speed at which enemy moves
    Animator animator;
    [SerializeField] bool move=false; // is the enemy moving or not
    Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        UpdateAnimator();
        if (move)
        {
            ChaseBehaviour();
        }
    }
    public void ChaseBehaviour()
    {
        // make the enemy move
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    // update animator for enemy movement
    void UpdateAnimator()
    {
        float chaseSpeed = move ? speed : 0;
        animator.SetFloat("Forward", chaseSpeed, 0.1f, Time.deltaTime);
    }

    // setter for move
    public void SetMove(bool move)
    {
        this.move = move;
    }
}
