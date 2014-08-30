using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public static class SSLValidator
{
	private static bool OnValidateCertificate (object sender, 
		X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		return true;
	}

	public static void OverrideValidation ()
	{
		ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;
		ServicePointManager.Expect100Continue = true;
	}
}