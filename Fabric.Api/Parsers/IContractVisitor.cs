using System;
using Fabric.Api.Ast;

namespace Fabric.Api.Parsers;

public interface IContractVisitor<T>
{
    T Visit(FAnd and);
    T Visit(FPay pay);
}

public interface IContractVisitor<T,CTX>
{
    T Visit(FAnd and, CTX ctx);
    T Visit(FPay pay, CTX ctx);
}
