using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Contellation.Custom.Controls.Datas.DataGrids.Generic
{
    public static class JsonConvert
    {
        private static DataContractJsonSerializerSettings GetSettings() =>
            new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffffff")
            };

        public static T Deserialize<T>(string filename)
        {
            try
            {
                if (!File.Exists(filename)) { return (T)default; }

                // ReSharper disable once ConvertToUsingDeclaration
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var ser = new DataContractJsonSerializer(typeof(T), GetSettings());
                    return (T)ser.ReadObject(fs);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JsonConvert.Deserialize error : {ex.Message}");
                throw;
            }
        }

        public static long Serialize<T>(string filename, T data)
        {
            try
            {
                // ReSharper disable once ConvertToUsingDeclaration
                using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (var writer =
                           JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, false, "  "))
                    {
                        var ser = new DataContractJsonSerializer(typeof(T), GetSettings());
                        ser.WriteObject(writer, data);
                        writer.Flush();
                        return fs.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JsonConvert.Serialize error : {ex.Message}");
                throw;
            }
        }
    }
}
