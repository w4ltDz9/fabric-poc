using System;
using Fabric.Api.Services;

namespace Fabric.Api.Parsers;

public interface IContract
{
    T Accept<T>(IContractVisitor<T> visitor);
    T Accept<T, CTX>(IContractVisitor<T, CTX> visitor, CTX ctx);
}
