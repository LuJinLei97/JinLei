namespace JinLei.Extensions;
public static partial class DelegateExtensions
{
    #region ToAction
    public static Action ToAction<TResult>(this Func<TResult> func) => func.IsNull() ? default : () => func();

    public static Action<T1> ToAction<T1, TResult>(this Func<T1, TResult> func) => func.IsNull() ? default : (t1) => func(t1);

    public static Action<T1, T2> ToAction<T1, T2, TResult>(this Func<T1, T2, TResult> func) => func.IsNull() ? default : (t1, t2) => func(t1, t2);

    public static Action<T1, T2, T3> ToAction<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func) => func.IsNull() ? default : (t1, t2, t3) => func(t1, t2, t3);

    public static Action<T1, T2, T3, T4> ToAction<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4) => func(t1, t2, t3, t4);

    public static Action<T1, T2, T3, T4, T5> ToAction<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5) => func(t1, t2, t3, t4, t5);

    public static Action<T1, T2, T3, T4, T5, T6> ToAction<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6) => func(t1, t2, t3, t4, t5, t6);

    public static Action<T1, T2, T3, T4, T5, T6, T7> ToAction<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7) => func(t1, t2, t3, t4, t5, t6, t7);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8) => func(t1, t2, t3, t4, t5, t6, t7, t8);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
    #endregion

    #region FuncAddParam
    public static Func<TParam, TResult> AddParam<TParam, TResult>(this Func<TResult> func, TParam param) => func.IsNull() ? default : (p) => func();

    public static Func<T1, TParam, TResult> AddParam<T1, TParam, TResult>(this Func<T1, TResult> func, TParam param) => func.IsNull() ? default : (t1, p) => func(t1);

    public static Func<T1, T2, TParam, TResult> AddParam<T1, T2, TParam, TResult>(this Func<T1, T2, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, p) => func(t1, t2);

    public static Func<T1, T2, T3, TParam, TResult> AddParam<T1, T2, T3, TParam, TResult>(this Func<T1, T2, T3, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, p) => func(t1, t2, t3);

    public static Func<T1, T2, T3, T4, TParam, TResult> AddParam<T1, T2, T3, T4, TParam, TResult>(this Func<T1, T2, T3, T4, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, p) => func(t1, t2, t3, t4);

    public static Func<T1, T2, T3, T4, T5, TParam, TResult> AddParam<T1, T2, T3, T4, T5, TParam, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, p) => func(t1, t2, t3, t4, t5);

    public static Func<T1, T2, T3, T4, T5, T6, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, p) => func(t1, t2, t3, t4, t5, t6);

    public static Func<T1, T2, T3, T4, T5, T6, T7, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, p) => func(t1, t2, t3, t4, t5, t6, t7);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, p) => func(t1, t2, t3, t4, t5, t6, t7, t8);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, p) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, p) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, p) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, p) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, p) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, p) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TParam, TResult> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TParam, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, TParam param) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, p) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
    #endregion

    #region FuncSubParam
    public static Func<TResult> SubParam<T1, TResult>(this Func<T1, TResult> func, T1 defaultParam = default) => func.IsNull() ? default : () => func(defaultParam);

    public static Func<T1, TResult> SubParam<T1, T2, TResult>(this Func<T1, T2, TResult> func, T2 defaultParam = default) => func.IsNull() ? default : (t1) => func(t1, defaultParam);

    public static Func<T1, T2, TResult> SubParam<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T3 defaultParam = default) => func.IsNull() ? default : (t1, t2) => func(t1, t2, defaultParam);

    public static Func<T1, T2, T3, TResult> SubParam<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T4 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3) => func(t1, t2, t3, defaultParam);

    public static Func<T1, T2, T3, T4, TResult> SubParam<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T5 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4) => func(t1, t2, t3, t4, defaultParam);

    public static Func<T1, T2, T3, T4, T5, TResult> SubParam<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T6 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5) => func(t1, t2, t3, t4, t5, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T7 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6) => func(t1, t2, t3, t4, t5, t6, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T8 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7) => func(t1, t2, t3, t4, t5, t6, t7, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T9 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8) => func(t1, t2, t3, t4, t5, t6, t7, t8, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T10 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T11 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T12 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T13 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T14 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T15 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, defaultParam);

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T16 defaultParam = default) => func.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, defaultParam);
    #endregion

    #region ToFunc
    public static Func<TResult> ToFunc<TResult>(this Action action, TResult result = default) => action.IsNull() ? default : () => { action(); return result; };

    public static Func<T1, TResult> ToFunc<T1, TResult>(this Action<T1> action, TResult result = default) => action.IsNull() ? default : (t1) => { action(t1); return result; };

    public static Func<T1, T2, TResult> ToFunc<T1, T2, TResult>(this Action<T1, T2> action, TResult result = default) => action.IsNull() ? default : (t1, t2) => { action(t1, t2); return result; };

    public static Func<T1, T2, T3, TResult> ToFunc<T1, T2, T3, TResult>(this Action<T1, T2, T3> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3) => { action(t1, t2, t3); return result; };

    public static Func<T1, T2, T3, T4, TResult> ToFunc<T1, T2, T3, T4, TResult>(this Action<T1, T2, T3, T4> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4) => { action(t1, t2, t3, t4); return result; };

    public static Func<T1, T2, T3, T4, T5, TResult> ToFunc<T1, T2, T3, T4, T5, TResult>(this Action<T1, T2, T3, T4, T5> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5) => { action(t1, t2, t3, t4, t5); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, TResult> ToFunc<T1, T2, T3, T4, T5, T6, TResult>(this Action<T1, T2, T3, T4, T5, T6> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6) => { action(t1, t2, t3, t4, t5, t6); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7) => { action(t1, t2, t3, t4, t5, t6, t7); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8) => { action(t1, t2, t3, t4, t5, t6, t7, t8); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15); return result; };

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, TResult result = default) => action.IsNull() ? default : (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16); return result; };
    #endregion

    #region ActionAddParam
    public static Action<TParam> AddParam<TParam>(this Action action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, TParam> AddParam<T1, TParam>(this Action<T1> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, TParam> AddParam<T1, T2, TParam>(this Action<T1, T2> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, TParam> AddParam<T1, T2, T3, TParam>(this Action<T1, T2, T3> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, TParam> AddParam<T1, T2, T3, T4, TParam>(this Action<T1, T2, T3, T4> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, TParam> AddParam<T1, T2, T3, T4, T5, TParam>(this Action<T1, T2, T3, T4, T5> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, TParam> AddParam<T1, T2, T3, T4, T5, T6, TParam>(this Action<T1, T2, T3, T4, T5, T6> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TParam> AddParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TParam>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, TParam param) => action.ToFunc(true).AddParam(param).ToAction();
    #endregion

    #region ActionSubParam
    public static Action SubParam<T1>(this Action<T1> action, T1 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1> SubParam<T1, T2>(this Action<T1, T2> action, T2 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2> SubParam<T1, T2, T3>(this Action<T1, T2, T3> action, T3 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3> SubParam<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T4 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4> SubParam<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T5 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5> SubParam<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T6 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6> SubParam<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T7 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7> SubParam<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T8 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T9 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T10 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T11 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T12 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T13 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T14 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T15 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> SubParam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T16 defaultParam = default) => action.ToFunc(true).SubParam(defaultParam).ToAction();
    #endregion
}