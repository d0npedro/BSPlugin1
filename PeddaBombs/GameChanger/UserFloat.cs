using BeatBits;
using BS_Utils.Utilities;

namespace GameChanger
{
    public class UserFloat : UserSetting
    {
        private string _name;

        private float _defaultValue;

        private float _value;

        private bool _autoSave = true;

        public float value
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

        public UserFloat(string name, float defaultValue, bool autoSave = true, bool autoLoad = true)
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
            // Load the float value from the INI file
            var config = new Config(Plugin.PluginName);
            _value = config.GetFloat("Settings", _name, _defaultValue);
        }

        public override void Save()
        {
            // Save the float value to the INI file
            var config = new Config(Plugin.PluginName);
            config.SetFloat("Settings", _name, _value);
        }
    }
}
