using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";


    public static SoundManager Instance { get; private set; }


    [SerializeField] private AudioClipRefsSO _AudioClipRefsSO;


    private float _Volume = 1f;



    private void Awake()
    {
        Instance = this;

        _Volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedUpSomething += Instance_OnPickedUpSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(_AudioClipRefsSO.Trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(_AudioClipRefsSO.ObjectDrop, baseCounter.transform.position);
    }

    private void Instance_OnPickedUpSomething(object sender, System.EventArgs e)
    {
        PlaySound(_AudioClipRefsSO.ObjectPickup, Player.Instance.transform.position);
            
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(_AudioClipRefsSO.Chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = sender as DeliveryCounter;
        PlaySound(_AudioClipRefsSO.DeliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = sender as DeliveryCounter;
        PlaySound(_AudioClipRefsSO.DeliverySuccess, deliveryCounter.transform.position);
    }

    public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume); 
    }

    public void PlayFootStepSound(Vector3 position, float volume = 1f)
    {
        PlaySound(_AudioClipRefsSO.Footstep, position, volume);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * _Volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(_AudioClipRefsSO.Warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(_AudioClipRefsSO.Warning, position);
    }

    public void ChangeVolume()
    {
        _Volume += 0.1f;

        if (_Volume >= 1.1f)
            _Volume = 0f;

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _Volume);
        PlayerPrefs.Save(); // Save manually in case Unity crashes before it saves them itself.
    }

    public float GetVolume()
    {
        return _Volume;
    }
}
