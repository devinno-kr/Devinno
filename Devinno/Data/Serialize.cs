using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Devinno.Data
{
    public class Serialize
    {
        #region xmlSerialze
        public static void XmlSerializeToFile(string Path, object obj, Type type)
        {
            XmlSerializer xmlser = new XmlSerializer(type);
            XmlWriterSettings set = new XmlWriterSettings();
            set.Indent = true;
            XmlWriter writer = XmlWriter.Create(Path, set);
            xmlser.Serialize(writer, obj);
            writer.Close();
        }

        public static object XmlDeserializeFromFile(string Path, Type type)
        {
            object ret = null;
            if (File.Exists(Path))
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    byte[] data = new byte[fs.Length];
                    int nlen = fs.Read(data, 0, data.Length);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                    System.IO.TextReader reader = new System.IO.StreamReader(ms);
                    XmlSerializer xmlser = new XmlSerializer(type);
                    ret = xmlser.Deserialize(reader);
                    reader.Close();
                    ms.Close();
                }
            }
            return ret;
        }

        public static byte[] XmlSerialize(object obj, Type type)
        {
            XmlSerializer xmlser = new XmlSerializer(type);
            StringBuilder resultXml = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(resultXml);
            xmlser.Serialize(writer, obj);
            byte[] data = Encoding.Default.GetBytes(resultXml.ToString());
            return data;
        }

        public static object XmlDeserialize(byte[] rawdata, Type type)
        {
            MemoryStream ms = new MemoryStream(rawdata);
            TextReader reader = new StreamReader(ms, Encoding.Default);
            XmlSerializer xmlser = new XmlSerializer(type);
            object obj = xmlser.Deserialize(reader);
            reader.Close();
            ms.Close();
            return obj;
        }
        #endregion

        #region Struct Serialize
        public static byte[] RawSerialize(object obj)
        {
            int rawSize = Marshal.SizeOf(obj);
            byte[] rawData = new byte[rawSize];
            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            IntPtr buf = handle.AddrOfPinnedObject();
            Marshal.StructureToPtr(obj, buf, false);
            handle.Free();
            return rawData;
        }

        public static object RawDeserialize(byte[] rawdata, Type type)
        {
            int rawSize = Marshal.SizeOf(type);
            if (rawSize > rawdata.Length) return null;
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            IntPtr buf = handle.AddrOfPinnedObject();
            object retobj = Marshal.PtrToStructure(buf, type);
            handle.Free();
            return retobj;
        }

        public static object RawDeserialize(byte[] ba, int offset, int size, Type type)
        {
            var rawdata = new byte[size];
            Array.Copy(ba, offset, rawdata, 0, size);

            int rawSize = Marshal.SizeOf(type);
            if (rawSize > rawdata.Length) return null;
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            IntPtr buf = handle.AddrOfPinnedObject();
            object retobj = Marshal.PtrToStructure(buf, type);
            handle.Free();
            return retobj;
        }

        public static T RawDeserialize<T>(byte[] rawdata) where T : struct
        {
            int rawSize = Marshal.SizeOf(typeof(T));
            if (rawSize > rawdata.Length) return default(T);
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            IntPtr buf = handle.AddrOfPinnedObject();
            T retobj = (T)Marshal.PtrToStructure(buf, typeof(T));
            handle.Free();
            return retobj;
        }

        public static T RawDeserialize<T>(byte[] ba, int offset, int size)
        {
            var rawdata = new byte[size];
            Array.Copy(ba, offset, rawdata, 0, size);

            int rawSize = Marshal.SizeOf(typeof(T));
            if (rawSize > rawdata.Length) return default(T);
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            IntPtr buf = handle.AddrOfPinnedObject();
            T retobj = (T)Marshal.PtrToStructure(buf, typeof(T));
            handle.Free();
            return retobj;
        }

        #endregion

        #region JsonSerialze
        #region BitmapConverter
        public class BitmapConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Bitmap);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                Bitmap bm = null;
                if (reader.Value != null)
                {
                    using (var m = new MemoryStream(Convert.FromBase64String((string)reader.Value)))
                    {
                        bm = (Bitmap)Bitmap.FromStream(m);
                    }
                }
                return bm;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                Bitmap bmp = (Bitmap)value;
                using (MemoryStream m = new MemoryStream())
                {
                    bmp.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                    writer.WriteValue(Convert.ToBase64String(m.ToArray()));
                }
            }
        }
        #endregion

        public static void JsonSerializeToFile(string Path, object obj, List<JsonConverter> cs = null)
        {
            File.WriteAllText(Path, JsonSerialize(obj, cs));
        }
        public static T JsonDeserializeFromFile<T>(string Path, List<JsonConverter> cs = null)
        {
            if (File.Exists(Path)) return JsonDeserialize<T>(File.ReadAllText(Path), cs);
            else return default(T);
        }

        public static string JsonSerialize(object obj, List<JsonConverter> cs = null)
        {
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.Converters.Add(new BitmapConverter());
            if (cs != null) foreach (var v in cs) set.Converters.Add(v);
            return JsonConvert.SerializeObject(obj, set);
        }
        public static object JsonDeserialize(string json, Type type, List<JsonConverter> cs = null)
        {
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.Converters.Add(new BitmapConverter());
            if (cs != null) foreach (var v in cs) set.Converters.Add(v);
            return JsonConvert.DeserializeObject(json, type, set);
        }
        public static T JsonDeserialize<T>(string json, List<JsonConverter> cs = null)
        {
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.Converters.Add(new BitmapConverter());
            if (cs != null) foreach (var v in cs) set.Converters.Add(v);
            return JsonConvert.DeserializeObject<T>(json, set);
        }


        public static void JsonSerializeWithTypeToFile(string Path, object obj, List<JsonConverter> cs = null)
        {
            File.WriteAllText(Path, JsonSerializeWithType(obj, cs));
        }
        public static T JsonDeserializeWithTypeFromFile<T>(string Path, List<JsonConverter> cs = null)
        {
            if (File.Exists(Path)) return JsonDeserializeWithType<T>(File.ReadAllText(Path), cs);
            else return default(T);
        }

        public static string JsonSerializeWithType(object obj, List<JsonConverter> cs = null)
        {
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.Converters.Add(new BitmapConverter());
            if (cs != null) foreach (var v in cs) set.Converters.Add(v);
            set.TypeNameHandling = TypeNameHandling.All;
            set.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.SerializeObject(obj, set);
        }
        public static T JsonDeserializeWithType<T>(string json, List<JsonConverter> cs = null)
        {
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.Converters.Add(new BitmapConverter());
            if (cs != null) foreach (var v in cs) set.Converters.Add(v);
            set.TypeNameHandling = TypeNameHandling.All;
            set.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<T>(json, set);
        }
        #endregion
    }
}
