using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace DreamboxControl
{

    // Singleton class to hold the settings

    public class Settings
    {
        private static Settings instance;

        private Settings () { }

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }

                try
                {
                    // Get the settings for this application.
                    settings = IsolatedStorageSettings.ApplicationSettings;

                }
                catch (Exception e)
                {
                    //Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
                }

                return instance;
            }
        }
        
        // Our isolated storage settings
        public static IsolatedStorageSettings settings;

        // The isolated storage key names of our settings
        const string DreamboxAddressKeyName = "DreamboxAddressSetting";
        const string SSHServerKeyName = "SSHServerSetting";
        const string SSHPortKeyName = "SSHPortSetting";
        const string SSHAccountKeyName = "SSHAccountSetting";
        const string SSHUsePasswordKeyName = "SSHUsePasswordSetting";
        const string SSHPasswordKeyName = "SSHPasswordSetting";
        const string SSHUseKeyKeyName = "SSHUseKeySetting";
        const string SSHKeyKeyName = "SSHKeySetting";
        const string SettingsSavedKeyName = "SettingsSavedSetting";

        // The default values of our settings
        const string DreamboxAddressSettingDefault = "192.168.0.2";
        const string SSHServerSettingDefault = "myhome.dyndns.org";
        const string SSHPortSettingDefault = "22";
        const string SSHAccountSettingDefault = "root";
        const Boolean SSHUsePasswordSettingDefault = true;
        const Boolean SSHUseKeySettingDefault = false;
        const string SSHPasswordSettingDefault = "password";
        const string SSHKeySettingDefault = 
"-----BEGIN RSA PRIVATE KEY-----\n" +
"<your key here>\n" +
"-----END RSA PRIVATE KEY-----\n";
        const Boolean SettingsSavedSettingDefault = false;

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            SettingsSavedSetting = true;
            settings.Save();
        }

        public bool SSHUsePasswordSetting
        {
            get
            {
                return GetValueOrDefault<bool>(SSHUsePasswordKeyName, SSHUsePasswordSettingDefault);
            }
            set
            {
                AddOrUpdateValue(SSHUsePasswordKeyName, value);
            }
        }

        public bool SSHUseKeySetting
        {
            get
            {
                return GetValueOrDefault<bool>(SSHUseKeyKeyName, SSHUseKeySettingDefault);
            }
            set
            {
                AddOrUpdateValue(SSHUseKeyKeyName, value);
            }
        }

        public string DreamboxAddressSetting
        {
            get
            {
                return GetValueOrDefault<string>(DreamboxAddressKeyName, DreamboxAddressSettingDefault);
            }
            set
            {
                AddOrUpdateValue(DreamboxAddressKeyName, value);
            }
        }


        public string SSHServerSetting
        {
            get
            {
                return GetValueOrDefault<string>(SSHServerKeyName, SSHServerSettingDefault);
            }
            set
            {
                AddOrUpdateValue(SSHServerKeyName, value);
            }
        }

        public string SSHPortSetting
        {
            get
            {
                return GetValueOrDefault<string>(SSHPortKeyName, SSHPortSettingDefault);
            }
            set
            {
                AddOrUpdateValue(SSHPortKeyName, value);
            }
        }


        public string SSHAccountSetting
        {
            get
            {
                return GetValueOrDefault<string>(SSHAccountKeyName, SSHAccountSettingDefault);
            }
            set
            {
                AddOrUpdateValue(SSHAccountKeyName, value);
            }
        }


        public string SSHPasswordSetting
        {
            get
            {
                return GetValueOrDefault<string>(SSHPasswordKeyName, SSHPasswordSettingDefault);
            }
            set
            {
                AddOrUpdateValue(SSHPasswordKeyName, value);
            }
        }

        public string SSHKeySetting
        {
            get
            {
                return GetValueOrDefault<string>(SSHKeyKeyName, SSHKeySettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(SSHKeyKeyName, value))
                {
                    Save();
                }
            }
        }

        public bool SettingsSavedSetting
        {
            get
            {
                return GetValueOrDefault<bool>(SettingsSavedKeyName, SettingsSavedSettingDefault);
            }
            set
            {
                AddOrUpdateValue(SettingsSavedKeyName, value);
            }
        }

    }

    // "normal" class for binding the settings into the GUI

    public class SettingsForGUI
    {
        public bool SSHUsePasswordSetting
        {
            get
            {
                return Settings.Instance.SSHUsePasswordSetting;
            }
        }

        public bool SSHUseKeySetting
        {
            get
            {
                return Settings.Instance.SSHUseKeySetting;
            }
        }

        public string DreamboxAddressSetting
        {
            get
            {
                return Settings.Instance.DreamboxAddressSetting;
            }
        }


        public string SSHServerSetting
        {
            get
            {
                return Settings.Instance.SSHServerSetting;
            }
        }

        public string SSHPortSetting
        {
            get
            {
                return Settings.Instance.SSHPortSetting;
            }
        }


        public string SSHAccountSetting
        {
            get
            {
                return Settings.Instance.SSHAccountSetting;
            }
        }


        public string SSHPasswordSetting
        {
            get
            {
                return Settings.Instance.SSHPasswordSetting;
            }
        }      
        
        public string SSHKeySetting
        {
            get
            {
                return Settings.Instance.SSHKeySetting;
            }
        }
    }
}