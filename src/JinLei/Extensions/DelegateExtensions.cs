using System.Linq.Expressions;
using System.Reflection;

namespace JinLei.Extensions;
public static partial class DelegateExtensions
{
    public static TDelegate ToTDelegate<TDelegate>(this Delegate d, Converter<object[], object[]> paramsConverter = default, Delegate resultConverter = default) where TDelegate : Delegate
    {
        try
        {
            if(d.IsNull())
            {
                return default;
            }

            var paramsConverterResult = (params object[] targetParams) => paramsConverter.IsNull() ? targetParams : paramsConverter(targetParams);

            var targetInvokeInfo = typeof(TDelegate).GetMethod("Invoke");

            var targetParameterExpressions = targetInvokeInfo.GetParameters().Select(p => Expression.Parameter(p.ParameterType, $"targetParameter_{p.Name}")).ToArray();
            var sourceParameterExpressions = d.GetType().GetMethod("Invoke").GetParameters().Select(p => Expression.Parameter(p.ParameterType, $"sourceParameter_{p.Name}")).ToArray();

            var sourceParmsAssignExpression = Expression.Assign(Expression.Variable(typeof(object[])), Expression.Invoke(Expression.Constant(paramsConverterResult), Expression.NewArrayInit(typeof(object), targetParameterExpressions.Select(p => Expression.Convert(p, typeof(object))))));
            var sourceResultExpression = Expression.Invoke(Expression.Constant(d), sourceParameterExpressions.Select((p, i) => Expression.Convert(Expression.ArrayIndex(sourceParmsAssignExpression.Left, Expression.Constant(i)), p.Type)));

            var blockVariables = new List<ParameterExpression> { sourceParmsAssignExpression.Left as ParameterExpression };
            var blockExpressions = new List<Expression> { sourceParmsAssignExpression, sourceResultExpression };
            if(resultConverter.IsNull() == false)
            {
                if(d.Method.ReturnType == typeof(void))
                {
                    blockExpressions.Add(Expression.Empty());
                    blockExpressions.Add(Expression.Invoke(Expression.Constant(resultConverter)));
                } else
                {
                    blockExpressions[blockExpressions.Count - 1] = Expression.Convert(Expression.Invoke(Expression.Constant(resultConverter), sourceResultExpression), targetInvokeInfo.ReturnType);
                }
            }

            var lambdaExpression = Expression.Lambda<TDelegate>(Expression.Block([.. blockVariables], [.. blockExpressions]), targetParameterExpressions);

            return lambdaExpression.Compile();
        } catch { }

        return default;
    }

    public static TDelegate AddParams<TDelegate>(this Delegate d, params object[] paramObjects) where TDelegate : Delegate => d.ToTDelegate<TDelegate>((object[] @params) => @params.Take(@params.Length - paramObjects.Length).ToArray());

    public static TDelegate SubParams<TDelegate>(this Delegate d, params object[] defalutParamObjects) where TDelegate : Delegate => d.ToTDelegate<TDelegate>((object[] @params) => @params.Take(@params.Length - defalutParamObjects.Length).Append(defalutParamObjects).ToArray());

    public static TFunc ToFunc<TDelegate, TFunc, TResult>(this TDelegate d, TResult result = default) where TDelegate : Delegate where TFunc : Delegate => d.ToTDelegate<TFunc>(resultConverter: () => result);

    public static TAction ToAction<TDelegate, TAction>(this TDelegate d) where TDelegate : Delegate where TAction : Delegate => d.ToTDelegate<TAction>(resultConverter: (object result) => { });
}

public static class MethodInfoExtensions
{
    public static TDelegate CreateDelegate<TDelegate>(this MethodInfo methodInfo, object target = default) where TDelegate : Delegate => methodInfo.CreateDelegate(typeof(TDelegate), target) as TDelegate;
}