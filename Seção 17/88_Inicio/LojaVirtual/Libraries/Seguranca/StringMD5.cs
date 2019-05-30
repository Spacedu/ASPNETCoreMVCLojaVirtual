using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Seguranca
{
    public class StringMD5
    {
        public static string MD5Hash(string texto)
        {
            using (var md5 = MD5.Create())
            {
                var resultado = md5.ComputeHash(Encoding.ASCII.GetBytes(texto));
                return Encoding.ASCII.GetString(resultado);
            }
        }
    }
}
