using System.Collections.Generic;

namespace GameChanger
{
    public abstract class UserSetting
    {
    	private static List<UserSetting> _settings = new List<UserSetting>();

    	public UserSetting()
    	{
    		_settings.Add(this);
    	}

    	~UserSetting()
    	{
    		_settings.Remove(this);
    	}

    	public abstract void Load();

    	public abstract void Save();

    	public static void LoadSettings()
    	{
    		for (int i = 0; i < _settings.Count; i++)
    		{
    			if (_settings[i] != null)
    			{
    				_settings[i].Load();
    			}
    		}
    	}
    }
}
