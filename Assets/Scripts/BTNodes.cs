using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface INode
{
    public enum STATE
    { RUN, SUCCESS, FAILED }

    public INode.STATE Evaluate();
}

public class ActionNode : INode
{
    public Func<INode.STATE> action; // 반환형이 INode.STATE인 대리자

    public ActionNode(Func<INode.STATE> action) // 노드를 생성할 때 매개변수로 대리자를 받음
    {
        this.action = action;
    }

    public INode.STATE Evaluate()
    {
        return action?.Invoke() ?? INode.STATE.FAILED;
    }
}

public class SelectorNode : INode
{
    private List<INode> children;

    public SelectorNode() { children = new List<INode>(); }

    public void Add(INode node) { children.Add(node); } // 셀렉터에 자식노드 추가

    public INode.STATE Evaluate()
    {
        foreach (INode child in children)
        {
            INode.STATE state = child.Evaluate();

            // children 안의 child 노드들 중 하나라도 SUCCESS면 성공을 반환
            // RUN 상태면 RUN 반환
            switch (state)
            {
                case INode.STATE.SUCCESS:
                    return INode.STATE.SUCCESS;
                case INode.STATE.RUN:
                    return INode.STATE.RUN;
            }
        }

        // foreach문이 끝났는데도 SUCCESS가 반환되지 않았으면 자식노드가 모두 FAILED라는 소리이니 FAILED 반환
        return INode.STATE.FAILED;
    }
}

public class SequenceNode : INode
{
    public List<INode> children;

    public SequenceNode() { children = new List<INode>(); }

    public void Add(INode node) { children.Add(node); }

    public INode.STATE Evaluate()
    {
        if (children.Count <= 0)
            return INode.STATE.FAILED;

        foreach (INode child in children)
        {
            switch (child.Evaluate())
            {
                // RUN 상태면 RUN 반환
                case INode.STATE.RUN:
                    return INode.STATE.RUN;
                // SUCCESS면 반복문 재호출
                case INode.STATE.SUCCESS:
                    continue;
                // FAILED면 FAILED 반환
                case INode.STATE.FAILED:
                    return INode.STATE.FAILED;
            }
        }

        // child가 모두 SUCCESS일 경우에만 SUCCESS 반환
        return INode.STATE.SUCCESS;
    }
}