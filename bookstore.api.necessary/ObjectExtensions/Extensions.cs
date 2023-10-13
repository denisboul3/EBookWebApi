using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookstore.api.Extensions;

public static class Extensions
{
    public static bool IsNull(this object item)
    {
        return item == null;
    }

    public static bool IsValid(this Guid obj, string guid = "")
    {
        if (string.IsNullOrEmpty(guid))
        {
            return obj != Guid.Empty;
        }

        bool isValid = Guid.TryParse(guid, out _);

        return isValid;
    }

    public static string ToSha3(this string item)
    {
        var hashAlgorithm = new Org.BouncyCastle.Crypto.Digests.Sha3Digest(512);

        byte[] input = Encoding.ASCII.GetBytes(item);

        hashAlgorithm.BlockUpdate(input, 0, input.Length);

        byte[] result = new byte[64]; // 512 / 8 = 64
        hashAlgorithm.DoFinal(result, 0);

        string hashString = BitConverter.ToString(result);
        return hashString.Replace("-", "").ToLowerInvariant();

    }
}
