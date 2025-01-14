using BeatBits;
using BS_Utils.Utilities;
using UnityEngine;

namespace GameChanger
{
    public class UserColor : UserSetting
    {
        private string _name;

        private Color _defaultValue;

        private Color _value;

        private bool _autoSave = true;

        public Color value
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

        public UserColor(string name, Color defaultValue, bool autoSave = true, bool autoLoad = true)
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
            // Load the HEX string from the INI file and convert it to a Color
            var config = new Config(Plugin.PluginName);
            string colorString = config.GetString("Settings", _name, "#" + ColorUtility.ToHtmlStringRGBA(_defaultValue));
            if (!ColorUtility.TryParseHtmlString(colorString, out _value))
            {
                _value = _defaultValue; // Fallback to default if parsing fails
            }
        }

        public override void Save()
        {
            // Convert the Color to a HEX string and save it in the INI file
            var config = new Config(Plugin.PluginName);
            string colorString = "#" + ColorUtility.ToHtmlStringRGBA(_value);
            config.SetString("Settings", _name, colorString);
        }
    }
}
