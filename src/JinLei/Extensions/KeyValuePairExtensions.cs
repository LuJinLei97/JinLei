#if NETFRAMEWORK
namespace JinLei.Extensions;

public static partial class KeyValuePair
{
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) => new(key, value);
}
#endif