using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Database
{
    #region attribute : SQLIgnoreAttribute
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SqlIgnoreAttribute : Attribute { }
    #endregion
}
