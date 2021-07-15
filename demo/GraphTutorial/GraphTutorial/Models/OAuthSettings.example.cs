namespace GraphTutorial.Models
{
    public static class OAuthSettings
    {
        public const string ApplicationId = "YOUR_APP_ID_HERE";
        public const string Scopes = "User.Read MailboxSettings.Read Calendars.ReadWrite";
        public const string RedirectUri = "msauth://com.companyname.GraphTutorial";
    }
}