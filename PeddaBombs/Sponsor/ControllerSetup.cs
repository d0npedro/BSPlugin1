using System.IO;
using BeatBits;
using GameChanger;
using UnityEngine;

namespace Sponsor
{
    public static class ControllerSetup
    {
    	private static Texture2D sponsorTexture;

    	private static Material sponsorMaterial;

    	public static void OnApplicationStart()
    	{
    		if (sponsorTexture == null)
    		{
    			LoadSponsorshipBanner();
    		}
    	}

    	private static void LoadSponsorshipBanner()
    	{
    		string path = "LIVStreamerKitSponsor.png";
    		string path2 = Path.Combine(Utils.GetAssemblyPath(), path);
    		if (File.Exists(path2))
    		{
    			byte[] array = File.ReadAllBytes(path2);
    			sponsorTexture = new Texture2D(2, 2);
    			sponsorTexture.LoadImage(array);
    			sponsorTexture.Apply();
    			Plugin.Log("Loaded sponsor banner size: {0}", array.Length);
    			Plugin.Log("Loaded sponsor banner, width: {0}, height: {1}", sponsorTexture.width, sponsorTexture.height);
    			sponsorMaterial = new Material(Shader.Find("Custom/TransparentTexureNoGlow"))
    			{
    				mainTexture = sponsorTexture
    			};
    		}
    	}

    	private static void CreateSponsorshipBanners()
    	{
    		GameObject gameObject = CreateSponsorshipBanner(10.4f, 2.6f, new Vector3(-2.375f, 1.3f, 11.2f), Quaternion.Euler(90f, 90f, 0f));
    		GameObject gameObject2 = CreateSponsorshipBanner(10.4f, 2.6f, new Vector3(2.375f, 1.3f, 11.2f), Quaternion.Euler(90f, -90f, 0f));
    		gameObject.transform.RotateAround(gameObject.transform.position + new Vector3(0f, 0f, 5.2f), Vector3.up, 17f);
    		gameObject2.transform.RotateAround(gameObject.transform.position + new Vector3(0f, 0f, 5.2f), Vector3.up, -17f);
    	}

    	private static GameObject CreateSponsorshipBanner(float width, float height, Vector3 position, Quaternion rotation)
    	{
    		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
    		gameObject.GetComponent<Renderer>().material = sponsorMaterial;
    		gameObject.transform.SetPositionAndRotation(position, rotation);
    		gameObject.transform.localScale = new Vector3(width / 10f, 1f, height / 10f);
    		return gameObject;
    	}
    }
}
