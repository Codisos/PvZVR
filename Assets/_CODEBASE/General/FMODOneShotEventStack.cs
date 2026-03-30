using UnityEngine;
using FMODUnity;

public class FMODOneShotEventStack : MonoBehaviour
{
    public EventReference[] sounds;

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShotAttached(sound,gameObject);
    }

    //pouze pro zbran a change mode, nedelat v ostrem projektu tohle je last minute blbost
    public void PlayOneShotChangeMode(EventReference sound, float parameterValue)
    {
        FMOD.Studio.PARAMETER_ID parameterID;

        FMOD.Studio.EventInstance Sound = RuntimeManager.CreateInstance(sound);
        Sound.start();

        FMOD.Studio.EventDescription soundDescription;
        Sound.getDescription(out soundDescription);

        FMOD.Studio.PARAMETER_DESCRIPTION soundParameterDescription;
        soundDescription.getParameterDescriptionByName("Mode", out soundParameterDescription);

        parameterID = soundParameterDescription.id;

        Sound.setParameterByID(parameterID, parameterValue);

        RuntimeManager.PlayOneShotAttached(sound, gameObject);
    }

}
