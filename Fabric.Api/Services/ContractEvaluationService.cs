using System;
using Fabric.Api.Parsers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Fabric.Api.Services;

public class ContractEvaluationService
{
    /// <summary>
    /// Evaluate a contract like "And(Pay(date,amount1, ccy1),Receive(date,amount2,ccy2));"
    /// </summary>
    /// <param name="contract"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    internal double Evaluate(string contract)
    {
        var expression = SyntaxFactory.ParseExpression(contract);
        var methodChainVisitor = new MethodChainVisitor();
        var dslRoot = methodChainVisitor.Visit(expression);
        var astRoot = MethodChainVisitor.ToAst(dslRoot);

        if (astRoot is IContract c)
        {
            var exchange_rates = new Dictionary<(DateTime, Ast.Ccy), double>();

            c.Accept(new RateFetcher(), exchange_rates);

            return c.Accept(new ContractEvaluator(), exchange_rates);
        }
        throw new ArgumentException("Provided contract is not a genuine financial contract!");
    }
}
