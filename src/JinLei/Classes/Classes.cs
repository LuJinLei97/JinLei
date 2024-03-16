using JinLei.Extensions;

namespace JinLei.Classes;
public class InvokeResult : Dictionary<string, object>
{
    public InvokeResult(object value = default, bool success = true, Exception exception = default)
    {
        Value = value;
        Success = success;
        Exception = exception;
    }

    public object Value { get => this[nameof(Value)]; set => this[nameof(Value)] = value; }

    public bool Success { get => this[nameof(Success)].AsOrDefault<bool>(true); set => this[nameof(Success)] = value; }

    public Exception Exception { get => this[nameof(Exception)].AsOrDefault<Exception>(); set => this[nameof(Exception)] = value; }
}

public class InvokeResult<TValue> : InvokeResult
{
    public InvokeResult(TValue value = default, bool success = true, Exception exception = default) : base(value, success, exception)
    {
    }

    public new TValue Value { get => base.Value.AsOrDefault<TValue>(); set => base.Value = value; }
}