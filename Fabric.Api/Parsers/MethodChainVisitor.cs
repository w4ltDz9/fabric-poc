using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Fabric.Api.Parsers;

public class MethodChainVisitor : CSharpSyntaxVisitor<MethodChainVisitor.DslNode>
{
    public abstract record DslNode;
    public record Call(string Name, List<DslNode> Args) : DslNode;
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
        => new Call(node.Identifier.Text, new());

    public override DslNode VisitLiteralExpression(LiteralExpressionSyntax node)
        => new Call(node.ToString(), new());

    public static INode ToAst(DslNode node, int indent = 0)
    {
        var pad = new string(' ', indent * 2);

        if (node is Call call)
        {
            Console.WriteLine($"{pad}Call: {call.Name}");
            foreach (var arg in call.Args)
            {
                ToAst(arg, indent + 1);
            }
        }
        return null; //TODO
    }
}
