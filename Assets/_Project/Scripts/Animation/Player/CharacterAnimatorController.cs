using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class CharacterAnimatorController : MonoBehaviour
{
    // Animator Parameters
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int DirectionX = Animator.StringToHash("DirectionX");
    private static readonly int DirectionY = Animator.StringToHash("DirectionY");
    
    private static readonly int IsBuilding = Animator.StringToHash("IsBuilding");
    private static readonly int IsGathering = Animator.StringToHash("IsGathering");
    
    private static readonly int Building = Animator.StringToHash("BuildStart");
    private static readonly int BuildingEnd = Animator.StringToHash("BuildStop");
    
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;

    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float damping = 0.1f;

    private void Awake()
    {
        animator.applyRootMotion = false;
        agent.updateRotation = true;
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (IsBusy())
        {
            animator.SetFloat(Speed, 0f);
            animator.SetFloat(DirectionX, 0f);
            animator.SetFloat(DirectionY, 0f);
            return;
        }
        
        var velocity = agent.velocity;
        var localVelocity = transform.InverseTransformDirection(velocity);
        
        var speed = velocity.magnitude;
        
        var dirX = localVelocity.x / maxSpeed;
        var dirY = localVelocity.z / maxSpeed;
        
        animator.SetFloat(Speed, speed, damping, Time.deltaTime);
        animator.SetFloat(DirectionX, dirX, damping, Time.deltaTime);
        animator.SetFloat(DirectionY, dirY, damping, Time.deltaTime);

        agent.updateRotation = true;
        agent.angularSpeed = 720f;
    }
    
    private bool IsBusy()
    {
        return animator.GetBool(IsBuilding) || animator.GetBool(IsGathering);
    }
    
    #region Public API

    public void StartBuilding()
    {
        animator.SetBool(IsBuilding, true);
        animator.SetTrigger(Building);
    }

    public void StopBuilding()
    {
        animator.SetTrigger(BuildingEnd);
    }

    public void StartGathering()
    {
        animator.SetBool(IsGathering, true);
    }
    
    public void StopGathering()
    {
        animator.SetBool(IsGathering, false);
    }
    
    #endregion
    
    #region Build Callbacks from animator

    public void OnBuildBegin()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public void OnBuildLoopStart()
    {
        // TODO: add start build process
    }

    public void OnBuildLoopEnd()
    {
        // TODO: cleanup between loops
    }

    public void OnBuildFinished()
    {
        agent.isStopped = false;
        animator.SetBool(IsBuilding, false);
    }
    
    #endregion
    
    #region Gather Callbacks from animator
    
    public void OnGatherStart()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
    
    public void OnGatherEnd()
    {
        agent.isStopped = false;
        animator.SetBool(IsGathering, false);
    }
    
    #endregion
}