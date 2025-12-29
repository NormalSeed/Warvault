using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class EnemyController : PooledObject
{
    EnemyModel model;

    SelectorNode rootNode;      // 루트 노드
    SequenceNode attackSeq;     // 공격 시퀀스
    SequenceNode detectSeq;     // 탐지 시퀀스
    SequenceNode deadSeq;       // 사망 시퀀스
    ActionNode idleAction;      // 대기 액션
    ActionNode returnAction;    // 귀환 액션

    Transform target;
    public int DetectRange;
    public int AttackRange;
    Vector3 originPos;

    float attackDelay = 0.5f;
    [SerializeField] Transform firePoint1;
    [SerializeField] Transform firePoint2;
    bool isAttack1 = true;

    public bool isDead = false;
    float deadTimer = 1.5f;
    bool isDeadAniPlayed = false;
    bool isAttackable = true;

    Animator animator;
    NavMeshAgent agent;

    readonly int idle = Animator.StringToHash("Idle");
    readonly int walk = Animator.StringToHash("Walk");
    readonly int shootA = Animator.StringToHash("ShootA");
    readonly int shootB = Animator.StringToHash("ShootB");
    readonly int dead = Animator.StringToHash("Dead");
    bool isShootA = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        model = GetComponent<EnemyModel>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            target = player.transform;

        // 노드 생성
        CreateAttackSeq();
        CreateDetectSeq();
        CreateDeadSeq();
        returnAction = new ActionNode(ReturnAction);
        idleAction = new ActionNode(IdleAction);

        // 노드 등록
        rootNode = new SelectorNode();
        rootNode.Add(deadSeq);
        rootNode.Add(attackSeq);
        rootNode.Add(detectSeq);
        rootNode.Add(returnAction);
        rootNode.Add(idleAction);
    }

    #region Attack Sequence
    void CreateAttackSeq()
    {
        attackSeq = new SequenceNode();
        attackSeq.Add(new ActionNode(CheckInAttackRange));
        attackSeq.Add(new ActionNode(Attack));
    }

    INode.STATE CheckInAttackRange()
    {
        // 타겟이 없으면 실패 반환
        if (target == null)
            return INode.STATE.FAILED;

        // 타겟과 자신의 거리가 공격 사거리보다 작으면 성공 반환
        if (Vector3.Distance(transform.position, target.position) < AttackRange)
        {
            Debug.Log("공격 범위 안에 들어옴");
            return INode.STATE.SUCCESS;
        }

        // 그외의 경우엔 실패 반환
        return INode.STATE.FAILED;
    }

    INode.STATE Attack()
    {
        Debug.Log("공격 중");

        agent.isStopped = true;

        // 공격 애니메이션
        if (isShootA)
        {
            animator.Play(shootA);
            isShootA = false;
        }
        else
        {
            animator.Play(shootB);
            isShootA = true;
        }

        // 공격 로직
        if (attackDelay <= 0f)
        {
            if (isAttack1)
            {
                PoolManager.Instance.SpawnFromPool("TestEnemyBullet1", firePoint1.position, firePoint1.rotation);
            }
            else
            {
                PoolManager.Instance.SpawnFromPool("TestEnemyBullet2", firePoint2.position, firePoint2.rotation);
            }

            isAttack1 = !isAttack1;
            attackDelay = 0.5f;
        }

        return INode.STATE.RUN;
    }
    #endregion

    #region Detect Sequence
    void CreateDetectSeq()
    {
        detectSeq = new SequenceNode();
        detectSeq.Add(new ActionNode(CheckInDetectRange));
        detectSeq.Add(new ActionNode(Trace));
    }

    INode.STATE CheckInDetectRange()
    {
        if (target == null)
            return INode.STATE.FAILED;

        if (Vector3.Distance(transform.position, target.position) < DetectRange)
        {
            Debug.Log("플레이어 탐지됨");
            return INode.STATE.SUCCESS;
        }

        return INode.STATE.FAILED;
    }

    INode.STATE Trace()
    {
        if (Vector3.Distance(transform.position, target.position) > AttackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);

            if (agent.velocity.magnitude > 0.1f)
                animator.Play(walk);

            return INode.STATE.RUN;
        }
        
        return INode.STATE.FAILED;
    }
    #endregion

    INode.STATE IdleAction()
    {
        Debug.Log("대기 중");

        agent.isStopped = true;
        animator.Play(idle);

        return INode.STATE.RUN;
    }

    INode.STATE ReturnAction()
    {
        if (Vector3.Distance(transform.position, originPos) >= 0.1f)
        {
            Debug.Log("복귀 중");

            agent.isStopped = false;
            agent.SetDestination(originPos);

            if (agent.velocity.magnitude > 0.1f)
                animator.Play(walk);

            return INode.STATE.RUN;
        }
        else
            return INode.STATE.SUCCESS;
    }

    #region Dead Sequence
    void CreateDeadSeq()
    {
        deadSeq = new SequenceNode();
        deadSeq.Add(new ActionNode(CheckDead));
        deadSeq.Add(new ActionNode(PlayDeadAnimation));
        deadSeq.Add(new ActionNode(ReturnToPool));
    }

    INode.STATE CheckDead()
    {
        return isDead ? INode.STATE.SUCCESS : INode.STATE.FAILED;
    }

    INode.STATE PlayDeadAnimation()
    {
        if (!isDeadAniPlayed)
        {
            animator.Play(dead);
            isAttackable = false;
            isDeadAniPlayed = true;
            deadTimer = 1.5f;
            agent.isStopped = true;
        }

        if (deadTimer > 0f)
        {
            deadTimer -= Time.deltaTime;
            return INode.STATE.RUN;
        }

        return INode.STATE.SUCCESS;
    }

    INode.STATE ReturnToPool()
    {
        ReturnPool();
        OnDespawn();
        return INode.STATE.SUCCESS;
    }
    #endregion

    void Update()
    {
        if (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
        }

        rootNode.Evaluate();
    }

    public override void OnSpawn()
    {
        isDead = false;
        isDeadAniPlayed = false;
        isAttackable = true;
        animator.enabled = true;
        agent.isStopped = false;
        agent.speed = 1f;
        agent.angularSpeed = 300f;

        originPos = transform.position;

        model.CurHp.Value = model.Hp;
    }

    public override void OnDespawn()
    {
        animator.enabled = false;
        agent.isStopped = true;
        SpawnManager.Instance.DecreaseCount();
    }

    public void TakeDamage(int amount)
    {
        model.CurHp.Value = Mathf.Max(model.CurHp.Value - amount, 0);
        if (model.CurHp.Value == 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        isDead = true;
        GameManager.Instance.AddScore(model.Score);
    }
}
