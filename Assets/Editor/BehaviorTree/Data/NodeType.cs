public enum NodeType
{
    BehaviourNode = 1,
    DecoratorNode,
    ConditionNode,
    ConditionWaitNode,
    ActionNode,
    WaitNode,
    SequenceNode,
    SelectorNode,
    NotDecorator,
    FailIfRunningDecorator,
    RunningIfFailDecorator,
    LoopNode,
    RandomNode,
    PriorityNode,
    ParallelNode,
    ParallelNodeAny,
    EventNode,
    WhileNode,
    IfNode,
    WeightSelectNode
}