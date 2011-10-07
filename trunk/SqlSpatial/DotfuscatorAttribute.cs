using System;
using System.Runtime.InteropServices;

[ComVisible(false), AttributeUsage(AttributeTargets.Assembly)]
public sealed class DotfuscatorAttribute : Attribute
{
    private string a;
    private int c;

    public DotfuscatorAttribute(string a, int c)
    {
        this.a = a;
        this.c = c;
    }

    public string A
    {
        get
        {
            return this.a;
        }
    }

    public int C
    {
        get
        {
            return this.c;
        }
    }
}

