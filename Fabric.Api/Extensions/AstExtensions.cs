using System;
using Fabric.Api.Ast;
using Fabric.Api.Parsers;

namespace Fabric.Api.Extensions;

public static class AstExtensions
{
    public static IContract And(this IContract c, params IContract[] cs)
    {
        return new FAnd(Enumerable.Concat(new[] { c }, cs));
    }

    public static IContract Pay(DateTime date, double amount, Ccy ccy)
    {
        return new FPay(date, amount, ccy);
    }

    public static IContract Receive(DateTime date, double amount, Ccy ccy)
    {
        return new FPay(date, -amount, ccy);
    }

    public static IContract FXForward(DateTime date, double amount1, Ccy ccy1, double amount2, Ccy ccy2)
    {
        return Pay(date,amount1, ccy1).And(Receive(date,amount2,ccy2));
    }
}
