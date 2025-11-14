using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyToolkit.Base.Extensions
{
    public static class AppDomainExtension
    {
        public static Assembly GetAssemblyByName(this AppDomain domain, string assemblyName)
        {
            return domain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == assemblyName);
        }
    }
}
