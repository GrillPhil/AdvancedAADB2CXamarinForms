namespace AdvancedAADB2C.Xamarin
{
    public class Constants
    {
        static readonly string inviteEndpoint = "http://10.0.2.2:7071/api/invite";
        static readonly string tenantName = "{Your Tenant Name}";
        static readonly string tenantId = "{Your Tenant Name}.onmicrosoft.com";
        static readonly string clientId = "{Your Client Id}";
        static readonly string policySignin = "B2C_1_Signin";
        static readonly string policySignup = "B2C_1_Signup";
        static readonly string policyPassword = "B2C_1_Passwordreset";
        static readonly string iosKeychainSecurityGroup = "com.grillphil.advancedaadb2c";

        static readonly string[] scopes = { "openid" };
        static readonly string authorityBase = $"https://{tenantName}.b2clogin.com/tfp/{tenantId}/";

        public static string ClientId
        {
            get
            {
                return clientId;
            }
        }
        public static string AuthoritySignin
        {
            get
            {
                return $"{authorityBase}{policySignin}";
            }
        }
        public static string AuthoritySignup
        {
            get
            {
                return $"{authorityBase}{policySignup}";
            }
        }
        public static string AuthorityPasswordReset
        {
            get
            {
                return $"{authorityBase}{policyPassword}";
            }
        }
        public static string[] Scopes
        {
            get
            {
                return scopes;
            }
        }
        public static string IosKeychainSecurityGroups
        {
            get
            {
                return iosKeychainSecurityGroup;
            }
        }
        public static string InviteEndpoint
        {
            get
            {
                return inviteEndpoint;
            }
        }
    }
}
