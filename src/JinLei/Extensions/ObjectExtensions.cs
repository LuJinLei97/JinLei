using System.Reflection;

namespace JinLei.Extensions;

public static partial class ObjectExtensions
{
    public static bool IsNull<TSource>(this TSource source) => source is null;

    public static bool Is<TResult>(this object source, out TResult result) => (source is TResult value ? (Is: true, Value: value) : (false, default)).Do(t => t.Value, out result).Do(t => t.Is);

    public static TResult AsOrDefault<TResult>(this object source, TResult defaultValue = default) => source.Is<TResult>(out var result) ? result : defaultValue;

    public static dynamic AsDynamicOrDefault(this object source, object defaultValue = default) => source.AsOrDefault(defaultValue);

    public static TSource GetValueOrDefault<TSource>(this TSource source, TSource defaultValue = default) => source.AsOrDefault(defaultValue);

    #region Clone
    private static MethodInfo MemberwiseCloneMethodInfo { get; } = new Lazy<MethodInfo>(() => typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)).Value;

    public static TSource MemberwiseClone<TSource>(TSource source) => MemberwiseCloneMethodInfo.Invoke(source, default).AsOrDefault<TSource>();

    internal static TSource Clone<TSource>(TSource source)
    {
        var newInstance = MemberwiseClone(source);

        foreach(var f in source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(t => t.FieldType.IsValueType == false))
        {
            f.SetValue(newInstance, Clone(f.GetValue(source)));
        }

        return newInstance;
    }
    #endregion
}