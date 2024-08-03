using JinLei.Extensions;

namespace JinLei.Classes;

/// <inheritdoc cref="IComparer{T}"/>
public partial class CommonComparer<T> : IComparer<T>, IEqualityComparer<T>
{
    /// <inheritdoc/>
    public virtual int Compare(T x, T y) => Comparison(x, y);
    /// <inheritdoc/>

    public virtual bool Equals(T x, T y) => equalityComparison.IsNull() == false || comparison.IsNull() ? EqualityComparison(x, y) : Comparison(x, y) == 0;
    /// <inheritdoc/>

    public virtual int GetHashCode(T x) => GetHashCodeFunc(x);

    /// <inheritdoc cref="Compare(T, T)"/>
    public virtual Comparison<T> Comparison
    {
        get => comparison ?? Comparer<T>.Default.Compare;
        set => comparison = value;
    }
    protected Comparison<T> comparison;

    /// <inheritdoc cref="Equals(T, T)"/>
    public virtual Func<T, T, bool> EqualityComparison
    {
        get => equalityComparison ?? EqualityComparer<T>.Default.Equals;
        set => equalityComparison = value;
    }
    protected Func<T, T, bool> equalityComparison;

    /// <inheritdoc cref="GetHashCode(T)"/>
    public virtual Converter<T, int> GetHashCodeFunc
    {
        get => getHashCodeFunc ?? EqualityComparer<T>.Default.GetHashCode;
        set => getHashCodeFunc = value;
    }
    protected Converter<T, int> getHashCodeFunc;
}