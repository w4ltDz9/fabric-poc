using System;
using System.Globalization;
using Fabric.Api.Ast;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Fabric.Api.Parsers;

public class MethodChainVisitor : CSharpSyntaxVisitor<MethodChainVisitor.DslNode>
{
    public abstract record DslNode;
    public record Call(string Name, List<DslNode> Args) : DslNode;
    public record Leaf(string Data) : DslNode;
    public override DslNode VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var args = new List<DslNode>();

        foreach (var arg in node.ArgumentList.Arguments)
        {
            var argNode = arg.Expression.Accept(this);
            args.Add(argNode);
        }

        if (node.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            // Handle chained call: x.Method(y)
            var target = memberAccess.Expression.Accept(this);
            args.Insert(0, target); // first arg is receiver
            return new Call(memberAccess.Name.Identifier.Text, args);
        }
        else if (node.Expression is IdentifierNameSyntax id)
        {
            // Simple function call: Func(args)
            return new Call(id.Identifier.Text, args);
        }

        throw new NotSupportedException($"Unsupported node: {node.Expression.Kind()}");
    }

    public override DslNode VisitIdentifierName(IdentifierNameSyntax node)
        => new Leaf(node.Identifier.Text);

    public override DslNode VisitLiteralExpression(LiteralExpressionSyntax node)
        => new Leaf(node.ToString());

    public static void ToConsole(DslNode node, int indent = 0)
    {
        var pad = new string(' ', indent * 2);

        if (node is Call call)
        {
            Console.WriteLine($"{pad}Call: {call.Name}");
            foreach (var arg in call.Args)
            {
                ToConsole(arg, indent + 1);
            }
        }
    }

    public static INode ToAst(DslNode node)
    {
        if (node is Leaf leaf)
        {
            if (Double.TryParse(leaf.Data, out double result_double))
                return new FDouble(result_double);
            else if (DateTime.TryParseExact(leaf.Data.Trim().Trim('"'),
                                            "yyyy-MM-dd",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out DateTime result_datetime))
                return new FDate(result_datetime);
            else if (Enum.TryParse<Ccy>(leaf.Data.Trim().Trim('"'), out Ccy result_ccy))
                return new FCurrency(result_ccy);
            throw new ArgumentException($"Unknown leaf data: {leaf.Data}");
        }
        else if (node is Call call)
        {
            var args = call.Args.Select(arg => ToAst(arg)).ToArray();

            INode ast = call.Name switch
            {
                "Pay" when args[0] is FDate date 
                            && args[1] is FDouble amount 
                            && args[2] is FCurrency currency
                    => new FPay(date, amount, currency),
                "Receive" when args[0] is FDate date 
                            && args[1] is FDouble amount 
                            && args[2] is FCurrency currency
                    => new FPay(date, -amount, currency),
                "And" when args[0] is IContract c1 
                            && args[1] is IContract c2 
                    => new FAnd(new IContract[] {c1, c2}),
                _ => throw new ApplicationException($"Invalid or unexpected argument types in call: {call.Name}")
            };
            return ast;
        }
        throw new ApplicationException($"Unknown node type: {node.GetType().Name}");
    }
}
