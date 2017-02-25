using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: AssemblyVersion("1.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: AssemblyProduct("redback")]
[assembly: InternalsVisibleTo("Redback.Tests")]
[assembly: AssemblyInformationalVersion("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

