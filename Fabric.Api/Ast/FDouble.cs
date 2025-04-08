using System;

namespace Fabric.Api.Ast;

public class FDouble : FWrapper<double>
{
    public FDouble(double inner) : base(inner)
    {
    }

    public double Amount => Inner;

            // Implicit conversions
    public static implicit operator double(FDouble wrapper) => wrapper.Inner;
    public static implicit operator FDouble(double value) => new FDouble(value);
}
