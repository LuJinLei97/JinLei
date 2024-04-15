#if NETFRAMEWORK
using System.Activities.Core.Presentation;
using System.Activities.Statements;
using System.ServiceModel.Activities;

namespace JinLei.Classes;

/// <summary>
/// .NET Framework 内置活动库 Factory
/// </summary>
public static class ActivityFactory
{
    #region 基元
    public static Assign Assign() => new();

    public static Assign<T> Assign<T>() => new();

    public static InvokeDelegate InvokeDelegate() => new();

    public static InvokeMethod InvokeMethod() => new();

    public static WriteLine WriteLine() => new();

    public static Delay Delay() => new();
    #endregion

    #region 控制流
    public static If If() => new();

    public static While While() => new();

    public static DoWhile DoWhile() => new();

    public static ForEach<T> ForEach<T>() => new();

    public static Switch<T> Switch<T>() => new();

    public static Sequence Sequence() => new();

    public static System.Activities.Statements.Parallel Parallel() => new();

    public static ParallelForEach<T> ParallelForEach<T>() => new();

    public static Pick Pick() => new();

    public static PickBranch PickBranch() => new();
    #endregion

    #region 集合
    public static AddToCollection<T> AddToCollection<T>() => new();

    public static ClearCollection<T> ClearCollection<T>() => new();

    public static ExistsInCollection<T> ExistsInCollection<T>() => new();

    public static RemoveFromCollection<T> RemoveFromCollection<T>() => new();
    #endregion

    #region 错误处理
    public static Throw Throw() => new();

    public static TryCatch TryCatch() => new();

    public static Rethrow Rethrow() => new();
    #endregion

    #region 流程图
    public static Flowchart Flowchart() => new();

    public static FlowDecision FlowDecision() => new();

    public static FlowSwitch<T> FlowSwitch<T>() => new();
    #endregion

    #region 状态机
    public static State State() => new();

    public static FinalState FinalState() => new();

    public static Transition Transition() => new();

    public static StateMachine StateMachine() => new();
    #endregion

    #region 运行时
    public static Persist Persist() => new();

    public static TerminateWorkflow TerminateWorkflow() => new();

    public static NoPersistScope NoPersistScope() => new();
    #endregion

    #region 事务
    public static CancellationScope CancellationScope() => new();

    public static CompensableActivity CompensableActivity() => new();

    public static Compensate Compensate() => new();

    public static Confirm Confirm() => new();

    public static TransactionScope TransactionScope() => new();

    public static TransactedReceiveScope TransactedReceiveScope() => new();
    #endregion

    #region 迁移
    public static Interop Interop() => new();
    #endregion
}
#endif