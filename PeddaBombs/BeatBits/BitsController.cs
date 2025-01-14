using System;
using System.Globalization;
using BeatBits;
using UnityEngine;

    public class BitsController : MonoBehaviour
    {
    	private static int MAX_POOL_OBJECTS = 30;

    	private static BitsControllerEvent controllerEvent;

    	public GameObject bitsHyperCubePrefab;

    	private Action _onComplete;

    	private bool _active;

    	private bool _initialized;

    	private BeatmapObjectManager _beatMapObjectManager;

    	private ObjectPool<BitsHyperCube> bitsHyperCubePool;

    	private CheerEvent _currentCheerEvent;

    	private SabotageEvent _currentSabotageEvent;

    	public void Init()
    	{
    		BitsHyperCube[] array = new BitsHyperCube[MAX_POOL_OBJECTS];
    		for (int i = 0; i < array.Length; i++)
    		{
    			array[i] = UnityEngine.Object.Instantiate(bitsHyperCubePrefab).GetComponent<BitsHyperCube>();
    			array[i].gameObject.SetActive(value: false);
    		}
    		bitsHyperCubePool = new ObjectPool<BitsHyperCube>(array);
    		_initialized = true;
    	}

    	private void TrySpawnHyperCube()
    	{
    		BitsHyperCube bitsHyperCube = bitsHyperCubePool.Next();
    		if (_currentCheerEvent != null)
    		{
    			bitsHyperCube.noteCubeRenderer.SetRendererType(NoteCubeRenderer.RENDERER_TYPE.OUTLINE);
    			bitsHyperCube.Message = _currentCheerEvent.Message;
    			bitsHyperCube.Username = _currentCheerEvent.Viewer.name;
    			bitsHyperCube.Bits = _currentCheerEvent.Bits;
    			bitsHyperCube.TierIndex = _currentCheerEvent.TierIndex;
    			bitsHyperCube.onSpawned = (Action<BitsHyperCube>)Delegate.Combine(bitsHyperCube.onSpawned, new Action<BitsHyperCube>(HyperCubeSpawned));
    			bitsHyperCube.sabotage = false;
    			bitsHyperCube.AddSongEvents(_beatMapObjectManager);
    		}
    		if (_currentSabotageEvent != null)
    		{
    			bitsHyperCube.noteCubeRenderer.SetRendererType(NoteCubeRenderer.RENDERER_TYPE.BOMB);
    			bitsHyperCube.Username = _currentSabotageEvent.Viewer.name;
    			bitsHyperCube.Bits = 1;
    			bitsHyperCube.TierIndex = 3;
    			bitsHyperCube.noteCubeRenderer.SetColor(ParseColorFromHexString(_currentSabotageEvent.Color));
    			bitsHyperCube.onSpawned = (Action<BitsHyperCube>)Delegate.Combine(bitsHyperCube.onSpawned, new Action<BitsHyperCube>(HyperCubeSpawned));
    			bitsHyperCube.sabotage = true;
    			bitsHyperCube.AddSongEvents(_beatMapObjectManager);
    		}
    	}

    	private Color ParseColorFromHexString(string colorcode)
    	{
    		colorcode = colorcode.TrimStart('#');
    		Color32 color = Color.black;
    		if (colorcode.Length == 6)
    		{
    			color = new Color32((byte)int.Parse(colorcode.Substring(0, 2), NumberStyles.HexNumber), (byte)int.Parse(colorcode.Substring(2, 2), NumberStyles.HexNumber), (byte)int.Parse(colorcode.Substring(4, 2), NumberStyles.HexNumber), byte.MaxValue);
    		}
    		else if (colorcode.Length == 8)
    		{
    			color = new Color32((byte)int.Parse(colorcode.Substring(2, 2), NumberStyles.HexNumber), (byte)int.Parse(colorcode.Substring(4, 2), NumberStyles.HexNumber), (byte)int.Parse(colorcode.Substring(6, 2), NumberStyles.HexNumber), (byte)int.Parse(colorcode.Substring(0, 2), NumberStyles.HexNumber));
    		}
    		return color;
    	}

    	private void HyperCubeSpawned(BitsHyperCube hyperCube)
    	{
    		hyperCube.onSpawned = (Action<BitsHyperCube>)Delegate.Remove(hyperCube.onSpawned, new Action<BitsHyperCube>(HyperCubeSpawned));
    		if (controllerEvent != null)
    		{
    			controllerEvent.cubesToSpawn--;
    			if (controllerEvent.cubesToSpawn > 0)
    			{
    				Plugin.Log("Spawning Hyper Cube, cubes left: " + controllerEvent.cubesToSpawn);
    				TrySpawnHyperCube();
    			}
    			else
    			{
    				End();
    			}
    		}
    	}

    	public void Begin(BeatmapObjectManager beatMapObjectManager, object streamerKitEvent, Action onComplete)
    	{
    		_active = true;
    		_beatMapObjectManager = beatMapObjectManager;
    		_currentCheerEvent = streamerKitEvent as CheerEvent;
    		_currentSabotageEvent = streamerKitEvent as SabotageEvent;
    		_onComplete = onComplete;
    		if (controllerEvent == null)
    		{
    			controllerEvent = new BitsControllerEvent();
    			controllerEvent.cubesToSpawn = GetCubesToSpawnCount();
    			Plugin.Log($"Starting to spawn: {controllerEvent.cubesToSpawn} hypercubes");
    		}
    		else
    		{
    			Plugin.Log($"Continuing to spawn: {controllerEvent.cubesToSpawn} hypercubes");
    		}
    		TrySpawnHyperCube();
    	}

    	public void End()
    	{
    		_active = false;
    		controllerEvent = null;
    		if (_onComplete != null)
    		{
    			_onComplete();
    		}
    	}

        private int GetCubesToSpawnCount()
        {
            if (_currentCheerEvent == null)
                return 1;

            switch (_currentCheerEvent.TierIndex)
            {
                case 0:
                    return DivideBitsByCubeCost(_currentCheerEvent.Bits, Settings.BitsTier0CubeCost.value);
                case 1:
                    return DivideBitsByCubeCost(_currentCheerEvent.Bits, Settings.BitsTier1CubeCost.value);
                case 2:
                    return DivideBitsByCubeCost(_currentCheerEvent.Bits, Settings.BitsTier2CubeCost.value);
                case 3:
                    return DivideBitsByCubeCost(_currentCheerEvent.Bits, Settings.BitsTier3CubeCost.value);
                case 4:
                    return DivideBitsByCubeCost(_currentCheerEvent.Bits, Settings.BitsTier4CubeCost.value);
                default:
                    return 1;
            }
        }


        private int DivideBitsByCubeCost(int bits, int cubeCost)
    	{
    		if (cubeCost == 0)
    		{
    			return 1;
    		}
    		return Mathf.FloorToInt(bits / cubeCost);
    	}
    }
