using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
[RequireComponent(typeof(PostProcessVolume))]
public class DynamicPostProcessingHandler : MonoBehaviour
{
    [SerializeField] float maxVignetteStrength = 0.6f;
    private PostProcessVolume _postProcessVolume;
    private Vignette _vignette;
    // Start is called before the first frame update
    void Start()
    {

        if(GetComponent<PostProcessVolume>() != null)
        {
            _postProcessVolume = GetComponent<PostProcessVolume>();
            if(_postProcessVolume != null) 
            {
                _postProcessVolume.profile.TryGetSettings(out _vignette);
            }
        }
    }
    private void Update()
    {
        UpdateVignette();
        UpdateEnablePostProcessing();
    }
    void UpdateVignette()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<Health>() != null)
        {
            _vignette.intensity.value = ((float)GameManager.Instance.currentPlayer.GetComponentInChildren<Health>().CurrentHealth).Remap(0, GameManager.Instance.currentPlayer.GetComponentInChildren<Health>().currentMaxHealth, maxVignetteStrength, 0);
        }
    }
    void UpdateEnablePostProcessing()
    {
        if(GameManager.Instance != null && GameManager.Instance.currentPlayer == null)
        {
            _postProcessVolume.enabled = false;
        }
        else
        {
            _postProcessVolume.enabled = true;
        }
    }
}
