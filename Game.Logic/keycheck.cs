using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Game.Logic
{
    public static class KeyCheck
    {
        public static bool Init(String key, String version, String ip, int rand)
        {
            return VerificaKey(key, version, ip, rand);
        }

        private static Boolean VerificaKey(String key, String version, String ip, int rand)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "keycheck");
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("key", key);
                reqparm.Add("version", version);
                reqparm.Add("ip", ip);
                reqparm.Add("rand", rand + "");
                byte[] responsebytes = client.UploadValues("https://antigo.ddtankbr.com.br/key", "POST", reqparm);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                String b = CalculateMD5Hash(key + version + rand);
                return CalculateMD5Hash(key + version + ip + rand) == responsebody;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input + "DDtank Antigo");

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }
    }
}