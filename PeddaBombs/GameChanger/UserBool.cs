using BeatBits;
using BS_Utils.Utilities;

namespace GameChanger
{
    public class UserBool : UserSetting
    {
        private string _name;

        private bool _defaultValue;

        private bool _value;

        private bool _autoSave = true;

        public bool value
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

        public UserBool(string name, bool defaultValue, bool autoSave = true, bool autoLoad = true)
        {
            _name = name;
            _defaultValue = (_value = defaultValue);
            _autoSave = autoSave;

            if (autoLoad)
            {
                Load();
            }
        }

        public override void Load()
        {
            // Load value from BS Utils INI system
            var config = new Config(Plugin.PluginName);
            _value = config.GetBool("Settings", _name, _defaultValue);
        }

        public override void Save()
        {
            // Save value to BS Utils INI system
            var config = new Config(Plugin.PluginName);
            config.SetBool("Settings", _name, _value);
        }
    }
}
