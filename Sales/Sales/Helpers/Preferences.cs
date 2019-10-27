namespace Sales.Helpers
{
    //    using Plugin.Settings;
    //    using Plugin.Settings.Abstractions;
    using Xamarin.Essentials;
    public static class Preferences
    {
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
            get { return Xamarin.Essentials.Preferences.Get(prvTokenType, SettingsDefaultValue); }
            set { Xamarin.Essentials.Preferences.Set(prvTokenType, value); }
        }

        public static string AccessToke
        {
            get { return Xamarin.Essentials.Preferences.Get(prvAccessToke, SettingsDefaultValue); }
            set { Xamarin.Essentials.Preferences.Set(prvAccessToke, value); }
        }

        public static bool IsRemembered
        {
            get { return Xamarin.Essentials.Preferences.Get(prvIsRemembered, SettingsDefaultBool); }
            set { Xamarin.Essentials.Preferences.Set(prvIsRemembered, value); }
        }

        public static string Email
        {
            get { return Xamarin.Essentials.Preferences.Get(prvEmail, SettingsDefaultValue); }
            set { Xamarin.Essentials.Preferences.Set(prvEmail, value); }
        }

        public static string PwdEMail
        {
            get { return Xamarin.Essentials.Preferences.Get(prvPwd, SettingsDefaultValue); }
            set { Xamarin.Essentials.Preferences.Set(prvPwd, value); }
        }
    }
}
