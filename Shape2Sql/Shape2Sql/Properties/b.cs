namespace Shape2Sql.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class b
    {
        private static ResourceManager a;
        private static CultureInfo b;

        internal b()
        {
        }

        internal static CultureInfo a()
        {
            return b;
        }

        internal static void a(CultureInfo A_0)
        {
            b = A_0;
        }

        internal static ResourceManager b()
        {
            if (object.ReferenceEquals(a, null))
            {
                ResourceManager manager = new ResourceManager("Shape2Sql.Properties.Resources", typeof(Shape2Sql.Properties.b).Assembly);
                a = manager;
            }
            return a;
        }
    }
}

