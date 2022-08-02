using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Animator animator;
    [SerializeField] bool move=false;
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
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
    void UpdateAnimator()
    {
        float chaseSpeed = move ? speed : 0;
        animator.SetFloat("Forward", chaseSpeed, 0.1f, Time.deltaTime);
    }
    public void SetMove(bool move)
    {
        this.move = move;
    }
}
