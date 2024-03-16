using System.Reflection;

namespace JinLei.Extensions;
public static partial class ObjectExtensions
{
    public static TResult Return<TResult>(this object source, TResult result) => result;

    public static TSource Out<TSource>(this TSource source, out TSource result) => result = source;

    public static List<TResult> DoDelegates<TSource, TResult>(this TSource source, IEnumerable<Delegate> delegates, Func<TSource, bool> condition = default) => (delegates?.ForEach(d => new[] { source }.ForEachDoDelegate<TSource, TResult>(d, condition).FirstOrDefault())).ToListOrEmpty();

    public static List<TResult> Do<TSource, TResult>(this TSource source, IEnumerable<Func<TSource, TResult>> delegates, Func<TSource, bool> condition = default) => source.DoDelegates<TSource, TResult>(delegates, condition);

    public static List<TResult> Do<TSource, TResult>(this TSource source, IEnumerable<Func<TResult>> delegates, Func<TSource, bool> condition = default) => source.DoDelegates<TSource, TResult>(delegates, condition);

    public static TSource Do<TSource, TResult>(this TSource source, IEnumerable<Func<TSource, TResult>> delegates, out List<TResult> results, Func<TSource, bool> condition = default) => source.Do(delegates, condition).Out(out results).Return(source);

    public static TSource Do<TSource, TResult>(this TSource source, IEnumerable<Func<TResult>> delegates, out List<TResult> results, Func<TSource, bool> condition = default) => source.Do(delegates, condition).Out(out results).Return(source);

    public static TSource Do<TSource>(this TSource source, IEnumerable<Action<TSource>> delegates, Func<TSource, bool> condition = default) => source.DoDelegates<TSource, object>(delegates, condition).Return(source);

    public static TSource Do<TSource>(this TSource source, IEnumerable<Action> delegates, Func<TSource, bool> condition = default) => source.DoDelegates<TSource, object>(delegates, condition).Return(source);

    #region DoSingleDelegate
    public static TResult Do<TSource, TResult>(this TSource source, Func<TSource, TResult> delegates, Func<TSource, bool> condition = default) => source.Do(new[] { delegates }, condition).FirstOrDefault();

    public static TResult Do<TSource, TResult>(this TSource source, Func<TResult> delegates, Func<TSource, bool> condition = default) => source.Do(new[] { delegates }, condition).FirstOrDefault();

    public static TSource Do<TSource, TResult>(this TSource source, Func<TSource, TResult> delegates, out TResult results, Func<TSource, bool> condition = default) => source.Do(delegates, condition).Out(out results).Return(source);

    public static TSource Do<TSource, TResult>(this TSource source, Func<TResult> delegates, out TResult results, Func<TSource, bool> condition = default) => source.Do(delegates, condition).Out(out results).Return(source);

    public static TSource Do<TSource>(this TSource source, Action<TSource> delegates, Func<TSource, bool> condition = default) => source.Do(new[] { delegates }, condition);

    public static TSource Do<TSource>(this TSource source, Action delegates, Func<TSource, bool> condition = default) => source.Do(new[] { delegates }, condition);
    #endregion

    public static bool Is<TResult>(this object o, out TResult result)
    {
        if(o is TResult value)
        {
            result = value;
            return true;
        }

        result = default;
        return false;
    }

    public static T AsOrDefault<T>(this object o, T defaultValue = default) => o.Is<T>(out var result) ? result : defaultValue;

    public static bool IsNull(this object o) => o == null;

    public static T GetValueOrDefault<T>(this T t, T defaultValue = default) => t ?? defaultValue;

    public static T GetValueInRange<T>(this T t, T range1, T range2) where T : IComparable => t.GetValueInRange(range1, range2, null);

    public static T GetValueInRange<T>(this T t, T range1, T range2, IComparer<T> comparer) => new[] { range1, t, range2 }.OrderBy(t => t, comparer).ElementAt(1);

    #region Clone
    private static MethodInfo MemberwiseCloneMethodInfo { get; } = new Lazy<MethodInfo>(() => typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)).Value;

    public static T MemberwiseClone<T>(T t) => MemberwiseCloneMethodInfo.Invoke(t, default).AsOrDefault<T>();

    public static T Clone<T>(T t)
    {
        var newInstance = MemberwiseClone(t);

        foreach(var f in t.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(t => t.FieldType.IsValueType == false))
        {
            f.SetValue(newInstance, Clone(f.GetValue(t)));
        }

        return newInstance;
    }
    #endregion
}