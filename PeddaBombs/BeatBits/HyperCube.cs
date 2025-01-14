using System.Reflection;
using UnityEngine;

namespace BeatBits
{
    public class HyperCube : MonoBehaviour
    {
    	public NoteCubeRenderer noteCubeRenderer;

    	protected NoteController noteController;

    	protected BeatmapObjectManager beatMapObjectManager;

    	public bool ghostNote
    	{
    		get
    		{
    			if (noteController == null)
    			{
    				return false;
    			}
    			GameNoteController gameNoteController = noteController as GameNoteController;
    			if (gameNoteController == null)
    			{
    				return false;
    			}
    			return GetFieldValue<NoteVisualModifierType>(gameNoteController, "_noteVisualModifierType") == NoteVisualModifierType.Ghost;
    		}
    	}

    	public virtual void AddSongEvents(BeatmapObjectManager _beatMapObjectManager)
    	{
    		beatMapObjectManager = _beatMapObjectManager;
    		_beatMapObjectManager.noteDidStartJumpEvent += HandleNoteDidStartJumpEvent;
    		_beatMapObjectManager.noteWasCutEvent += HandleNoteWasCutEvent;
    		_beatMapObjectManager.noteWasMissedEvent += HandleNoteWasMissedEvent;
    	}

    	public virtual void RemoveSongEvents()
    	{
    		beatMapObjectManager.noteDidStartJumpEvent -= HandleNoteDidStartJumpEvent;
    		beatMapObjectManager.noteWasCutEvent -= HandleNoteWasCutEvent;
    		beatMapObjectManager.noteWasMissedEvent -= HandleNoteWasMissedEvent;
    	}

    	protected virtual void HandleNoteDidStartJumpEvent(NoteController noteController)
    	{
    	}

    	protected virtual void HandleNoteWasMissedEvent(NoteController noteController)
    	{
    	}

    	protected virtual void HandleNoteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
    	{
    	}

    	protected T GetFieldValueObject<T>(object target, string fieldName)
    	{
    		FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
    		if (field != null)
    		{
    			return (T)field.GetValue(target);
    		}
    		return default(T);
    	}

    	protected T GetFieldValue<T>(object target, string fieldName)
    	{
    		return (T)target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target);
    	}
    }
}
