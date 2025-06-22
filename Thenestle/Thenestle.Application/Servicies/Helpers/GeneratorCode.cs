using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thenestle.Domain.Interfaces.Services;
using System.Security.Cryptography;

namespace Thenestle.Application.Servicies.Helpers
{
    public class GeneratorCode : IGeneratorCode
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Генерирует 8-символьный инвайт-код используя любые символы
        /// </summary>
        /// <returns></returns>
        public string GenerateCodeAsync()
        {
            var code = new char[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[8];
                rng.GetBytes(buffer);
                for (int i = 0; i < code.Length; i++)
                {
                    // Преобразуем байт в индекс символа из chars
                    code[i] = chars[buffer[i] % chars.Length];
                }
            }
            return new string(code);
        }
    }
}
