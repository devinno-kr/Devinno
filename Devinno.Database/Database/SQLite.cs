using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if NET5_0
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;

using SqliteCommand = System.Data.SQLite.SQLiteCommand;
using SqliteConnection = System.Data.SQLite.SQLiteConnection;
using SqliteTransaction = System.Data.SQLite.SQLiteTransaction;
#endif

namespace Devinno.Database
{
    #region class : SQLite
    public class SQLite
    {
        #region Properties
        public string FileName { get; set; }
        public bool Lock { get; private set; }
        public string ConnectString { get { return string.Format(@"Data Source={0};Version=3;Pooling=false;Compress=false", FileName); } }
        #endregion

        #region Constructor
        public SQLite()
        {

        }
        #endregion

        #region Method
        #region Table
        public void CreateTable<T>(string TableName) where T : SQLiteData { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.CreateTable<T>(cmd, TableName); }); }
        public void DropTable(string TableName) { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.DropTable(cmd, TableName); }); }
        public bool ExistTable(string TableName) { bool ret = false; ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.ExistTable(cmd, TableName); }); return ret; }
        #endregion
        #region Command
        public bool Exist(string TableName, int Id) { bool ret = false; ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.Exist(cmd, TableName, Id); }); return ret; }
        public bool Exist<T>(string TableName, T Data) where T : SQLiteData { bool ret = false; ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.Exist<T>(cmd, TableName, Data); }); return ret; }
        public bool Check(string TableName, string Where) { bool ret = false; ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.Check(cmd, TableName, Where); }); return ret; }
        public List<T> Select<T>(string TableName) where T : SQLiteData { return Select<T>(TableName, null); }
        public List<T> Select<T>(string TableName, string Where) where T : SQLiteData { List<T> ret = null; ExecuteWaiting((conn, cmd, trans) => { ret = SQLiteCommandTool.Select<T>(cmd, TableName, Where); }); return ret; }
        public void Update<T>(string TableName, params T[] Datas) where T : SQLiteData { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.Update<T>(cmd, TableName, Datas); }); }
        public void Insert<T>(string TableName, params T[] Datas) where T : SQLiteData { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.Insert<T>(cmd, TableName, Datas); }); }
        public void Delete<T>(string TableName, params T[] Datas) where T : SQLiteData { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.Delete<T>(cmd, TableName, Datas); }); }
        public void Delete(string TableName, List<int> Ids) { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.Delete(cmd, TableName, Ids); }); }
        public void Delete(string TableName, string Where) { ExecuteWaiting((conn, cmd, trans) => { SQLiteCommandTool.Delete(cmd, TableName, Where); }); }
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
                            trans.Rollback();
                        }
                    }
                }
                conn.Close();
            }
            Lock = false;
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
        public string ConnectString { get { return string.Format(@"Data Source=:memory:;Version=3;"); } }
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
        public void CreateTable<T>(string TableName) where T : SQLiteData { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.CreateTable<T>(cmd, TableName); }); }
        public void DropTable(string TableName) { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.DropTable(cmd, TableName); }); }
        public bool ExistTable(string TableName) { bool ret = false; ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.ExistTable(cmd, TableName); }); return ret; }
        #endregion
        #region Command
        public bool Exist(string TableName, int Id) { bool ret = false; ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.Exist(cmd, TableName, Id); }); return ret; }
        public bool Exist<T>(string TableName, T Data) where T : SQLiteData { bool ret = false; ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.Exist<T>(cmd, TableName, Data); }); return ret; }
        public bool Check(string TableName, string Where) { bool ret = false; ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.Check(cmd, TableName, Where); }); return ret; }
        public List<T> Select<T>(string TableName) where T : SQLiteData { return Select<T>(TableName, null); }
        public List<T> Select<T>(string TableName, string Where) where T : SQLiteData { List<T> ret = null; ExecuteWaiting((cmd, trans) => { ret = SQLiteCommandTool.Select<T>(cmd, TableName, Where); }); return ret; }
        public void Update<T>(string TableName, params T[] Datas) where T : SQLiteData { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.Update<T>(cmd, TableName, Datas); }); }
        public void Insert<T>(string TableName, params T[] Datas) where T : SQLiteData { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.Insert<T>(cmd, TableName, Datas); }); }
        public void Delete<T>(string TableName, params T[] Datas) where T : SQLiteData { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.Delete<T>(cmd, TableName, Datas); }); }
        public void Delete(string TableName, List<int> Ids) { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.Delete(cmd, TableName, Ids); }); }
        public void Delete(string TableName, string Where) { ExecuteWaiting((cmd, trans) => { SQLiteCommandTool.Delete(cmd, TableName, Where); }); }
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
            using (var cmd = conn.CreateCommand())
            {
                using (var trans = conn.BeginTransaction())
                {
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
        #endregion
        #endregion
    }
    #endregion
    #region class : SQLiteCommandTool
    public class SQLiteCommandTool
    {
        #region Command
        #region CreateTable
        public static void CreateTable<T>(SqliteCommand cmd, string TableName) where T : SQLiteData
        {
            var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute))).ToList();

            if (props.Count > 0)
            {
                var sb = new StringBuilder();
                var sb2 = new StringBuilder();
                sb.AppendLine("CREATE TABLE IF NOT EXISTS `" + TableName + "`");
                sb.AppendLine("(");
                sb.AppendLine("     `Id` INTEGER PRIMARY KEY AUTOINCREMENT,");
                foreach (var p in props) sb2.AppendLine("     `" + p.Name + "` " + GetTypeText(p) + ",");

                var vs = sb2.ToString();
                vs = vs.Substring(0, vs.Length - 3);

                sb.AppendLine(vs);
                sb.AppendLine(");");

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
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + TableName + "'";
            using (var reader = cmd.ExecuteReader())
            {
                ret = reader.HasRows;
            }
            return ret;
        }
        #endregion
        #region Exists
        public static bool Exist(SqliteCommand cmd, string TableName, int Id)
        {
            bool ret = false;
            string sql = "SELECT * FROM `" + TableName + "` WHERE `Id`=" + Id;
            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = rd.HasRows;
            }
            return ret;
        }

        public static bool Exist<T>(SqliteCommand cmd, string TableName, T Data) where T : SQLiteData
        {
            bool ret = false;
            string sql = "SELECT * FROM `" + TableName + "` WHERE `Id`=" + Data.Id;
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

            string sql = "SELECT * FROM `" + TableName + "` " + Where;

            cmd.CommandText = sql;
            using (var rd = cmd.ExecuteReader())
            {
                ret = rd.HasRows;
            }

            return ret;
        }
        #endregion
        #region Select
        static byte[] ba = new byte[5 * 1024 * 1024];
        public static List<T> Select<T>(SqliteCommand cmd, string TableName, string Where) where T : SQLiteData
        {
            List<T> ret = null;

            string sql = "SELECT * FROM `" + TableName + "`";
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
                            #region Bool
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
                            #region Integer
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
                            #endregion
                            #region Real
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
                            #endregion
                            #region Text
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
                                    var len = rd.GetBytes(idx, 0, ba, 0, ba.Length);

                                    var ls = new List<int>();
                                    for (int i = 0; i < len; i += 4) ls.Add(BitConverter.ToInt32(ba, i));

                                    pi.SetValue(v, ls.ToArray(), null);
                                }
                            }
                            #endregion
                            #region uInt[]
                            if (tp == typeof(uint[]))
                            {
                                if (!rd.IsDBNull(idx))
                                {
                                    var len = rd.GetBytes(idx, 0, ba, 0, ba.Length);

                                    var ls = new List<uint>();
                                    for (int i = 0; i < len; i += 4) ls.Add(BitConverter.ToUInt32(ba, i));

                                    pi.SetValue(v, ls.ToArray(), null);
                                }
                            }
                            #endregion
                            #region byte[]
                            if (tp == typeof(byte[]))
                            {
                                if (!rd.IsDBNull(idx))
                                {
                                    var len = rd.GetBytes(idx, 0, ba, 0, ba.Length);

                                    var r = new byte[len]; Array.Copy(ba, 0, r, 0, Convert.ToInt32(len));
                                    pi.SetValue(v, r, null);
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
        public static void Update<T>(SqliteCommand cmd, string TableName, params T[] Datas) where T : SQLiteData
        {
            foreach (var Data in Datas)
            {
                if (Data != null)
                {
                    string sql = "UPDATE `" + TableName + "` SET ";
                    string where = " WHERE `Id`=" + Data.Id;

                    var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)));
                    foreach (var pi in props) sql += " `" + pi.Name + "` = @" + pi.Name + ",";

                    cmd.CommandText = sql.Substring(0, sql.Length - 1) + where;
                    foreach (var pi in props) cmd.Parameters.AddWithValue("@" + pi.Name, GetValue(Data, pi));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #region Insert
        public static void Insert<T>(SqliteCommand cmd, string TableName, params T[] Datas) where T : SQLiteData
        {
            foreach (var Data in Datas)
            {
                var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !Attribute.IsDefined(x, typeof(SqlIgnoreAttribute)));

                string s_insert_in = string.Concat(props.Select(x => " `" + x.Name + "`,").ToArray());
                string s_values_in = string.Concat(props.Select(x => " @" + x.Name + ",").ToArray());

                s_values_in = s_values_in.Substring(0, s_values_in.Length - 1);
                s_insert_in = s_insert_in.Substring(0, s_insert_in.Length - 1);

                string s_insert = "INSERT INTO `" + TableName + "` (" + s_insert_in + " )";
                string s_values = "VALUES (" + s_values_in + " )";
                string s_sql = s_insert + "\r\n" + s_values;

                cmd.CommandText = s_sql;
                foreach (var pi in props) cmd.Parameters.AddWithValue("@" + pi.Name, GetValue(Data, pi));
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region Delete
        public static void Delete<T>(SqliteCommand cmd, string TableName, params T[] Datas) where T : SQLiteData
        {
            if (Datas.Length > 0)
            {
                string sql = "DELETE FROM `" + TableName + "`\r\nWHERE ";
                for (int i = 0; i < Datas.Length; i++) sql += "ID = " + Datas[i].Id + (i < Datas.Length - 1 ? " OR " : "");

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(SqliteCommand cmd, string TableName, List<int> Ids)
        {
            if (Ids.Count > 0)
            {
                string sql = "DELETE FROM `" + TableName + "`\r\nWHERE ";
                for (int i = 0; i < Ids.Count; i++) sql += "ID = " + Ids[i] + (i < Ids.Count - 1 ? " OR " : "");

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
        #region GetTypeText
        static string GetTypeText(PropertyInfo pi)
        {
            string ret = null;
            var tp = pi.PropertyType;
            if (tp == typeof(bool)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(bool?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(byte)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(short)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(ushort)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(int)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(uint)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(long)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(ulong)) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(sbyte?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(byte?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(short?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(ushort?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(int?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(uint?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(long?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(ulong?)) ret = "INTEGER DEFAULT NULL";
            else if (tp == typeof(float)) ret = "REAL NOT NULL DEFAULT 0";
            else if (tp == typeof(double)) ret = "REAL NOT NULL DEFAULT 0";
            else if (tp == typeof(decimal)) ret = "REAL NOT NULL DEFAULT 0";
            else if (tp == typeof(float?)) ret = "REAL DEFAULT NULL";
            else if (tp == typeof(double?)) ret = "REAL DEFAULT NULL";
            else if (tp == typeof(decimal?)) ret = "REAL DEFAULT NULL";
            else if (tp == typeof(string)) ret = "TEXT DEFAULT NULL";
            else if (tp == typeof(DateTime)) ret = "TEXT NOT NULL DEFAULT '2000-01-01 00:00:00'";
            else if (tp == typeof(DateTime?)) ret = "TEXT DEFAULT NULL";
            else if (tp == typeof(TimeSpan)) ret = "TEXT NOT NULL DEFAULT '00:00:00'";
            else if (tp == typeof(TimeSpan?)) ret = "TEXT DEFAULT NULL";
            else if (tp.IsEnum) ret = "INTEGER NOT NULL DEFAULT 0";
            else if (tp == typeof(int[])) ret = "BLOB DEFAULT NUL";
            else if (tp == typeof(uint[])) ret = "BLOB DEFAULT NUL";
            else if (tp == typeof(byte[])) ret = "BLOB DEFAULT NUL";
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
                ret = v.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (tp == typeof(DateTime?))
            {
                var v = (DateTime?)pi.GetValue(Data, null);
                ret = v.HasValue ? v.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;
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
                    ret = ls.ToArray();
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
                }
            }
            else if (tp == typeof(byte[])) ret = pi.GetValue(Data, null);

            return ret;
        }
        #endregion
    }
    #endregion
    #region class : SQLiteData
    public class SQLiteData { public int Id { get; set; } }
    #endregion

}
