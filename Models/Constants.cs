
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Models
{
    public class Constants
    {
        public const string RegexStringInput = @"^[a-zA-Z0-9_\-\.\,\s\:]*$";

        public const string RegexStringPasswordInput = @"^[a-zA-Z0-9_\-\.\@\!]+$";

        public const string RegexIntInput = @"^[0-9]+$";

        public const string RegexMobileNoInput = @"^[0-9]{10}$";

        public const string RegexDecimalInput = @"^\d+(\.\d{1,2})?$";

        public const string StringAlphNumeric = "Only Alphabets and Numbers allowed. (A to Z small/capital letter and 0 to 9, undersore, dash and space)";

        public const string StringPwdAlphNumeric = "Only Alphabets and Numbers allowed. (A to Z small/capital letter and 0 to 9, _,-,.,@,! characters is allow)";

        public const string StringNumeric = "Only Numbers allowed.";

        public const string StringMobileNumber = "Mobile must be 10 digit";

        public const string StringDecimal = "Only Decimal allowed. e.g 12.50";

        public const string FormatShortDate = "{0:dd-MMM-yyyy}";

        public const string FormatDateTime = "{0:dd-MMM-yyyy HH:mm:ss}";

        public const string WebApiStatusFail = "Fail";

        public const string WebApiStatusOk = "Ok";

        public static  byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public  static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }


       
    }
}
