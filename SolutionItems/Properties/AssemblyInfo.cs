using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: AssemblyVersion("1.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: AssemblyProduct("redback")]
[assembly: InternalsVisibleTo("Redback.Tests")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyInformationalVersion("")]    // trigger pre release package
#else
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyInformationalVersion("")]    // trigger pre release package
#endif

