using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Readers
{
    class JsonReader : IReader
    {
        /// <summary>
        /// Read settings in json format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T Read<T>(string filePath)
        {
            try
            {
                byte[] buffer;
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }

                string json = Encoding.Default.GetString(buffer);
                if (NewtonjsonEtensions.ValidateJson(json))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch
            {

            }
            return default(T);
        }

        /// <summary>
        /// Read settings in json format asynchronous
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<T> ReadAsync<T>(string filePath)
        {
            try
            {
                byte[] buffer;
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    buffer = new byte[fs.Length];
                    await fs.ReadAsync(buffer, 0, buffer.Length);
                }

                string json = Encoding.Default.GetString(buffer);
                if (NewtonjsonEtensions.ValidateJson(json))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch
            {

            }
            return default(T);
        }
    }
}
