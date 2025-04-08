using System;

namespace Fabric.Api.Ast;

public class FDate : FWrapper<DateTime>
{
    public FDate(DateTime inner) : base(inner)
    {
    }

    public DateTime Date => Inner;

        // Implicit conversions
    public static implicit operator DateTime(FDate wrapper) => wrapper.Inner;
    public static implicit operator FDate(DateTime value) => new FDate(value);
}
