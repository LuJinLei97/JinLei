using System.Linq.Expressions;
using System.Reflection;

namespace JinLei.Extensions;
public static partial class DelegateExtensions
{
    public static TNewDelegate CreateNewDelegate<TNewDelegate>(this Delegate d, Converter<object[], object[]> paramsConverter = default, Delegate resultConverter = default) where TNewDelegate : Delegate
    {
        try
        {
            if(d.IsNull())
            {
                return default;
            }

            var paramsConverterResult = (params object[] targetParams) => paramsConverter.IsNull() ? targetParams : paramsConverter(targetParams);

            var targetParameterExpressions = typeof(TNewDelegate).GetMethod("Invoke").GetParameters().Select(p => Expression.Parameter(p.ParameterType, $"targetParameter_{p.Name}")).ToArray();
            var sourceParameterExpressions = d.Method.GetParameters().Select(p => Expression.Parameter(p.ParameterType, $"sourceParameter_{p.Name}")).ToArray();

            var sourceParmsAssignExpression = Expression.Assign(Expression.Variable(typeof(object[]), "sourceParms"), Expression.Call(Expression.Constant(paramsConverterResult.Target), paramsConverterResult.Method, Expression.NewArrayInit(typeof(object), targetParameterExpressions.Select(p => Expression.TypeAs(p, typeof(object))))));
            var sourceResultExpression = Expression.Call(Expression.Constant(d.Target), d.Method, sourceParameterExpressions.Select((p, i) => Expression.Unbox(Expression.ArrayIndex(sourceParmsAssignExpression.Left, Expression.Constant(i)), p.Type)));

            var blockVariables = new List<ParameterExpression> { sourceParmsAssignExpression.Left as dynamic };
            var blockExpressions = new List<Expression> { sourceParmsAssignExpression, sourceResultExpression };
            if(resultConverter != null)
            {
                if(d.Method.ReturnType == typeof(void))
                {
                    blockExpressions.Add(Expression.Call(Expression.Constant(resultConverter.Target), resultConverter.Method, Array.Empty<ParameterExpression>()));
                } else
                {
                    var sourceResultAssignExpression = Expression.Assign(Expression.Variable(d.Method.ReturnType), sourceResultExpression);

                    blockVariables.Add(sourceResultAssignExpression.Left as dynamic);
                    blockExpressions[blockExpressions.Count - 1] = sourceResultAssignExpression;
                    blockExpressions.Add(Expression.Call(Expression.Constant(resultConverter.Target), resultConverter.Method, resultConverter.Method.GetParameters().Select((p, i) => i == 0 ? sourceResultAssignExpression.Left : Expression.Default(p.ParameterType)).ToArray()));
                }
            }

            var lambdaExpression = Expression.Lambda<TNewDelegate>(Expression.Block(blockVariables.ToArray(), [.. blockExpressions]), targetParameterExpressions);

            return lambdaExpression.Compile();
        } catch { }

        return default;
    }

    public static TNewDelegate AddParams<TNewDelegate>(this Delegate d, params object[] paramObjects) where TNewDelegate : Delegate => d.CreateNewDelegate<TNewDelegate>((params object[] @params) => @params.Take(@params.Length - paramObjects.Length).ToArray());

    public static TNewDelegate SubParams<TNewDelegate>(this Delegate d, params object[] defalutParamObjects) where TNewDelegate : Delegate => d.CreateNewDelegate<TNewDelegate>((params object[] @params) => @params.Take(@params.Length - defalutParamObjects.Length).Append(defalutParamObjects).ToArray());
}

public static class MethodInfoExtensions
{
    public static TDelegate CreateDelegate<TDelegate>(this MethodInfo methodInfo, object target = default) where TDelegate : Delegate => methodInfo.CreateDelegate(typeof(TDelegate), target) as TDelegate;
}