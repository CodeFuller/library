using System.Reflection;

//	http://stackoverflow.com/a/62637/5740031

[assembly: AssemblyProduct("CF.Library")]

[assembly: AssemblyCompany("CodeFuller")]
[assembly: AssemblyCopyright("Copyright © 2016")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
