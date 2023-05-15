using System;

internal class Pet : Attribute
{
    private byte byte_0;

    public Pet(byte byte_1)
    {
        // ISSUE: reference to a compiler-generated method
        this.method_1(byte_1);
    }
    private void method_1(byte byte_1)
    {
        this.byte_0 = byte_1;
    }
    public byte method_0()
    {
        return this.byte_0;
    }
}
