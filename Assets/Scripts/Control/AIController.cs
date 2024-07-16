using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attribute;
using GameDevTV.Utils;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;
    [SerializeField] float dwellingTime = 5f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float wayPointTolerance = 1f;
    [Range(0, 1)]
    [SerializeField] float speedFraction = 0.2f;

    float timeSinceLastSawPlayer=Mathf.Infinity;
    float timeSinceReachedWayPoint = Mathf.Infinity;

    Fighter fighter;
    GameObject player;
    Health health;
    Mover mover;

    int currentWayPointIndex=0;
    LazyValue<Vector3> guardPosition;

    private void Awake()
    {
        guardPosition = new LazyValue<Vector3>(GetGuardPosition);
    }

    private void Start()
    {
        fighter = GetComponent<Fighter>();
        mover = GetComponent<Mover>();
        health = GetComponent<Health>();
        player = GameObject.FindWithTag("Player");
    }

    private Vector3 GetGuardPosition()
    {
        return transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (health.IsDead()) return;

        if(InAttackRangeOfPlayer()&&fighter.CanAttak(player))
        {
            AttackBehaviour();
        }

        else if(timeSinceLastSawPlayer<suspicionTime)
        {
            if (gameObject.tag != "Enemy") return;

            SuspicionBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        UpdateTimers();

    }

    bool InAttackRangeOfPlayer()
    {
        float distanceToPlayer=Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer < chaseDistance;
    }

    void AttackBehaviour()
    {
        timeSinceLastSawPlayer = 0f;
        fighter.Attack(player);
    }

    void SuspicionBehaviour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition.value;
        if(patrolPath!=null)
        {
            if(AtWayPoint())
            {
                    timeSinceReachedWayPoint = 0;
                    CycleWayPoint();
            }
            nextPosition = GetCurrentWayPoint();
        }

        if(timeSinceReachedWayPoint>dwellingTime)
        {
            mover.StartMoveAction(nextPosition, speedFraction);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
    
    bool AtWayPoint()
    {
        float distanceFromWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        return distanceFromWayPoint < wayPointTolerance;
    }

    void CycleWayPoint()
    {
        currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
    }

    Vector3 GetCurrentWayPoint()
    {
        return patrolPath.GetWayPoint(currentWayPointIndex);
    }

    void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceReachedWayPoint += Time.deltaTime;
    }
}
