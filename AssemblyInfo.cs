using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("21E42EC3-D33C-40AA-A4F5-271953CA1FDB")]

[assembly: AssemblyTitle("RestlessMouse")]
[assembly: AssemblyDescription("Minimal mouse activity simulator")]

[assembly: AssemblyCompany("Ittegrat")]
[assembly: AssemblyProduct("Windows Mouse Activity Simulator")]
[assembly: AssemblyCopyright("Copyright © 2022 Ittegrat")]

[assembly: AssemblyVersion("0.1.0")]
[assembly: AssemblyFileVersion("0.1.0")]
[assembly: AssemblyInformationalVersion("0.1.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
