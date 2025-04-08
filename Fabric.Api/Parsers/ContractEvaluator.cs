using System;
using Fabric.Api.Ast;

namespace Fabric.Api.Parsers;

public class ContractEvaluator : IContractVisitor<double, Dictionary<(DateTime,Ccy),double>>
{
    public double Visit(FAnd and, Dictionary<(DateTime, Ccy), double> ctx)
    {
        return and.Contracts.Sum(x => x.Accept(this, ctx));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pay">A payment contract</param>
    /// <param name="ctx">A dictionary that holds exchange rates to base currency on specific dates</param>
    /// <returns>The value of the payment in base currency</returns>
    /// <exception cref="NotImplementedException"></exception>
    public double Visit(FPay pay, Dictionary<(DateTime, Ccy), double> ctx)
    {
        if (ctx.TryGetValue((pay.Date, pay.Currency), out double rate))
            return rate*pay.Amount;
        throw new KeyNotFoundException($"The required key ({(pay.Date)},({pay.Currency}) was not found in the provided context.");
    }
}
