using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JinLei.Extensions;
public static partial class ObjectExtensions
{
    public static bool IsNull<TSource>(this TSource source) => source is null;

    public static TResult AsOrDefault<TResult>(this object source, TResult defaultValue = default) => source.Is<TResult>(out var result) ? result : defaultValue;

    public static dynamic AsDynamicOrDefault(this object source, dynamic defaultValue = default) => source ?? defaultValue;

    public static TSource GetValueOrDefault<TSource>(this TSource source, TSource defaultValue = default) => source.AsDynamicOrDefault(defaultValue);

    public static TSource GetValueInRange<TSource>(this TSource source, TSource range1, TSource range2) where TSource : IComparable => source.GetValueInRange(range1, range2, null);

    public static TSource GetValueInRange<TSource>(this TSource source, TSource range1, TSource range2, IComparer<TSource> comparer) => new[] { range1, source, range2 }.OrderBy(t => t, comparer).ElementAt(1);

    public static Type GetTypeFromCaller([CallerMemberName] string memberName = default)
    {
        var callerType = new StackFrame(1, false).GetMethod().DeclaringType;
        var members = callerType.GetMember(memberName ?? string.Empty, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(t => t.MemberType is MemberTypes.Field or MemberTypes.Property).ToArray();
        if(members.Length != 0)
        {
            if(members[0] is FieldInfo field)
            {
                return field.FieldType;
            } else if(members[0] is PropertyInfo property)
            {
                return property.PropertyType;
            }
        }

        return callerType;
    }

    #region Clone
    private static MethodInfo MemberwiseCloneMethodInfo { get; } = new Lazy<MethodInfo>(() => typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)).Value;

    public static TSource MemberwiseClone<TSource>(TSource source) => MemberwiseCloneMethodInfo.Invoke(source, default).AsOrDefault<TSource>();

    public static TSource Clone<TSource>(TSource source)
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