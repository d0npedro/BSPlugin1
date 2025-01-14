using BeatBits;
using BS_Utils.Utilities;

namespace GameChanger
{
    public class UserString : UserSetting
    {
        private string _name;

        private string _defaultValue;

        private string _value;

        private bool _autoSave = true;

        public string value
        {
            get => _value;
            set
            {
                _value = value;
                if (_autoSave)
                {
                    Save();
                }
            }
        }

        public UserString(string name, string defaultValue, bool autoSave = true, bool autoLoad = true)
        {
            _name = name;
            _defaultValue = defaultValue;
            _value = defaultValue;
            _autoSave = autoSave;

            if (autoLoad)
            {
                Load();
            }
        }

        public override void Load()
        {
            // Load the string value from the INI file
            var config = new Config(Plugin.PluginName);
            _value = config.GetString("Settings", _name, _defaultValue);
        }

        public override void Save()
        {
            // Save the string value to the INI file
            var config = new Config(Plugin.PluginName);
            config.SetString("Settings", _name, _value);
        }
    }
}
