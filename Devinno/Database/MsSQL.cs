using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Database
{
    #region class : MySQL
    public class MsSQL
    {
        #region Properties
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 3306;
        public string ID { get; set; } = "root";
        public string Password { get; set; } = "1234";
        public string DatabaseName { get; set; }
        public bool IntegratedSecurity { get; set; }

        private string ConnectString => (IntegratedSecurity
            ? $"server={Host};Port={Port};database={DatabaseName};Integrated Security=True;CHARSET=utf8"
            : $"Server={Host};Port={Port};Database={DatabaseName};Uid={ID};pwd={Password};CHARSET=utf8");
        #endregion
        #region Constructor
        public MsSQL()
        {

        }
        #endregion

        #region Method
        #region Table
        public void CreateTable<T>(string TableName) where T : MsData { Execute((conn, cmd, trans) => { MsSqlCommandTool.CreateTable<T>(cmd, TableName); }); }
        public void DropTable(string TableName) { Execute((conn, cmd, trans) => { MsSqlCommandTool.DropTable(cmd, TableName); }); }
        public bool ExistTable(string TableName) { bool ret = false; Execute((conn, cmd, trans) => { ret = MsSqlCommandTool.ExistTable(cmd, TableName); }); return ret; }
        #endregion
        #region Command
        public bool Exist(string TableName, int Id) { bool ret = false; Execute((conn, cmd, trans) => { ret = MsSqlCommandTool.Exist(cmd, TableName, Id); }); return ret; }
        public bool Exist<T>(string TableName, T Data) where T : MsData { bool ret = false; Execute((conn, cmd, trans) => { ret = MsSqlCommandTool.Exist<T>(cmd, TableName, Data); }); return ret; }
        public bool Check(string TableName, string Where) { bool ret = false; Execute((conn, cmd, trans) => { ret = MsSqlCommandTool.Check(cmd, TableName, Where); }); return ret; }
        public List<T> Select<T>(string TableName) where T : MsData { return Select<T>(TableName, null); }
        public List<T> Select<T>(string TableName, string Where) where T : MsData { List<T> ret = null; Execute((conn, cmd, trans) => { ret = MsSqlCommandTool.Select<T>(cmd, TableName, Where); }); return ret; }
        public void Update<T>(string TableName, params T[] Datas) where T : MsData { Execute((conn, cmd, trans) => { MsSqlCommandTool.Update<T>(cmd, TableName, Datas); }); }
        public void Insert<T>(string TableName, params T[] Datas) where T : MsData { Execute((conn, cmd, trans) => { MsSqlCommandTool.Insert<T>(cmd, TableName, Datas); }); }
        public void Delete<T>(string TableName, params T[] Datas) where T : MsData { Execute((conn, cmd, trans) => { MsSqlCommandTool.Delete<T>(cmd, TableName, Datas); }); }
        public void Delete(string TableName, List<int> Ids) { Execute((conn, cmd, trans) => { MsSqlCommandTool.Delete(cmd, TableName, Ids); }); }
        public void Delete(string TableName, string Where) { Execute((conn, cmd, trans) => { MsSqlCommandTool.Delete(cmd, TableName, Where); }); }
        #endregion
        #region Execute
        public void Execute(Action<SqlConnection, SqlCommand, SqlTransaction> ExcuteQuery)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                ExcuteQuery(conn, cmd, trans);
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                try { trans.Rollback(); }
                                catch (SqlException ex2) { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        #endregion
        #endregion
    }
    #endregion
    #region class : MsSqlCommandTool
    public class MsSqlCommandTool
    {
        #region Command
        #region CreateTable
        public static void CreateTable<T>(SqlCommand cmd, string TableName) where T : MsData
        {
            var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute))).ToList();

            if (props.Count > 0)
            {
                var sb = new StringBuilder();

                sb.AppendLine("IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + TableName + "')");
                sb.AppendLine("CREATE TABLE [" + TableName + "]");
                sb.AppendLine("(");
                sb.AppendLine("     [ID] [uniqueidentifier] NOT NULL PRIMARY KEY,");
                foreach (var p in props) sb.AppendLine("     [" + p.Name + "] " + GetTypeText(p) + ",");
                sb.AppendLine(")");

                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region DropTable
        public static void DropTable(SqlCommand cmd, string TableName)
        {
            cmd.CommandText = "DROP TABLE IF EXISTS [" + TableName + "]";
            cmd.ExecuteNonQuery();
        }
        #endregion
        #region ExistTable
        public static bool ExistTable(SqlCommand cmd, string TableName)
        {
            bool ret = false;
            cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + TableName + "'";
            using (var reader = cmd.ExecuteReader())
            {
                ret = reader.HasRows;
            }
            return ret;
        }
        #endregion
        #region Exists
        public static bool Exist(SqlCommand cmd, string TableName, int Id)
        {
            bool ret = false;
            string sql = "SELECT * FROM [" + TableName + "] WHERE [Id]=" + Id;
            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = rd.HasRows;
            }
            return ret;
        }

        public static bool Exist<T>(SqlCommand cmd, string TableName, T Data) where T : MsData
        {
            bool ret = false;
            string sql = "SELECT * FROM [" + TableName + "] WHERE [Id]=" + Data.Id;
            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = rd.HasRows;
            }
            return ret;
        }
        #endregion
        #region Check
        public static bool Check(SqlCommand cmd, string TableName, string Where)
        {
            bool ret = false;

            string sql = "SELECT * FROM [" + TableName + "] " + Where;

            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = rd.HasRows;
            }

            return ret;
        }
        #endregion
        #region Select
        public static List<T> Select<T>(SqlCommand cmd, string TableName, string Where) where T : MsData
        {
            List<T> ret = null;

            string sql = "SELECT * FROM [" + TableName + "]";
            if (!string.IsNullOrEmpty(Where)) sql += " " + Where;

            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = new List<T>();
                while (rd.Read())
                {
                    var id = rd.GetInt32(rd.GetOrdinal("Id"));
                    var v = (T)Activator.CreateInstance(typeof(T));
                    var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)));

                    v.Id = id;

                    foreach (var pi in props)
                    {
                        var tp = pi.PropertyType;
                        int idx = rd.GetOrdinal(pi.Name);

                        try
                        {
                            #region bool
                            if (tp == typeof(bool))
                            {
                                pi.SetValue(v, rd.GetBoolean(idx), null);
                            }
                            else if (tp == typeof(bool?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetBoolean(idx), null);
                            }
                            #endregion
                            #region byte
                            else if (tp == typeof(byte))
                            {
                                pi.SetValue(v, rd.GetByte(idx), null);
                            }
                            else if (tp == typeof(byte?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetByte(idx), null);
                            }
                            #endregion
                            #region short
                            else if (tp == typeof(short))
                            {
                                pi.SetValue(v, rd.GetInt16(idx), null);
                            }
                            else if (tp == typeof(short?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetInt16(idx), null);
                            }
                            #endregion
                            #region ushort
                            else if (tp == typeof(ushort))
                            {
                                pi.SetValue(v, Convert.ToUInt16(rd.GetInt32(idx)), null);
                            }
                            else if (tp == typeof(ushort?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, Convert.ToUInt16(rd.GetInt32(idx)), null);
                            }
                            #endregion
                            #region int
                            else if (tp == typeof(int))
                            {
                                pi.SetValue(v, rd.GetInt32(idx), null);
                            }
                            else if (tp == typeof(int?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetInt32(idx), null);
                            }
                            #endregion
                            #region uint
                            else if (tp == typeof(uint))
                            {
                                pi.SetValue(v, Convert.ToUInt32(rd.GetInt64(idx)), null);
                            }
                            else if (tp == typeof(uint?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, Convert.ToUInt32(rd.GetInt64(idx)), null);
                            }
                            #endregion
                            #region long
                            else if (tp == typeof(long))
                            {
                                pi.SetValue(v, rd.GetInt64(idx), null);
                            }
                            else if (tp == typeof(long?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetInt64(idx), null);
                            }
                            #endregion
                            #region ulong
                            else if (tp == typeof(ulong))
                            {
                                pi.SetValue(v, Convert.ToUInt64(rd.GetInt64(idx)), null);
                            }
                            else if (tp == typeof(ulong?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, Convert.ToUInt64(rd.GetInt64(idx)), null);
                            }
                            #endregion
                            #region float
                            else if (tp == typeof(float))
                            {
                                pi.SetValue(v, rd.GetFloat(idx), null);
                            }
                            else if (tp == typeof(float?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetFloat(idx), null);
                            }
                            #endregion
                            #region double
                            else if (tp == typeof(double))
                            {
                                pi.SetValue(v, rd.GetDouble(idx), null);
                            }
                            else if (tp == typeof(double?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetDouble(idx), null);
                            }
                            #endregion
                            #region decimal
                            else if (tp == typeof(decimal))
                            {
                                pi.SetValue(v, rd.GetDecimal(idx), null);
                            }
                            else if (tp == typeof(decimal?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetDecimal(idx), null);
                            }
                            #endregion
                            #region string
                            else if (tp == typeof(string))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetString(idx), null);
                            }
                            #endregion
                            #region DateTime
                            if (tp == typeof(DateTime))
                            {
                                pi.SetValue(v, rd.GetDateTime(idx), null);
                            }
                            else if (tp == typeof(DateTime?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, rd.GetDateTime(idx), null);
                            }
                            #endregion
                            #region TimeSpan
                            if (tp == typeof(TimeSpan))
                            {
                                pi.SetValue(v, TimeSpan.Parse(rd.GetString(idx)), null);
                            }
                            else if (tp == typeof(TimeSpan?))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null, null);
                                else pi.SetValue(v, TimeSpan.Parse(rd.GetString(idx)), null);
                            }
                            #endregion
                            #region Enum
                            if (tp.IsEnum)
                            {
                                if (!rd.IsDBNull(idx))
                                {
                                    pi.SetValue(v, Enum.ToObject(tp, rd.GetInt32(idx)), null);
                                }
                            }
                            #endregion
                            #region Int[]
                            if (tp == typeof(int[]))
                            {
                                if (!rd.IsDBNull(idx))
                                {
                                    var ba = Convert.FromBase64String(rd.GetString(idx));

                                    var ls = new List<int>();
                                    for (int i = 0; i < ba.Length; i += 4) ls.Add(BitConverter.ToInt32(ba, i));

                                    pi.SetValue(v, ls.ToArray(), null);
                                }
                            }
                            #endregion
                            #region uInt[]
                            if (tp == typeof(uint[]))
                            {
                                if (!rd.IsDBNull(idx))
                                {
                                    var ba = Convert.FromBase64String(rd.GetString(idx));

                                    var ls = new List<uint>();
                                    for (int i = 0; i < ba.Length; i += 4) ls.Add(BitConverter.ToUInt32(ba, i));

                                    pi.SetValue(v, ls.ToArray(), null);
                                }
                            }
                            #endregion
                            #region byte[]
                            if (tp == typeof(byte[]))
                            {
                                if (!rd.IsDBNull(idx))
                                {
                                    var ba = Convert.FromBase64String(rd.GetString(idx));
                                    pi.SetValue(v, ba, null);
                                }
                            }
                            #endregion
                            #region Bitmap
                            else if (tp == typeof(System.Drawing.Bitmap))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null);
                                else
                                {
                                    using (var m = new MemoryStream(Convert.FromBase64String(rd.GetString(idx))))
                                    {
                                        pi.SetValue(v, (Bitmap)Bitmap.FromStream(m));
                                    }
                                }
                            }
                            else if (tp == typeof(System.Drawing.Image))
                            {
                                if (rd.IsDBNull(idx)) pi.SetValue(v, null);
                                else
                                {
                                    using (var m = new MemoryStream(Convert.FromBase64String(rd.GetString(idx))))
                                    {
                                        pi.SetValue(v, (Image)Image.FromStream(m));
                                    }
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex) { }
                    }

                    ret.Add(v);
                }
            }

            return ret;
        }
        #endregion
        #region Update
        public static void Update<T>(SqlCommand cmd, string TableName, params T[] Datas) where T : MsData
        {
            foreach (var Data in Datas)
            {
                if (Data != null)
                {
                    string sql = "UPDATE [" + TableName + "] SET ";
                    string where = " WHERE [Id]=" + Data.Id;

                    var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)));
                    foreach (var pi in props) sql += " [" + pi.Name + "] = @" + pi.Name + ",";

                    cmd.CommandText = sql.Substring(0, sql.Length - 1) + where;
                    foreach (var pi in props) cmd.Parameters.AddWithValue("@" + pi.Name, GetValue(Data, pi));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #region Insert
        public static void Insert<T>(SqlCommand cmd, string TableName, params T[] Datas) where T : MsData
        {
            foreach (var Data in Datas)
            {
                var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)));

                string s_insert_in = string.Concat(props.Select(x => " [" + x.Name + "],").ToArray());
                string s_values_in = string.Concat(props.Select(x => " @" + x.Name + ",").ToArray());

                s_values_in = s_values_in.Substring(0, s_values_in.Length - 1);
                s_insert_in = s_insert_in.Substring(0, s_insert_in.Length - 1);

                string s_insert = "INSERT INTO [" + TableName + "] (" + s_insert_in + " )";
                string s_values = "VALUES (" + s_values_in + " )";
                string s_sql = s_insert + "\r\n" + s_values;

                cmd.CommandText = s_sql;
                foreach (var pi in props) cmd.Parameters.AddWithValue("@" + pi.Name, GetValue(Data, pi));
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region Delete
        public static void Delete<T>(SqlCommand cmd, string TableName, params T[] Datas) where T : MsData
        {
            if (Datas.Length > 0)
            {
                string sql = "DELETE FROM [" + TableName + "]\r\nWHERE ";
                for (int i = 0; i < Datas.Length; i++) sql += "ID = " + Datas[i].Id + (i < Datas.Length - 1 ? " OR " : "");

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(SqlCommand cmd, string TableName, List<int> Ids)
        {
            if (Ids.Count > 0)
            {
                string sql = "DELETE FROM [" + TableName + "]\r\nWHERE ";
                for (int i = 0; i < Ids.Count; i++) sql += "ID = " + Ids[i] + (i < Ids.Count - 1 ? " OR " : "");

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(SqlCommand cmd, string TableName, string Where)
        {
            string sql = "DELETE FROM [" + TableName + "]\r\n" + Where;

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        #endregion
        #endregion
        #region GetTypeText
        static string GetTypeText(PropertyInfo pi)
        {
            string ret = null;
            var tp = pi.PropertyType;
            if (tp == typeof(bool)) ret = "bit(1) NOT NULL DEFAULT 0";
            else if (tp == typeof(bool?)) ret = "bit(1) DEFAULT NULL";
            else if (tp == typeof(byte)) ret = "tinyint(3) unsigned NOT NULL DEFAULT '0'";
            else if (tp == typeof(short)) ret = "smallint(6) NOT NULL DEFAULT '0'";
            else if (tp == typeof(ushort)) ret = "smallint(6) unsigned NOT NULL DEFAULT '0'";
            else if (tp == typeof(int)) ret = "int(11) NOT NULL DEFAULT '0'";
            else if (tp == typeof(uint)) ret = "int(11) unsigned NOT NULL DEFAULT '0'";
            else if (tp == typeof(long)) ret = "bigint(20) NOT NULL DEFAULT '0'";
            else if (tp == typeof(ulong)) ret = "bigint(20) unsigned NOT NULL DEFAULT '0'";
            else if (tp == typeof(byte?)) ret = "tinyint(3) unsigned DEFAULT NULL";
            else if (tp == typeof(short?)) ret = "smallint(6) DEFAULT NULL";
            else if (tp == typeof(ushort?)) ret = "smallint(6) unsigned DEFAULT NULL";
            else if (tp == typeof(int?)) ret = "int(11) DEFAULT NULL";
            else if (tp == typeof(uint?)) ret = "int(11) unsigned DEFAULT NULL";
            else if (tp == typeof(long?)) ret = "bigint(20) DEFAULT NULL";
            else if (tp == typeof(ulong?)) ret = "bigint(20) unsigned DEFAULT NULL";
            else if (tp == typeof(float)) ret = "float NOT NULL DEFAULT '0'";
            else if (tp == typeof(double)) ret = "double NOT NULL DEFAULT '0'";
            else if (tp == typeof(decimal)) ret = "decimal NOT NULL DEFAULT '0'";
            else if (tp == typeof(float?)) ret = "float DEFAULT NULL";
            else if (tp == typeof(double?)) ret = "double DEFAULT NULL";
            else if (tp == typeof(decimal?)) ret = "decimal DEFAULT NULL";
            else if (tp == typeof(string)) ret = "text DEFAULT NULL";
            else if (tp == typeof(DateTime)) ret = "datetime NOT NULL DEFAULT '1970-01-01 00:00:00'";
            else if (tp == typeof(DateTime?)) ret = "datetime DEFAULT NULL";
            else if (tp == typeof(TimeSpan)) ret = "text NOT NULL DEFAULT '00:00:00'";
            else if (tp == typeof(TimeSpan?)) ret = "text DEFAULT NULL";
            else if (tp.IsEnum) ret = "int(11) NOT NULL DEFAULT '0'";
            else if (tp == typeof(int[])) ret = "longtext DEFAULT NULL";
            else if (tp == typeof(uint[])) ret = "longtext DEFAULT NULL";
            else if (tp == typeof(byte[])) ret = "longtext DEFAULT NULL";
            else throw new Exception("Unknown Type");
            return ret;
        }
        #endregion
        #region GetValue
        public static object GetValue(object Data, PropertyInfo pi)
        {
            var tp = pi.PropertyType;
            object ret = null;

            if (tp == typeof(bool)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(bool?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(byte)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(byte?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(short)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(short?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(ushort)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(ushort?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(int)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(int?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(uint)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(uint?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(long)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(long?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(ulong)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(ulong?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(float)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(float?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(double)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(double?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(decimal)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(decimal?)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(string)) ret = pi.GetValue(Data, null);
            else if (tp == typeof(DateTime))
            {
                var v = (DateTime)pi.GetValue(Data, null);
                ret = v;
            }
            else if (tp == typeof(DateTime?))
            {
                var v = (DateTime?)pi.GetValue(Data, null);
                ret = v.HasValue ? v.Value : (DateTime?)null;
            }
            else if (tp == typeof(TimeSpan))
            {
                var v = (TimeSpan)pi.GetValue(Data, null);
                ret = v.ToString();
            }
            else if (tp == typeof(TimeSpan?))
            {
                var v = (TimeSpan?)pi.GetValue(Data, null);
                ret = v.HasValue ? v.Value.ToString() : null;
            }
            else if (tp.IsEnum)
            {
                var v = pi.GetValue(Data, null);
                ret = Convert.ToInt32(v).ToString();
            }
            else if (tp == typeof(int[]))
            {
                var v = (int[])pi.GetValue(Data, null);

                if (v == null) ret = null;
                else
                {
                    var ls = new List<byte>();
                    foreach (var vv in v.Select(x => BitConverter.GetBytes(x))) ls.AddRange(vv);
                    ret = Convert.ToBase64String(ls.ToArray());
                }
            }
            else if (tp == typeof(uint[]))
            {
                var v = (uint[])pi.GetValue(Data, null);

                if (v == null) ret = null;
                else
                {
                    var ls = new List<byte>();
                    foreach (var vv in v.Select(x => BitConverter.GetBytes(x))) ls.AddRange(vv);
                    ret = Convert.ToBase64String(ls.ToArray());
                }
            }
            else if (tp == typeof(byte[]))
            {
                var v = (byte[])pi.GetValue(Data, null);
                if (v == null) ret = null;
                else
                {
                    ret = Convert.ToBase64String(v);
                }
            }
            else if (tp == typeof(System.Drawing.Bitmap))
            {
                var v = (Bitmap)pi.GetValue(Data);
                if (v != null)
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        v.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                        ret = Convert.ToBase64String(m.ToArray());
                    }
                }
            }
            else if (tp == typeof(System.Drawing.Image))
            {
                var v = (Image)pi.GetValue(Data);
                if (v != null)
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        v.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                        ret = Convert.ToBase64String(m.ToArray());
                    }
                }
            }
            return ret;
        }
        #endregion
    }
    #endregion
    #region class : MsData
    public class MsData
    {
        public int Id { get; set; }
    }
    #endregion
}
