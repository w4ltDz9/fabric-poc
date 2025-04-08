using System;
using Fabric.Api.Parsers;
using Fabric.Api.Services;

namespace Fabric.Api.Ast;

public class FAnd : IContract
{
    public FAnd(IEnumerable<IContract> contracts)
    {
        Contracts = contracts;
    }

    public IEnumerable<IContract> Contracts { get; }

    public T Accept<T>(IContractVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Accept<T, CTX>(IContractVisitor<T, CTX> visitor, CTX ctx)
    {
        return visitor.Visit(this, ctx);
    }
}
