using UnityEngine;
using FMODUnity;

public class FMODMusic : MonoBehaviour
{
    public EventReference music;
    public IntVariable ScriptableSwitch;

    FMOD.Studio.EventInstance _sound;
    FMOD.Studio.PARAMETER_ID _parameterID;

    private void Start()
    {

        _sound = RuntimeManager.CreateInstance(music);
        _sound.start();

        FMOD.Studio.EventDescription soundDescription;
        _sound.getDescription(out soundDescription);

        FMOD.Studio.PARAMETER_DESCRIPTION soundParameterDescription;
        soundDescription.getParameterDescriptionByName("MusicEnabled", out soundParameterDescription);

        _parameterID = soundParameterDescription.id;

        _sound.setParameterByID(_parameterID, ScriptableSwitch.Value);

        RuntimeManager.PlayOneShot(music);

        
    }

    public void SetMusic()
    {

        switch (ScriptableSwitch.Value)
        {
            case 0:
                ScriptableSwitch.Value = 1;
                break;
            case 1:
                ScriptableSwitch.Value = 0;
                break;
        }

        _sound.setParameterByID(_parameterID, ScriptableSwitch.Value);
    }

    public void EndMusic()
    {
        _sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _sound.release();
    }
}
