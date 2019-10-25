namespace Sales.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string prvTokenType = "TokenType";
        private const string prvAccessToke = "AccessToke";
        private const string prvIsRemembered = "IsRemembered";
        private const string prvEmail = "Email";
        private const string prvPwd = "Pwd";

        private static readonly string SettingsDefaultValue = string.Empty;
        private static readonly bool  SettingsDefaultBool = false;

        #endregion


        public static string TokenType
        {
            get { return AppSettings.GetValueOrDefault(prvTokenType, SettingsDefaultValue); }
            set { AppSettings.AddOrUpdateValue(prvTokenType, value); }
        }

        public static string AccessToke
        {
            get { return AppSettings.GetValueOrDefault(prvAccessToke, SettingsDefaultValue); }
            set { AppSettings.AddOrUpdateValue(prvAccessToke, value); }
        }

        public static bool IsRemembered
        {
            get { return AppSettings.GetValueOrDefault(prvIsRemembered, SettingsDefaultBool); }
            set { AppSettings.AddOrUpdateValue(prvIsRemembered, value); }
        }

        public static string Email
        {
            get { return AppSettings.GetValueOrDefault(prvEmail, SettingsDefaultValue); }
            set { AppSettings.AddOrUpdateValue(prvEmail, value); }
        }

        public static string PwdEMail
        {
            get { return AppSettings.GetValueOrDefault(prvPwd, SettingsDefaultValue); }
            set { AppSettings.AddOrUpdateValue(prvPwd, value); }
        }
    }
}
