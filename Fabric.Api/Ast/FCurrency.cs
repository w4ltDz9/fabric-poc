using System;

namespace Fabric.Api.Ast;

public enum Ccy{EUR,DKK}

public class FCurrency : FWrapper<Ccy>
{
    public FCurrency(Ccy inner) : base(inner)
    {
    }

    public Ccy Currency => Inner;

            // Implicit conversions
    public static implicit operator Ccy(FCurrency wrapper) => wrapper.Inner;
    public static implicit operator FCurrency(Ccy value) => new FCurrency(value);
}
