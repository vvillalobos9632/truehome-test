 using System.Security.Cryptography;
using System.Text;

namespace Common.Utils.Utils
{
    public static class EncryptionClass
    {
        static string HashToEncryption = "TrueHomeEncrytionKey";
        public static string EncriptarTexto(this string texto, bool usarHashing)
        {
            byte[] arreglo_llave;
            byte[] arreglo_encriptar = UTF8Encoding.UTF8.GetBytes(texto);

            if (usarHashing)
            {
                MD5CryptoServiceProvider obj_hash_md5 = new MD5CryptoServiceProvider();
                arreglo_llave = obj_hash_md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HashToEncryption));
                obj_hash_md5.Clear();
            }
            else
                arreglo_llave = UTF8Encoding.UTF8.GetBytes(HashToEncryption);

            TripleDESCryptoServiceProvider obj_tdes = new TripleDESCryptoServiceProvider();
            obj_tdes.Key = arreglo_llave;
            obj_tdes.Mode = CipherMode.ECB;
            obj_tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform obj_crypto_trans = obj_tdes.CreateEncryptor();
            byte[] texto_encriptado = obj_crypto_trans.TransformFinalBlock(arreglo_encriptar, 0, arreglo_encriptar.Length);
            obj_tdes.Clear();
            return Convert.ToBase64String(texto_encriptado, 0, texto_encriptado.Length).Replace("/", "|JV|").Replace("+", "|||");
        }

        public static string DesencriptarTexto(this string texto_cifrado, bool usarHashing)
        {
            byte[] keyArray;

            byte[] toEncryptArray = Convert.FromBase64String(texto_cifrado.Replace("|JV|", "/").Replace("|||", "+"));

            if (usarHashing)
            {
                MD5CryptoServiceProvider obj_hash_md5 = new MD5CryptoServiceProvider();
                keyArray = obj_hash_md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HashToEncryption));
                obj_hash_md5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(HashToEncryption);
            }

            TripleDESCryptoServiceProvider obj_tdes = new TripleDESCryptoServiceProvider();
            obj_tdes.Key = keyArray;

            obj_tdes.Mode = CipherMode.ECB;
            obj_tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform obj_descifrador = obj_tdes.CreateDecryptor();
            byte[] resultArray = obj_descifrador.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            obj_tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
