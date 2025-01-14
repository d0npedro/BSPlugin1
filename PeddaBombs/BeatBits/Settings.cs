using GameChanger;
using UnityEngine;

namespace BeatBits
{
    public static class Settings
    {
    	public static UserInt Port = new UserInt("Port", 4649);

    	public static UserColor TshirtSponsorNameColor = new UserColor("TshirtSponsorNameColor", new Color(1f, 1f, 1f, 1f));

    	public static UserFloat TshirtSponsorNameDistance = new UserFloat("TshirtSponsorNameDistance", 1f);

    	public static UserFloat TshirtSponsorNameVerticalOffset = new UserFloat("TshirtSponsorNameVerticalOffset", 1f);

    	public static UserInt SubscriberParticlesPerSecond = new UserInt("SubscriberParticlesPerSecond", 200);

    	public static UserInt BitsTier0Duration = new UserInt("BitsTier0Duration", 1);

    	public static UserInt BitsTier0CubeCost = new UserInt("BitsTier0CubeCost", 1);

    	public static UserFloat BitsTier0NameSize = new UserFloat("BitsTier0NameSize", 0.25f);

    	public static UserInt BitsTier0BurstParticles = new UserInt("BitsTier0BurstParticles", 100);

    	public static UserInt BitsTier1Start = new UserInt("BitsTier1Start", 1);

    	public static UserInt BitsTier1CubeCost = new UserInt("BitsTier1CubeCost", 5);

    	public static UserInt BitsTier1Duration = new UserInt("BitsTier1Duration", 2);

    	public static UserFloat BitsTier1NameSize = new UserFloat("BitsTier1NameSize", 0.5f);

    	public static UserInt BitsTier1BurstParticles = new UserInt("BitsTier1BurstParticles", 250);

    	public static UserInt BitsTier2Start = new UserInt("BitsTier2Start", 1000);

    	public static UserInt BitsTier2CubeCost = new UserInt("BitsTier2CubeCost", 50);

    	public static UserInt BitsTier2Duration = new UserInt("BitsTier2Duration", 3);

    	public static UserFloat BitsTier2NameSize = new UserFloat("BitsTier2NameSize", 1f);

    	public static UserInt BitsTier2BurstParticles = new UserInt("BitsTier2BurstParticles", 750);

    	public static UserInt BitsTier3Start = new UserInt("BitsTier3Start", 5000);

    	public static UserInt BitsTier3CubeCost = new UserInt("BitsTier3CubeCost", 250);

    	public static UserInt BitsTier3Duration = new UserInt("BitsTier3Duration", 5);

    	public static UserFloat BitsTier3NameSize = new UserFloat("BitsTier3NameSize", 1.5f);

    	public static UserInt BitsTier3BurstParticles = new UserInt("BitsTier3BurstParticles", 2000);

    	public static UserInt BitsTier4Start = new UserInt("BitsTier4Start", 10000);

    	public static UserInt BitsTier4CubeCost = new UserInt("BitsTier4CubeCost", 500);

    	public static UserInt BitsTier4Duration = new UserInt("BitsTier4Duration", 10);

    	public static UserFloat BitsTier4NameSize = new UserFloat("BitsTier4NameSize", 2f);

    	public static UserInt BitsTier4BurstParticles = new UserInt("BitsTier4BurstParticles", 5000);

    	public static UserInt SubsTier1Duration = new UserInt("SubsTier1Duration", 10);

    	public static UserInt SubsTier2Duration = new UserInt("SubsTier2Duration", 20);

    	public static UserInt SubsTier3Duration = new UserInt("SubsTier3Duration", 30);

    	public static UserInt SubsTier4Duration = new UserInt("SubsTier4Duration", 40);

    	public static UserFloat NewFollowerScrollSpeed = new UserFloat("NewFollowerScrollSpeed", 500f);

    	public static UserColor bitsColor1 = new UserColor("bitsColor1", new Color(1f, 1f, 1f, 1f));

    	public static UserColor bitsColor100 = new UserColor("bitsColor100", new Color(0.5724139f, 0f, 1f, 1f));

    	public static UserColor bitsColor1000 = new UserColor("bitsColor1000", new Color(0f, 1f, 0f, 1f));

    	public static UserColor bitsColor5000 = new UserColor("bitsColor5000", new Color(0f, 0f, 1f, 1f));

    	public static UserColor bitsColor10000 = new UserColor("bitsColor10000", new Color(1f, 0f, 0f, 1f));

    	public static UserBool enableBits = new UserBool("enableBits", defaultValue: true);

    	public static UserBool enableSubs = new UserBool("enableSubs", defaultValue: true);

    	public static UserBool debug = new UserBool("debug", defaultValue: false);

    	public static void Load()
    	{
    		UserSetting.LoadSettings();
    	}
    }
}
