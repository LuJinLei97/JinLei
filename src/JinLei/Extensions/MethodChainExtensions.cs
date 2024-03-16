namespace JinLei.Extensions;
public static partial class MethodChainExtensions
{
    public static TResult Return<TResult>(this object source, TResult result) => result;

    public static TSource Out<TSource>(this TSource source, out TSource result) => result = source;

    public static TResult Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, Func<TSource, bool> condition = default) => source.Do<TSource, TResult>(@delegate as Delegate, condition);
    #region Do
    public static TResult Do<TSource, TResult>(this TSource source, Delegate @delegate, Delegate condition = default) => new[] { source }.ForEachIterator<TSource, TResult>(@delegate, condition).FirstOrDefault();

    public static TSource Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, out TResult result, Func<TSource, bool> condition = default) => source.Do(@delegate, condition).Out(out result).Return(source);

    public static TSource Do<TSource>(this TSource source, Action<TSource> @delegate, Func<TSource, bool> condition = default) => source.Do<TSource, object>(@delegate, condition).Return(source);
    #endregion

    public static bool Is<TResult>(this object source, out TResult result) => (source is TResult value ? (Is: true, Value: value) : (false, default)).Do(t => t.Value, out result).Is;
}