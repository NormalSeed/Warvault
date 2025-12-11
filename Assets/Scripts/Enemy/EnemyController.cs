using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    SelectorNode rootNode;      // 루트 노드
    SequenceNode attackSeq;     // 공격 시퀀스
    SequenceNode detectSeq;     // 탐지 시퀀스
    ActionNode idleAction;      // 대기 액션
    ActionNode returnAction;    // 귀환 액션

    Transform target;
    public int DetectRange;
    public int AttackRange;
    Vector3 originPos;

    void Start()
    {
        originPos = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            target = player.transform;

        // 노드 생성
        CreateAttackSeq();
        CreateDetectSeq();
        returnAction = new ActionNode(ReturnAction);
        idleAction = new ActionNode(IdleAction);

        // 노드 등록
        rootNode = new SelectorNode();
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
            // y축을 무시한 수평 방향 벡터 계산
            Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 dir = (targetPos - transform.position).normalized;

            transform.forward = dir;
            transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);

            return INode.STATE.RUN;
        }
        
        return INode.STATE.FAILED;
    }
    #endregion

    INode.STATE IdleAction()
    {
        Debug.Log("대기 중");
        return INode.STATE.RUN;
    }

    INode.STATE ReturnAction()
    {
        if (Vector3.Distance(transform.position, originPos) >= 0.1f)
        {
            Debug.Log("복귀 중");

            // y축을 무시한 수평 방향 벡터 계산
            Vector3 originFlat = new Vector3(originPos.x, transform.position.y, originPos.z);
            Vector3 dir = (originFlat - transform.position).normalized;

            transform.forward = dir;
            transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
            return INode.STATE.RUN;
        }
        else
            return INode.STATE.SUCCESS;
    }

    void Update()
    {
        rootNode.Evaluate();
    }
}
