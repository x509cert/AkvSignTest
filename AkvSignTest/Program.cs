using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Security.Cryptography;

string keyVaultUrl = "https://xyzzy.vault.azure.net/";
string keyName = "key-sign-xyzzy-tst";
string dataToSign = "Hello, World!";

var client = new KeyClient(new Uri(keyVaultUrl), new AzureCliCredential());
KeyVaultKey key = client.GetKey(keyName);
var cryptoClient = new CryptographyClient(key.Id, new AzureCliCredential());
byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSign);

using (var hasher = SHA256.Create())
{
    byte[] hashedData = hasher.ComputeHash(dataBytes);
    SignResult signResult = cryptoClient.Sign(SignatureAlgorithm.RS256, hashedData);

    Console.WriteLine(Convert.ToBase64String(signResult.Signature));
}