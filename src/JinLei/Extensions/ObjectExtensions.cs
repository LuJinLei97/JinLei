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