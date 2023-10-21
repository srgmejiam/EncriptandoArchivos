using System;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;
using System.Text.Unicode;
using System.Text;

namespace EncryptDoc
{
    public class Encrypt
    {
        public static void EncryptFile(string inputFile, string outputFile, string password)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;

                // Generar una sal aleatoria utilizando RandomNumberGenerator
                byte[] salt = new byte[16];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // Derivación de la clave desde la contraseña utilizando PBKDF2 con la sal generada
                using (Rfc2898DeriveBytes keyDerivation = new(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = keyDerivation.GetBytes(aesAlg.BlockSize / 8);
                }

                using FileStream fsInput = new(inputFile, FileMode.Open);
                using FileStream fsOutput = new(outputFile, FileMode.Create);
                using ICryptoTransform encryptor = aesAlg.CreateEncryptor();
                using CryptoStream cryptoStream = new(fsOutput, encryptor, CryptoStreamMode.Write);
                // Escribir la sal en el archivo de salida antes de los datos cifrados
                fsOutput.Write(salt, 0, salt.Length);

                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = fsInput.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
            }
        }

        public static void DecryptFile(string inputFile, string outputFile, string password)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;

                // Leer la sal del archivo cifrado
                byte[] salt = new byte[16];
                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
                {
                    fsInput.Read(salt, 0, salt.Length);

                    // Derivación de la clave desde la contraseña utilizando PBKDF2 con la sal leída
                    using (Rfc2898DeriveBytes keyDerivation = new (password, salt, 10000, HashAlgorithmName.SHA256))
                    {
                        aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);
                        aesAlg.IV = keyDerivation.GetBytes(aesAlg.BlockSize / 8);
                    }

                    using (FileStream fsOutput = new (outputFile, FileMode.Create)) // Abre el archivo de salida en modo de creación
                    using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                    using (CryptoStream cryptoStream = new (fsInput, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fsOutput.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }
    }
}
