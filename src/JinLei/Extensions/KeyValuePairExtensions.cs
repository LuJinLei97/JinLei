#if NETFRAMEWORK
namespace JinLei.Extensions;
public static class KeyValuePair
{
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) => new(key, value);
}
#endif