using BeatBits;
using BS_Utils.Utilities;

namespace GameChanger
{
    public class UserInt : UserSetting
    {
        private string _name;

        private int _defaultValue;

        private int _value;

        private bool _autoSave = true;

        public int value
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

        public UserInt(string name, int defaultValue, bool autoSave = true, bool autoLoad = true)
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
            // Load the integer value from the INI file
            var config = new Config(Plugin.PluginName);
            _value = config.GetInt("Settings", _name, _defaultValue);
        }

        public override void Save()
        {
            // Save the integer value to the INI file
            var config = new Config(Plugin.PluginName);
            config.SetInt("Settings", _name, _value);
        }
    }
}
