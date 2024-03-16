using JinLei.Extensions;

namespace JinLei.Classes;
public class InvokeResult
{
    public InvokeResult(object value = default, bool success = true, Exception exception = default)
    {
        Value = value;
        Success = success;
        Exception = exception;
    }

    public virtual object Value { get => Properties[nameof(Value)]; set => Properties[nameof(Value)] = value; }

    public virtual bool Success { get => Properties[nameof(Success)].AsOrDefault<bool>(true); set => Properties[nameof(Success)] = value; }

    public virtual Exception Exception { get => Properties[nameof(Exception)].AsOrDefault<Exception>(); set => Properties[nameof(Exception)] = value; }

    public virtual Dictionary<string, object> Properties { get; protected set; } = [];
}

public class InvokeResult<TValue> : InvokeResult
{
    public InvokeResult(TValue value = default, bool success = true, Exception exception = default) : base(value, success, exception)
    {
    }

    public new virtual TValue Value { get => base.Value.AsOrDefault<TValue>(); set => base.Value = value; }
}