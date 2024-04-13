using System.Reflection;

namespace JinLei.Extensions;
public static partial class ObjectExtensions
{
    public static TResult Return<TResult>(this object source, TResult result) => result;

    public static TSource Out<TSource>(this TSource source, out TSource result) => result = source;

    public static TResult Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, Func<TSource, bool> condition = default) => source.DoDelegate<TSource, TResult>(@delegate, condition);

    #region Do
    public static TResult DoDelegate<TSource, TResult>(this TSource source, Delegate @delegate, Delegate condition = default) => new[] { source }.ForEachDoDelegate<TSource, TResult>(@delegate, condition).FirstOrDefault();

    public static object DoDelegate<TSource>(this TSource source, Delegate @delegate, Delegate condition = default) => source.DoDelegate<TSource, object>(@delegate, condition);

    public static TResult Do<TSource, TResult>(this TSource source, Func<TResult> @delegate, Func<TSource, bool> condition = default) => source.DoDelegate<TSource, TResult>(@delegate, condition);

    public static TSource Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, out TResult result, Func<TSource, bool> condition = default) => source.Do(@delegate, condition).Out(out result).Return(source);

    public static TSource Do<TSource, TResult>(this TSource source, Func<TResult> @delegate, out TResult result, Func<TSource, bool> condition = default) => source.Do(@delegate, condition).Out(out result).Return(source);

    public static TSource Do<TSource>(this TSource source, Action<TSource> @delegate, Func<TSource, bool> condition = default) => source.DoDelegate<TSource, object>(@delegate, condition).Return(source);

    public static TSource Do<TSource>(this TSource source, Action @delegate, Func<TSource, bool> condition = default) => source.DoDelegate<TSource, object>(@delegate, condition).Return(source);

    public static TSource DoActions<TSource>(this TSource source, params Action<TSource>[] @delegates) => @delegates.ForEach(d => source.Do(d)).Return(source);

    public static TSource DoActions<TSource>(this TSource source, params Action[] @delegates) => @delegates.ForEach(d => source.Do(d)).Return(source);
    #endregion

    public static bool Is<TResult>(this object source, out TResult result)
    {
        if(source is TResult value)
        {
            result = value;
            return true;
        }

        result = default;
        return false;
    }

    public static bool IsNull<TSource>(this TSource source) => source == null;

    public static TResult AsOrDefault<TResult>(this object source, TResult defaultValue = default) => source.Is<TResult>(out var result) ? result : defaultValue;

    public static TSource GetValueOrDefault<TSource>(this TSource source, TSource defaultValue = default) => source ?? defaultValue;

    public static TSource GetValueInRange<TSource>(this TSource source, TSource range1, TSource range2) where TSource : IComparable => source.GetValueInRange(range1, range2, null);

    public static TSource GetValueInRange<TSource>(this TSource source, TSource range1, TSource range2, IComparer<TSource> comparer) => new[] { range1, source, range2 }.OrderBy(t => t, comparer).ElementAt(1);

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