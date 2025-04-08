using System;
using Fabric.Api.Parsers;
using Fabric.Api.Services;

namespace Fabric.Api.Ast;

public sealed class FPay : INode, IContract
{
    public FPay(FDate date, FDouble amount, FCurrency currency)
    {
        Date = date;
        Amount = amount;
        Currency = currency;
    }

    public FDate Date { get; }
    public FDouble Amount { get; }
    public FCurrency Currency { get; }

    public T Accept<T>(IContractVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Accept<T, CTX>(IContractVisitor<T, CTX> visitor, CTX ctx)
    {
        return visitor.Visit(this, ctx);
    }
}