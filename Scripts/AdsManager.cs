using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;

public class AdsManager : MonoBehaviour
{
    IInterstitialAd _ad;
    private string _adUnitId = "Interstitial_Android";
    private string _gameId = "5001675";
    private static int _adCount = 0;
    private List<AudioSource> _audioSources;
    private bool _adloaded;


    void Start()
    {
        InitServices();
        DontDestroyOnLoad(gameObject);
        _audioSources = new List<AudioSource>();
    }

    public void UpdateAdCount()
    {
        if (!_adloaded)
        {
            return;
        }

        _adCount++;

        if (_adCount == 3)
        {
            ShowAd();
            _adCount = 0;
            _audioSources.Add(GameObject.Find("GameMusic").GetComponent<AudioSource>());
            _audioSources.Add(GameObject.Find("UIMusic").GetComponent<AudioSource>());

            foreach (AudioSource a in _audioSources)
            {
                if (a.isPlaying)
                    a.volume = 1f;
            }

            Time.timeScale = 0f;
        }


    }

    public async Task InitServices()
    {
        try
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetGameId(_gameId);
            await UnityServices.InitializeAsync(initializationOptions);

            InitializationComplete();
        }
        catch (Exception e)
        {
            InitializationFailed(e);
        }
    }

    public void SetupAd()
    {
        //Create
        _ad = MediationService.Instance.CreateInterstitialAd(_adUnitId);

        //Subscribe to events
        _ad.OnClosed += AdClosed;
        _ad.OnClicked += AdClicked;
        _ad.OnLoaded += AdLoaded;
        _ad.OnFailedLoad += AdFailedLoad;

        // Impression Event
        MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
    }

    public void Dispose() => _ad?.Dispose();


    public async void ShowAd()
    {
        if (_ad.AdState == AdState.Loaded)
        {
            try
            {
                InterstitialAdShowOptions showOptions = new InterstitialAdShowOptions();
                showOptions.AutoReload = true;
                await _ad.ShowAsync(showOptions);
                AdShown();
            }
            catch (ShowFailedException e)
            {
                AdFailedShow(e);
            }
        }
    }

    void InitializationComplete()
    {
        SetupAd();
        LoadAd();
    }

    async Task LoadAd()
    {
        try
        {
            await _ad.LoadAsync();
        }
        catch (LoadFailedException)
        {
            // We will handle the failure in the OnFailedLoad callback
        }
    }

    void InitializationFailed(Exception e)
    {
        Debug.Log("Initialization Failed: " + e.Message);
    }

    void AdLoaded(object sender, EventArgs e)
    {
        Debug.Log("Ad loaded");
        _adloaded = true;
    }

    void AdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        Debug.Log("Failed to load ad");
        Debug.Log(e.Message);
    }

    void AdShown()
    {
        Debug.Log("Ad shown!");
    }

    void AdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad has closed");
        // Execute logic after an ad has been closed.
        Time.timeScale = 1f;

        foreach (AudioSource a in _audioSources)
        {
            if (a.isPlaying)
                a.volume = 1f;
        }
    }

    void AdClicked(object sender, EventArgs e)
    {
        Debug.Log("Ad has been clicked");
        // Execute logic after an ad has been clicked.
    }

    void AdFailedShow(ShowFailedException e)
    {
        Debug.Log(e.Message);
    }

    void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
        Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
    }
}
