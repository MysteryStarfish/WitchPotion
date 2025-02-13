using System.Linq;
using UnityEngine;

namespace Map
{
    public enum NodeActionType
    {
        NEXTNODE_0,
        NEXTNODE_1,
        NEXTNODE_2,
        COLLECTION,
        RECTIFICATION,
        GOAL
    }
    public class NodeAction<T>
    {
        public NodeActionType ActionType { get; private set; }
        public T[] Conditions { get; private set; }
        private readonly int _useLeftTime;
        private int _currentUseLeftTime;
        public bool Locked = false;
        public NodeAction(NodeActionType actionType, T[] conditions, int useLeftTime)
        {
            ActionType = actionType;
            Conditions = conditions;
            _useLeftTime = useLeftTime;
            _currentUseLeftTime = useLeftTime;
        }

        private void LockAction()
        {
            Locked = true;
        }
        private void UnlockAction()
        {
            Locked = false;
        }

        public void AddCondition(T condition)
        {
            Conditions = Conditions.Append(condition).ToArray();
        }

        public void UsedAction()
        {
            Debug.Log(_currentUseLeftTime);
            _currentUseLeftTime -= 1;
            if (_currentUseLeftTime <= 0) LockAction();
        }

        public void ResetUsedTimes()
        {
            _currentUseLeftTime = _useLeftTime;
            if (Conditions.Length == 0) UnlockAction();
        }
    }
}