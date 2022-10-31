using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Devinno.Tools;
using System.Data.SQLite;
using SqliteCommand = System.Data.SQLite.SQLiteCommand;
using SqliteConnection = System.Data.SQLite.SQLiteConnection;
using SqliteTransaction = System.Data.SQLite.SQLiteTransaction;
using SqliteDataReader = System.Data.SQLite.SQLiteDataReader;

namespace Devinno.Database
{
    #region class : SQLite
    public class SQLite
    {
        #region Properties
        public string FileName { get; set; }
        public bool Lock { get; private set; }
        public string ConnectStringOptions { get; set; } = "";
        public bool Pooling { get; set; } = false;
        public bool Compress { get; set; } = false;
        public string ConnectString { get { return string.Format(@"Data Source={0};Version=3;Pooling={1};Compress={2};", FileName, Pooling ? "true" : "false", Compress ? "true" : "false") + ConnectStringOptions; } }
        #endregion

        #region Constructor
        public SQLite()
        {

        }
        #endregion

        #region Method
        #region Table
        public void CreateTable<T>(string TableName) { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.CreateTable<T>(cmd, TableName); }); }
        public void DropTable(string TableName) { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.DropTable(cmd, TableName); }); }
        public bool ExistTable(string TableName) { bool ret = false; ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.ExistTable(cmd, TableName); }); return ret; }
        #endregion
        #region Command
        #region Exist
        public bool Exist<T>(string TableName, T Data)
        {
            bool ret = false;
            ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.Exist<T>(cmd, TableName, Data); }); 
            return ret;
        }
        #endregion
        #region Check
        public bool Check(string TableName, string Where)
        {
            bool ret = false;
            ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.Check(cmd, TableName, Where); }); 
            return ret;
        }
        #endregion
        #region Select
        public List<T> Select<T>(string TableName)
        {
            return Select<T>(TableName, null);
        }
        public List<T> Select<T>(string TableName, string Where)
        {
            List<T> ret = null; 
            ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.Select<T>(cmd, TableName, Where); });
            return ret;
        }
        #endregion
        #region Update
        public void Update<T>(string TableName, params T[] Datas)
        {
            TransactionWaiting((conn, trans) =>
            {
                try
                {
                    foreach (var Data in Datas)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = trans;
                            if (Data != null) SQLiteCommandTool.Update<T>(cmd, TableName, Data);
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try { trans.Rollback(); }
                    catch (SqlException ex2) { }
                }

            });
        }
        #endregion
        #region Insert
        public void Insert<T>(string TableName, params T[] Datas)
        {
            TransactionWaiting((conn, trans) =>
            {
                try
                {
                    foreach (var Data in Datas)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = trans;
                            if (Data != null) SQLiteCommandTool.Insert<T>(cmd, TableName, Data);
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try { trans.Rollback(); }
                    catch (SqlException ex2) { }
                }

            });
        }
        #endregion
        #region Delete
        public void Delete<T>(string TableName, params T[] Datas)  
        { 
            ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.Delete<T>(cmd, TableName, Datas); });
        }
        public void Delete(string TableName, string Where)
        {
            ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.Delete(cmd, TableName, Where); });
        }
        #endregion
        #endregion
        #region Execute
        public void ExecuteWaiting(Action<SqliteConnection, SqliteCommand, SqliteTransaction> ExcuteQuery)
        {
            while (Lock) System.Threading.Thread.Sleep(10);
            Execute(ExcuteQuery);
        }

        public void Execute(Action<SqliteConnection, SqliteCommand, SqliteTransaction> ExcuteQuery)
        {
            Lock = true;
            using (var conn = new SqliteConnection() { ConnectionString = this.ConnectString })
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            ExcuteQuery(conn, cmd, trans);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                        }
                    }
                }
                conn.Close();
            }
            Lock = false;
        }

        public void TransactionWaiting(Action<SqliteConnection, SqliteTransaction> ExcuteTransaction)
        {
            while (Lock) System.Threading.Thread.Sleep(10);
            Transaction(ExcuteTransaction);
        }
        public void Transaction(Action<SqliteConnection, SqliteTransaction> ExcuteTransaction)
        {
            try
            {
                using (var conn = new SqliteConnection(ConnectString))
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            ExcuteTransaction(conn, trans);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
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
    #region class : SQLiteMemDB
    public class SQLiteMemDB
    {
        #region Properties
        public bool Lock { get; private set; }
        public string ConnectStringOptions { get; set; } = "";

        public string ConnectString { get { return string.Format(@"Data Source=:memory:;Version=3;") + ConnectStringOptions; } }
        public SqliteConnection Connection { get; private set; }
        #endregion

        #region Constructor
        public SQLiteMemDB()
        {

        }
        #endregion

        #region Method
        #region Connection
        #region Open
        public void Open()
        {
            Connection = new SqliteConnection() { ConnectionString = this.ConnectString };
            Connection.Open();
        }
        #endregion
        #region Close
        public void Close()
        {
            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
        #endregion
        #endregion

        #region Table
        public void CreateTable<T>(string TableName) { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.CreateTable<T>(cmd, TableName); }); }
        public void DropTable(string TableName) { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.DropTable(cmd, TableName); }); }
        public bool ExistTable(string TableName) { bool ret = false; ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.ExistTable(cmd, TableName); }); return ret; }
        #endregion
        #region Command
        #region Exist
        public bool Exist<T>(string TableName, T Data)
        {
            bool ret = false;
            ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.Exist<T>(cmd, TableName, Data); });
            return ret;
        }
        #endregion
        #region Check
        public bool Check(string TableName, string Where)
        {
            bool ret = false;
            ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.Check(cmd, TableName, Where); });
            return ret;
        }
        #endregion
        #region Select
        public List<T> Select<T>(string TableName)
        {
            return Select<T>(TableName, null);
        }
        public List<T> Select<T>(string TableName, string Where)
        {
            List<T> ret = null;
            ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.Select<T>(cmd, TableName, Where); });
            return ret;
        }
        #endregion
        #region Update
        public void Update<T>(string TableName, params T[] Datas)
        {
            TransactionWaiting((trans) =>
            {
                try
                {
                    var conn = Connection;
                    foreach (var Data in Datas)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = trans;
                            if (Data != null) SQLiteCommandTool.Update<T>(cmd, TableName, Data);
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try { trans.Rollback(); }
                    catch (SqlException ex2) { }
                }

            });
        }
        #endregion
        #region Insert
        public void Insert<T>(string TableName, params T[] Datas)
        {
            TransactionWaiting((trans) =>
            {
                try
                {
                    var conn = Connection;
                    foreach (var Data in Datas)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = trans;
                            if (Data != null) SQLiteCommandTool.Insert<T>(cmd, TableName, Data);
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try { trans.Rollback(); }
                    catch (SqlException ex2) { }
                }

            });
        }
        #endregion
        #region Delete
        public void Delete<T>(string TableName, params T[] Datas)
        {
            ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.Delete<T>(cmd, TableName, Datas); });
        }
        public void Delete(string TableName, string Where)
        {
            ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.Delete(cmd, TableName, Where); });
        }
        #endregion
        #endregion
        #region Execute
        public void ExecuteWaiting(Action<SqliteCommand, SqliteTransaction> ExcuteQuery)
        {
            while (Lock) System.Threading.Thread.Sleep(10);
            Execute(ExcuteQuery);
        }
        public void Execute(Action<SqliteCommand, SqliteTransaction> ExcuteQuery)
        {
            Lock = true;
            var conn = Connection;
            using (var trans = conn.BeginTransaction())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = trans;
                    try
                    {
                        ExcuteQuery(cmd, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }
            }
            Lock = false;
        }

        public void TransactionWaiting(Action<SqliteTransaction> ExcuteTransaction)
        {
            while (Lock) System.Threading.Thread.Sleep(10);
            Transaction(ExcuteTransaction);
        }
        public void Transaction(Action<SqliteTransaction> ExcuteTransaction)
        {
            try
            {
                var conn = Connection;
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        ExcuteTransaction(trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }

            }
            catch (Exception ex) { }
        }
        #endregion
        #endregion
    }
    #endregion
    #region class : SQLiteCommandTool
    public class SQLiteCommandTool
    {
        #region Command
        #region CreateTable
        public static void CreateTable<T>(SqliteCommand cmd, string TableName)
        {
            var keys = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && Attribute.IsDefined(x, typeof(SqlKeyAttribute))).ToList();
            var props = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)) && !Attribute.IsDefined(x, typeof(SqlKeyAttribute))).ToList();

            if (props.Count > 0)
            {
                var sb = new StringBuilder();

                sb.AppendLine("CREATE TABLE IF NOT EXISTS `" + TableName + "`");
                sb.AppendLine("(");

                var ls = new List<string>();

                foreach (var p in keys)
                {
                    var v = GetKeyText(p);
                    var idx = v.IndexOf("AUTOINCREMENT");

                    if(idx >= 0)
                    {
                        v = v.Substring(0, idx);
                        ls.Add(p.Name);
                    }

                    sb.AppendLine(v + ",");

                }
                foreach (var p in props) sb.AppendLine(GetTypeText(p) + ",");

                if (keys.Count > 0)
                {
                    var sK = "";
                    foreach (var v in keys)
                    {
                        var ac = "";

                        if (ls.Contains(v.Name)) ac = "AUTOINCREMENT";

                        sK += "`" + v.Name + "` " + ac + ",";

                    }
                    sK = sK.Trim(',');
                    sb.AppendLine($"    PRIMARY KEY({sK})");
                }

                sb.AppendLine(")");

                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region DropTable
        public static void DropTable(SqliteCommand cmd, string TableName)
        {
            cmd.CommandText = "DROP TABLE IF EXISTS `" + TableName + "`";
            cmd.ExecuteNonQuery();
        }
        #endregion
        #region ExistTable
        public static bool ExistTable(SqliteCommand cmd, string TableName)
        {
            bool ret = false;
            
            cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{TableName}'";
            using (var reader = cmd.ExecuteReader())
            {
                ret = reader.HasRows;
            }
            
            return ret;
        }
        #endregion
        #region Exists
        public static bool Exist<T>(SqliteCommand cmd, string TableName, T Data)
        {
            bool ret = false;

            string sql = $"SELECT * FROM `{TableName}` {GetWhere<T>(Data)}";
            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = rd.HasRows;
            }

            return ret;
        }
        #endregion
        #region Check
        public static bool Check(SqliteCommand cmd, string TableName, string Where)
        {
            bool ret = false;

            string sql = $"SELECT * FROM `{TableName}` {Where}";

            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = rd.HasRows;
            }

            return ret;
        }
        #endregion
        #region Select
        public static List<T> Select<T>(SqliteCommand cmd, string TableName, string Where)
        {
            List<T> ret = null;

            string sql = "SELECT * FROM `" + TableName + "`";
            if (!string.IsNullOrEmpty(Where)) sql += " " + Where;

            var props = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute))).ToList();

            cmd.CommandText = sql;

            try
            {
                using (var rd = cmd.ExecuteReader())
                {
                    ret = new List<T>();
                    while (rd.Read())
                    {
                        var v = (T)Activator.CreateInstance(typeof(T));
                        Read(rd, props, v);
                        ret.Add(v);
                    }
                }
            }
            catch (Exception ex) { }
            return ret;
        }
        #endregion
        #region Update
        public static void Update<T>(SqliteCommand cmd, string TableName, T Data)
        {
            if (Data != null)
            {
                var keys = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && Attribute.IsDefined(x, typeof(SqlKeyAttribute))).ToList();
                var props = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)) && !Attribute.IsDefined(x, typeof(SqlKeyAttribute))).ToList();

                string sql = $"UPDATE `{TableName}` SET ";
                string where = GetWhere<T>(keys, Data);
                foreach (var p in props) sql += $" `{p.Name}` = @{p.Name},";
                sql = sql.Substring(0, sql.Length - 1) + where;

                cmd.CommandText = sql;
                foreach (var pi in props) cmd.Parameters.AddWithValue("@" + pi.Name, GetValue(Data, pi));
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region Insert
        public static void Insert<T>(SqliteCommand cmd, string TableName,  T Data)
        {
            if (Data != null)
            {
                #region cols
                var keys = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && Attribute.IsDefined(x, typeof(SqlKeyAttribute))).ToList();
                var props = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)) && !Attribute.IsDefined(x, typeof(SqlKeyAttribute))).ToList();
                var cols = new List<PropertyInfo>();
                foreach (var v in keys)
                {
                    var bAutoInc = v.CustomAttributes.Where(x => x.AttributeType == typeof(SqlKeyAttribute)).FirstOrDefault()
                                     ?.NamedArguments.Where(x => x.MemberName == "AutoIncrement").FirstOrDefault().TypedValue.Value ?? false;

                    if (bAutoInc is bool && !(bool)bAutoInc) cols.Add(v);
                }
                cols.AddRange(props);
                #endregion

                string s_insert_in = string.Concat(cols.Select(x => $" `{x.Name}`,").ToArray());
                string s_values_in = string.Concat(cols.Select(x => $" @{x.Name},").ToArray());

                s_values_in = s_values_in.Substring(0, s_values_in.Length - 1);
                s_insert_in = s_insert_in.Substring(0, s_insert_in.Length - 1);

                string s_insert = $"INSERT INTO `{TableName}` ({s_insert_in})";
                string s_values = $"VALUES ({s_values_in})";
                string s_sql = s_insert + "\r\n" + s_values;

                cmd.CommandText = s_sql;
                foreach (var pi in cols)
                {
                    var sVal = GetValue(Data, pi);
                    cmd.Parameters.AddWithValue("@" + pi.Name, sVal);
                }

                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region Delete
        public static void Delete<T>(SqliteCommand cmd, string TableName, params T[] Datas)
        {
            if (Datas != null && Datas.Length > 0)
            {
                string sql = "DELETE FROM `" + TableName + "`\r\n" + GetWhere<T>(Datas);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(SqliteCommand cmd, string TableName, string Where)
        {
            string sql = "DELETE FROM `" + TableName + "`\r\n" + Where;

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        #endregion
        #endregion

        #region Tool
        #region GetTypeText
        static string GetTypeText(PropertyInfo pi)
        {
            string ret = null;
            var tp = pi.PropertyType;

            try
            {
                var ni = GetNullableInfo(pi);
                if (ni != null)
                {
                    string sType = null, sDefault = null;
                    #region sType / sDefault
                    if (ni.Type == typeof(bool)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(sbyte)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(byte)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(short)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(ushort)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(int)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(uint)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(long)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(ulong)) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(float)) { sType = "REAL"; sDefault = "0"; }
                    else if (ni.Type == typeof(double)) { sType = "REAL"; sDefault = "0"; }
                    else if (ni.Type == typeof(decimal)) { sType = "REAL"; sDefault = "0"; }
                    else if (ni.Type == typeof(char)) { sType = "TEXT"; sDefault = "' '"; }
                    else if (ni.Type == typeof(string)) { sType = "TEXT"; sDefault = "NULL"; }
                    else if (ni.Type == typeof(DateTime)) { sType = "TEXT"; sDefault = "'1970-01-01 00:00:00'"; }
                    else if (ni.Type == typeof(TimeSpan)) { sType = "TEXT"; sDefault = "'00:00:00'"; }
                    else if (ni.Type.IsEnum) { sType = "INTEGER"; sDefault = "0"; }
                    else if (ni.Type == typeof(byte[])) { sType = "TEXT"; sDefault = "NULL"; }
                    else if (ni.Type == typeof(Bitmap)) { sType = "TEXT"; sDefault = "NULL"; }
                    else throw new Exception("Unknown Type");

                    var sqlType = pi.CustomAttributes.Where(x => x.AttributeType == typeof(SqlTypeAttribute)).FirstOrDefault()
                                            ?.NamedArguments.Where(x => x.MemberName == "TypeString").FirstOrDefault().TypedValue.Value ?? null;

                    if (sqlType != null && sqlType is string) sType = (string)sqlType;
                    #endregion

                    if (sType != null)
                    {
                        if (ni.IsNullable)
                        {
                            ret = $"     `{pi.Name}` {sType} DEFAULT NULL";
                        }
                        else
                        {
                            if (sDefault != null)
                            {
                                if (sDefault == "NULL")
                                    ret = $"     `{pi.Name}` {sType} DEFAULT NULL";
                                else
                                    ret = $"     `{pi.Name}` {sType} NOT NULL DEFAULT {sDefault}";
                            }
                            else ret = $"     `{pi.Name}` {sType}";
                        }
                    }
                }
                else throw new Exception("Unknown Type");
            }
            catch (Exception ex) { }

            return ret;
        }
        #endregion
        #region GetKeyText
        static string GetKeyText(PropertyInfo pi)
        {
            string ret = null;
            var tp = pi.PropertyType;


            var ni = GetNullableInfo(pi);
            if (ni != null)
            {
                string sType = null, sAutoInc = "";
                #region sType / sAutoInc
                if (ni.Type == typeof(int) && !ni.IsNullable) { sType = "INTEGER"; }
                else if (ni.Type == typeof(uint) && !ni.IsNullable) { sType = "INTEGER"; }
                else if (ni.Type == typeof(long) && !ni.IsNullable) { sType = "INTEGER"; }
                else if (ni.Type == typeof(ulong) && !ni.IsNullable) { sType = "INTEGER"; }
                else if (ni.Type == typeof(string) && !ni.IsNullable) { sType = "TEXT"; }
                else new Exception("This type cannot be used as a key.");

                #region SqlType.TypeString
                var sqlType = pi.CustomAttributes.Where(x => x.AttributeType == typeof(SqlTypeAttribute)).FirstOrDefault()
                                            ?.NamedArguments.Where(x => x.MemberName == "TypeString").FirstOrDefault().TypedValue.Value ?? null;

                if (sqlType != null && sqlType is string) sType = (string)sqlType;
                #endregion

                #region SqlKey.AutoIncrement
                var bAutoInc = pi.CustomAttributes.Where(x => x.AttributeType == typeof(SqlKeyAttribute)).FirstOrDefault()
                                  ?.NamedArguments.Where(x => x.MemberName == "AutoIncrement").FirstOrDefault().TypedValue.Value ?? false;


                if (bAutoInc is bool && (bool)bAutoInc) sAutoInc = "AUTOINCREMENT";
                #endregion
                #endregion

                if (sType != null)
                {
                    ret = $"     `{pi.Name}` {sType} NOT NULL {sAutoInc}";
                }
            }
            else throw new Exception("This type cannot be used as a key.");

            return ret;
        }
        #endregion
        #region GetValue
        public static object GetValue(object Data, PropertyInfo pi)
        {
            var tp = pi.PropertyType;
            object ret = DBNull.Value;

            var ni = GetNullableInfo(pi);
            if (ni != null)
            {
                var value = pi.GetValue(Data, null);
                if (value != null)
                {
                    if (ni.Type == typeof(sbyte)) ret = Convert.ToInt32(value);
                    else if (ni.Type == typeof(ushort)) ret = Convert.ToInt32(value);
                    else if (ni.Type == typeof(uint)) ret = Convert.ToInt64(value);
                    else if (ni.Type == typeof(ulong)) ret = Convert.ToInt64(value);
                    else if (ni.Type.IsEnum) ret = (int)value;
                    else if (ni.Type == typeof(TimeSpan)) ret = ((TimeSpan)value).ToString();
                    else if (ni.Type == typeof(byte[])) ret = CryptoTool.EncodeBase64String((byte[])value);
                    else if (ni.Type == typeof(Bitmap)) ret = CryptoTool.EncodeBase64String((Bitmap)value);
                    else ret = value;
                }
            }
            return ret;
        }
        #endregion
        #region GetValueText
        static string GetValueText(PropertyInfo p, object Value)
        {
            string ret = "";

            var ni = GetNullableInfo(p);
            if (ni != null)
            {
                var v = p.GetValue(Value);

                if (ni.Type == typeof(bool)) ret = $"{(v != null ? ((bool)v ? "1" : "0") : "NULL")}";
                else if (ni.Type == typeof(sbyte)) ret = $"{(v != null ? ((sbyte)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(byte)) ret = $"{(v != null ? ((byte)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(short)) ret = $"{(v != null ? ((short)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(ushort)) ret = $"{(v != null ? ((ushort)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(int)) ret = $"{(v != null ? ((int)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(uint)) ret = $"{(v != null ? ((uint)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(long)) ret = $"{(v != null ? ((long)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(ulong)) ret = $"{(v != null ? ((ulong)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(float)) ret = $"{(v != null ? ((float)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(double)) ret = $"{(v != null ? ((double)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(decimal)) ret = $"{(v != null ? ((decimal)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(char)) ret = $"{(v != null ? "'" + ((char)v) + "'" : "NULL")}";
                else if (ni.Type == typeof(string)) ret = $"{(v != null ? "'" + ((string)v) + "'" : "NULL")}";
                else if (ni.Type == typeof(DateTime)) ret = $"{(v != null ? "'" + ((DateTime)v).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" : "NULL")}";
                else if (ni.Type == typeof(TimeSpan)) ret = $"{(v != null ? "'" + ((TimeSpan)v).ToString() + "'" : "NULL")}";
                else if (ni.Type.IsEnum) ret = $"{(v != null ? ((int)v).ToString() : "NULL")}";
                else if (ni.Type == typeof(byte[])) ret = $"{(v != null ? "'" + CryptoTool.EncodeBase64String((byte[])v) + "'" : "NULL")}";
                else if (ni.Type == typeof(Bitmap)) ret = $"{(v != null ? "'" + CryptoTool.EncodeBase64String((Bitmap)v) + "'" : "NULL")}";
                else throw new Exception("Unknown Type");

            }
            else throw new Exception("Unknown Type");

            return ret;
        }
        #endregion
        #region GetWhere
        public static string GetWhere<T>(params T[] Datas)
        {
            var keys = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && Attribute.IsDefined(x, typeof(SqlKeyAttribute))).ToList();

            return GetWhere(keys, Datas);
        }

        public static string GetWhere<T>(List<PropertyInfo> keys, params T[] Datas)
        {
            string ret = "";

            if (Datas != null && Datas.Length > 0)
            {
                ret += " WHERE ";

                for (int i = 0; i < Datas.Length; i++)
                {
                    var v = Datas[i];

                    ret += " (";
                    for (int j = 0; j < keys.Count; j++)
                    {
                        var k = keys[j];
                        ret += $" `{k.Name}` = {GetValueText(k, v)}" + (j < keys.Count - 1 ? " And " : "");
                    }

                    ret += " )" + (i < Datas.Length - 1 ? " Or " : "");
                }
            }

            return ret;
        }
        #endregion
        #region GetNullableInfo
        static NullableInfo GetNullableInfo(PropertyInfo pi)
        {
            NullableInfo ret = null;

            var tp = Nullable.GetUnderlyingType(pi.PropertyType);
            if (tp == null)
            {
                if (pi.CustomAttributes.Count() > 0)
                {
                    ret = new NullableInfo
                    {
                        IsNullable = pi.CustomAttributes.Where(x => x.AttributeType.ToString() == "System.Runtime.CompilerServices.NullableAttribute").FirstOrDefault() != null,
                        Type = pi.PropertyType,
                    };
                }
                else
                {
                    ret = new NullableInfo
                    {
                        IsNullable = false,
                        Type = pi.PropertyType,
                    };
                }
            }
            else
            {
                ret = new NullableInfo
                {
                    IsNullable = true,
                    Type = tp,
                };
            }

            return ret;
        }
        #endregion
        #region Read
        static void Read<T>(SqliteDataReader? rd, List<PropertyInfo> props, T? v)
        {
            if (rd != null && v != null && props != null && props.Count > 0)
            {
                foreach (var p in props)
                {
                    var tp = p.PropertyType;
                    int idx = rd.GetOrdinal(p.Name);

                    var ni = GetNullableInfo(p);
                    if (ni != null)
                    {
                        try
                        {
                            #region bool
                            if (ni.Type == typeof(bool))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, rd.GetBoolean(idx));
                                }
                            }
                            #endregion
                            #region sbyte
                            else if (ni.Type == typeof(sbyte))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, Convert.ToSByte(rd.GetInt32(idx)));
                                }
                            }
                            #endregion
                            #region Byte
                            else if (ni.Type == typeof(byte))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, Convert.ToByte(rd.GetInt32(idx)));
                                }
                            }
                            #endregion
                            #region Short
                            else if (ni.Type == typeof(short))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, Convert.ToInt16(rd.GetInt32(idx)));
                                }
                            }
                            #endregion
                            #region UShort
                            else if (ni.Type == typeof(ushort))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, Convert.ToUInt16(rd.GetInt32(idx)));
                                }
                            }
                            #endregion
                            #region Int
                            else if (ni.Type == typeof(int))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, rd.GetInt32(idx));
                                }
                            }
                            #endregion
                            #region UInt
                            else if (ni.Type == typeof(uint))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, Convert.ToUInt32(rd.GetInt64(idx)));
                                }
                            }
                            #endregion
                            #region Long
                            else if (ni.Type == typeof(long))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, rd.GetInt64(idx));
                                }
                            }
                            #endregion
                            #region ULong
                            else if (ni.Type == typeof(ulong))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, Convert.ToUInt64(rd.GetInt64(idx)));
                                }
                            }
                            #endregion
                            #region Float
                            else if (ni.Type == typeof(float))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, rd.GetFloat(idx));
                                }
                            }
                            #endregion
                            #region Double
                            else if (ni.Type == typeof(double))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, rd.GetDouble(idx));
                                }
                            }
                            #endregion
                            #region Decimal
                            else if (ni.Type == typeof(decimal))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, rd.GetDecimal(idx));
                                }
                            }
                            #endregion
                            #region Char
                            else if (ni.Type == typeof(char))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    var s = rd.GetString(idx);
                                    if (s.Length >= 1) p.SetValue(v, s.First());
                                    else if (ni.IsNullable) p.SetValue(v, null);
                                }
                            }
                            #endregion
                            #region String
                            else if (ni.Type == typeof(string))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, rd.GetString(idx));
                                }
                            }
                            #endregion
                            #region DateTime
                            else if (ni.Type == typeof(DateTime))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, DateTime.Parse(rd.GetString(idx)));
                                }
                            }
                            #endregion
                            #region TimeSpan
                            else if (ni.Type == typeof(TimeSpan))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, TimeSpan.Parse(rd.GetString(idx)));
                                }
                            }
                            #endregion
                            #region Enum
                            else if (ni.Type.IsEnum)
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    if (ni.IsNullable) p.SetValue(v, null);
                                }
                                else
                                {
                                    p.SetValue(v, Enum.ToObject(tp, rd.GetInt32(idx)));
                                }
                            }
                            #endregion
                            #region Byte[]
                            else if (ni.Type == typeof(byte[]))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    p.SetValue(v, null);
                                }
                                else
                                {
                                    var ba = Convert.FromBase64String(rd.GetString(idx));
                                    p.SetValue(v, ba);
                                }
                            }
                            #endregion
                            #region Bitmap
                            else if (ni.Type == typeof(Bitmap))
                            {
                                if (rd.IsDBNull(idx))
                                {
                                    p.SetValue(v, null);
                                }
                                else
                                {
                                    using (var m = new MemoryStream(Convert.FromBase64String(rd.GetString(idx))))
                                    {
                                        p.SetValue(v, (Bitmap)Bitmap.FromStream(m));
                                    }
                                }
                            }
                            #endregion
                            #region Error
                            else throw new Exception("Unknown Type");
                            #endregion
                        }
                        catch (Exception ex) { throw new Exception("Parse Error"); }
                    }
                    else throw new Exception("Read Error");
                }
            }
        }
        #endregion
        #endregion
    }
    #endregion
}
