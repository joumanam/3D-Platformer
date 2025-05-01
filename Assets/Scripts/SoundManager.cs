using System;
using UnityEngine;

public enum SoundType
{
    SHOOT,
    HOLDITEM,
    DROPITEM,
    FOOTSTEP,
    JUMP,
    ENEMYHIT,
    ENEMYDEAD,
    COLLECTCOIN
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{

    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1f, bool random = true, int index = 0)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0)
            return;


        int clipIndex = random ? UnityEngine.Random.Range(0, clips.Length) : index;
        AudioClip clip = clips[clipIndex];

        if (clip == null)
            return;

        instance.audioSource.PlayOneShot(clip, volume);
    }

    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds
    {
        get => sounds;
    }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
