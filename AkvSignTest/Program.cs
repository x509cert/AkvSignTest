using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Security.Cryptography;

string? keyVaultUrl = Environment.GetEnvironmentVariable("AKV_SIGNING_URI");
if (keyVaultUrl == null) {
    Console.WriteLine("Please set the AkvSigningUri environment variable.");
    return;
}

var cred = new AzureCliCredential();
var client = new KeyClient(new Uri(keyVaultUrl), cred);

string keyName = "key-sign-mikehow-tst";
KeyVaultKey key = client.GetKey(keyName);

var cryptoClient = new CryptographyClient(key.Id, cred);

string dataToSign = "Hello, AKV Digital Signature World!";

var dataBytes = Encoding.UTF8.GetBytes(dataToSign);
var hashedData = SHA256.HashData(dataBytes);
SignResult signResult = cryptoClient.Sign(SignatureAlgorithm.RS256, hashedData);

Console.WriteLine(Convert.ToBase64String(signResult.Signature));
