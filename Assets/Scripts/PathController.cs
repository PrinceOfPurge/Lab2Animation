using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{

    [SerializeField] 
    public PathManager PathManager;

    List<Waypoint> thePath;
    Waypoint Target;

    public float MoveSpeed;
    public float RotateSpeed;

    public Animator animator;
    bool isWalking;
    bool Collided;
    
    private void Start()
    {
        thePath = PathManager.GetPath();
        if (thePath != null && thePath.Count>0)
        {
            Target = thePath[0];
        }
        
        isWalking = false;
        animator.SetBool("isWalking", isWalking);
        Collided = false;
        animator.SetBool("collided", Collided);
        
        
    }

    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = Target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void moveForward()
    {
      
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, Target.pos);
        
        if (distanceToTarget < stepSize)
        {
            //we will overshoot the target
            //so we should do something smarter here
            return;
        }
        //take a step forward
        Vector3 moveDir = Vector3.forward;
        Debug.Log(moveDir * stepSize);
        transform.Translate(moveDir * stepSize);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
                //toggle if any key pressed
                isWalking = !isWalking;
                animator.SetBool("isWalking", isWalking);
        }

        if (isWalking)
        {
            rotateTowardsTarget();
            moveForward();
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Target = PathManager.GetNextTarget();
        isWalking = false;
        animator.SetBool("isWalking", false);
        //animator.SetBool("collided", true);
        Debug.Log($"{gameObject.name} collided with {other.gameObject.name} and stopped.");
        
    }
    
    
}
