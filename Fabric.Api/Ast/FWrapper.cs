using System;
using Fabric.Api.Parsers;

namespace Fabric.Api.Ast;

/// <summary>
/// Immutable wrapper around a value type or immutable reference.
/// Make sure T is immutable.
/// </summary>
public abstract class FWrapper<T> : INode
{
    private readonly T _inner;

    public T Inner => _inner;

    public FWrapper(T inner) => _inner = inner;

    public override string ToString() => Inner?.ToString() ?? "null";

    public override bool Equals(object? obj)
        => obj is FWrapper<T> other && Equals(other);

    public bool Equals(FWrapper<T>? other)
    {
        if (other is null) return false;
        return EqualityComparer<T>.Default.Equals(_inner, other._inner);
    }

    public override int GetHashCode()
        => EqualityComparer<T>.Default.GetHashCode(_inner!);

    public static bool operator ==(FWrapper<T>? left, FWrapper<T>? right)
        => Equals(left, right);

    public static bool operator !=(FWrapper<T>? left, FWrapper<T>? right)
        => !Equals(left, right);
}
