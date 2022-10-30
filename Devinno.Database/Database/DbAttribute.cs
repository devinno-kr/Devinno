using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Database
{
    #region attribute : SqlKey
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SqlKeyAttribute : Attribute
    {
        public bool AutoIncrement { get; set; } = false;
    }
    #endregion
    #region attribute : SqlType
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SqlTypeAttribute : Attribute
    {
        public string? TypeString { get; set; } = null;
    }
    #endregion
    #region attribute : SqlIgnore
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SqlIgnoreAttribute : Attribute { }
    #endregion}
    #region class : NullableInfo
    class NullableInfo
    {
        public bool IsNullable { get; set; }
        public Type Type { get; set; }
    }
    #endregion
}
