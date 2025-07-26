using UnityEngine;

public class SoundManager : MonoBehaviour{
    public static SoundManager instance;

    void Start()
    {
        instance = this;
    }
    public void PlaySound(AudioClip clip, float pitchSpread,  SoundType soundType = SoundType.Main, float volume = 1){
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = clip;
        source.pitch = UnityEngine.Random.Range(1 - pitchSpread, 1 + pitchSpread);

        Settings settings = SettingsManager.instance.Settings;
        float sourceVolume = 1;
        switch(soundType){
            case SoundType.Music:
                sourceVolume *= settings.musicVolume;
            break;
            case SoundType.Main:
                sourceVolume *= settings.mainVolume;
            break;
            case SoundType.System:
                sourceVolume *= settings.systemVolume;
            break;
        }
        sourceVolume *= volume;
        
        source.volume = sourceVolume;
        source.Play();
        Destroy(source, clip.length + 0.5f);
    }
}