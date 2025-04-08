using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Fabric.Api.Ast;

namespace Fabric.Api.Parsers;

public class RateFetcher : IContractVisitor<object?, Dictionary<(DateTime, Ccy), double>>
{
    
    public object? Visit(FAnd and, Dictionary<(DateTime, Ccy), double> ctx)
    {
        and.Contracts.Select(x => x.Accept(this,ctx)).ToArray();
        return null;
    }

    public object? Visit(FPay pay, Dictionary<(DateTime, Ccy), double> ctx)
    {
        // For now we always use DKK as base
        var baseRates = new Dictionary<(Ccy, Ccy), double>
        {
            { (Ccy.DKK, Ccy.EUR), 7.50 },
            { (Ccy.EUR, Ccy.DKK), 0.13 },
            { (Ccy.DKK, Ccy.DKK), 1.00 },
            { (Ccy.EUR, Ccy.EUR), 1.00 },
        };

        ctx.TryAdd((pay.Date,pay.Currency),
            GetExchangeRate(pay.Currency,
                            Ccy.DKK,
                            baseRates[(Ccy.DKK, pay.Currency)],
                            pay.Date));
        return null;
    }

    /// <summary>
    /// Dummy random exchange rate generator that is stable wrt dates and currencies
    /// </summary>
    /// <param name="fromCurrency"></param>
    /// <param name="toCurrency"></param>
    /// <param name="baseRate">Today's rate</param>
    /// <param name="date"></param>
    /// <returns></returns>
    public static double GetExchangeRate(Ccy fromCurrency, Ccy toCurrency, double baseRate, DateTime date)
    {
        // Combine inputs into a unique string
        string key = $"{fromCurrency}_{toCurrency}_{date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";

        // Use SHA256 to hash the string deterministically
        using (var sha256 = SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));

            // Convert part of the hash to a number
            int seed = BitConverter.ToInt32(hash, 0);
            var rng = new Random(seed);

            // Generate a rate between 0.5 and 1.5
            return (0.5 + rng.NextDouble()) * baseRate; 
        }
    }
}
