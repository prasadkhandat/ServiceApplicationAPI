using LogLibrary;
using System;
using DataLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Security.Cryptography;

namespace LicenseLibrary
{
    public class LicenseClass
    {
        string Passphrase = "APKPRK@1987";

        public LoginDetails authenticate(String username, String password)
        {
            LoginDetails ld = new LoginDetails();
            try
            {
                ld.isAuthenticated = false;
                string query = "select * from user_master where uname='" + username + "'";
                DataSQL sql = new DataSQL();
                DataTable dt = sql.getData(query);
                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["Password"].ToString().Equals(password))
                    {
                        ld.isAuthenticated = true;
                        ld.FirstName = dt.Rows[0]["FirstName"].ToString();
                        ld.LastName = dt.Rows[0]["LastName"].ToString();
                        ld.Email = dt.Rows[0]["Email"].ToString();
                        ld.Phone = dt.Rows[0]["Phone"].ToString();
                        ld.Role = dt.Rows[0]["Role"].ToString();
                        ld.UID = dt.Rows[0]["User_ID"].ToString();
                        //ld.AuthToken = GenerateAuthToken(username);
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.AppendLog("Error ", "LicenseLibrary >> LicenseClass >> authenticate", ex.Message);
            }
            return ld;
        }

        public string GenerateAuthToken(string UserName)
        {
            string Token = EncryptString(BCrypt.HashPassword(UserName, BCrypt.GenerateSalt(4)) + ":::" + UserName + ":::" + DateTime.Now.AddHours(24).ToString("yyyy-MM-dd HH:mm:ss"));
            //string Token = EncryptString(MasterID.ToLower().Trim() + ":::" + UserName + ":::" + DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss"));
            return Token;
        }

        public bool CheckToken(string UserName, string Token)
        {
            bool isAuthenticated = false;
            try
            {
                if (Token != null)
                {
                    string tmpstring = DecryptString(Token);
                    string[] ResultString = tmpstring.Split(new string[] { ":::" }, StringSplitOptions.None);
                    if (ResultString.Length == 3)
                    {
                        if (ResultString[0].Length > 0)
                        {
                            if (BCrypt.CheckPassword(UserName.ToLower().Trim(), ResultString[0]) == true)
                            {
                                if (UserName.ToLower().Trim() == ResultString[1].ToLower().Trim() && ResultString[1].Length > 0)
                                {
                                    DateTime tmpdate = new DateTime();
                                    if (DateTime.TryParse(ResultString[2], out tmpdate) == true)
                                    {
                                        if (tmpdate >= DateTime.Now)
                                        {
                                            isAuthenticated = true;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return isAuthenticated;
        }

        private string EncryptString(string Message)
        {
            
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        private string DecryptString(string Message)
        {
            
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }
    }

    public class LoginDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        //public string AuthToken { get; set; }
        public bool isAuthenticated { get; set; }
        public string UID { get; set; }
    }
}
