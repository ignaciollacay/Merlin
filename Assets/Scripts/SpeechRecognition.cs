//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Threading.Tasks;
using System.Globalization;
using System;
using System.Diagnostics;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif
using Debug = UnityEngine.Debug;

/// <summary>
/// SpeechRecognition class lets the user use Speech-to-Text to convert spoken words
/// into text strings. Support for interim results (i.e. recognition hypotheses) 
/// that are returned in near real-time as the speaks in the microphone.
/// </summary>
public class SpeechRecognition : MonoBehaviour
{
    // Public fields in the Unity inspector
    [Tooltip("Unity UI Text component used to post recognizing results on screen.")]
    public Text RecognizingText;
    [Tooltip("Unity UI Text component used to post recognized results on screen.")]
    public Text RecognizedText;
    [Tooltip("Unity UI Text component used to report potential errors on screen.")]
    public Text ErrorText;
    [Tooltip("Unity UI Text component used to post recognized messages on screen.")]
    public Text DebugText;

    [Tooltip("Speech Assessment Manager")]
    public PhraseRecognition phraseRecognition;

    // Used to show live messages on screen, must be locked to avoid threading deadlocks since
    // the recognition events are raised in a separate thread
    private string recognizedString = "";
    private string recognizingString = "";
    private string debugString = "";
    private string errorString = "";
    private System.Object threadLocker = new System.Object();

    // Speech recognition key, required
    [Header("Speech Recognition Settings")]
    [Tooltip("Connection string to Cognitive Services Speech.")]
    public string SpeechServiceAPIKey = "65fcf04b1c36450aa9d9d8d62b5a6b00";
    [Tooltip("Region for your Cognitive Services Speech instance (must match the key).")]
    public string SpeechServiceRegion = "eastus";
    // The current language of origin is locked to English-US in this sample. Change this
    // to another region & language code to use a different origin language.
    // e.g. fr-fr, es-es, etc. Full list in https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support?tabs=stt-tts
    [Tooltip("Region & Language).")]
    public string fromLanguage = "es-AR";

    // Cognitive Services Speech objects used for Speech Recognition
    private SpeechRecognizer recognizer;
    private PhraseListGrammar phraseList;

    [Header("Phrase List Settings")]
    [Tooltip("Add all keywords in text to read to Phrase List")]
    [SerializeField] private bool automaticPhraseList = false;
    [Tooltip("Add custom keywords in text to read to Phrase List")]
    [SerializeField] private bool customPhraseList = false;
    public Toggle phraseListEnabled;
    [Tooltip("Add custom keywords to Phrase List")]
    [SerializeField] private string[] customKeywords;

    private bool micPermissionGranted = false;
#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    private void Awake()
    {
        // Uncomment and set the values to manually set Speech Service API Key and Region in code,
        //SpeechServiceAPIKey = "YourSubscriptionKey";
        //SpeechServiceRegion = "YourServiceRegion";
    }

    private void Start()
    {
#if PLATFORM_ANDROID
        // Request to use the microphone, cf.
        // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
        recognizedString = "Waiting for microphone permission...";
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#elif PLATFORM_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
#else
        micPermissionGranted = true;
#endif
    }

    /// <summary>
    /// Attach to button component used to launch continuous recognition (with or without translation)
    /// </summary>
    public void StartContinuous()
    {
        errorString = "";
        if (micPermissionGranted)
        {
            StartContinuousRecognition();
        }
        else
        {
            debugString = "This app cannot function without access to the microphone.";
            errorString = "ERROR: Microphone access denied.";
            Debug.LogError(errorString);
        }
    }

    /// <summary>
    /// Creates a class-level Speech Recognizer for a specific language using Azure credentials
    /// and hooks-up lifecycle & recognition events
    /// </summary>
    void CreateSpeechRecognizer()
    {
        if (SpeechServiceAPIKey.Length == 0 || SpeechServiceAPIKey == "YourSubscriptionKey")
        {
            debugString = "You forgot to obtain Cognitive Services Speech credentials and inserting them in this app." + Environment.NewLine +
                               "See the README file and/or the instructions in the Awake() function for more info before proceeding.";
            errorString = "ERROR: Missing service credentials";
            Debug.LogError(errorString);
            return;
        }
        Debug.Log("Creating Speech Recognizer.");
        debugString = "Initializing speech recognition, please wait...";

        if (recognizer == null)
        {
            SpeechConfig config = SpeechConfig.FromSubscription(SpeechServiceAPIKey, SpeechServiceRegion);
            config.SpeechRecognitionLanguage = fromLanguage;
            recognizer = new SpeechRecognizer(config);

            if (recognizer != null)
            {
                // Subscribes to speech events.
                recognizer.Recognizing += RecognizingHandler;
                recognizer.Recognizing += phraseRecognition.PronunciationAssessment;
                recognizer.Recognized += RecognizedHandler;
                recognizer.SpeechStartDetected += SpeechStartDetectedHandler;
                recognizer.SpeechEndDetected += SpeechEndDetectedHandler;
                recognizer.Canceled += CanceledHandler;
                recognizer.SessionStarted += SessionStartedHandler;
                recognizer.SessionStopped += SessionStoppedHandler;
            }
            if (phraseRecognition != null)
            {
                phraseRecognition.OnPhraseRecognized += StopRecognition;
            }
        }
        
        Debug.Log("CreateSpeechRecognizer exit");
    }
    void CreatePhraseList()
    {
        // Implement Phrase Lists
        phraseList = PhraseListGrammar.FromRecognizer(recognizer);

        if (phraseRecognition == null)
        {
            Debug.LogWarning("TextToRead reference missing. Make sure to link Phrase Recognition in the inspector");
        }

        if (automaticPhraseList)
        {
            foreach (string keyword in phraseRecognition.words)
            {
                phraseList.AddPhrase(keyword);
            }
        }
        if (customPhraseList)
        {
            foreach (string keyword in customKeywords)
            {
                phraseList.AddPhrase(keyword);
            }
        }
    }

    /// <summary>
    /// Initiate continuous speech recognition from the default microphone.
    /// </summary>
    private async void StartContinuousRecognition()
    {
        if (phraseListEnabled.isOn)
        {
            automaticPhraseList = true;
            Debug.Log("Phrase List toggle On");
        }
        else
            automaticPhraseList = false;
        
        Debug.Log("Starting Continuous Speech Recognition.");
        CreateSpeechRecognizer();

        if ((phraseRecognition != null) && ((customPhraseList) || (automaticPhraseList)))
        {
            CreatePhraseList();
            Debug.Log("Creating Phrase List");
        }
        else
        {
            Debug.Log("Phrase list could not be created. Check if TextToRead is assigned or Phrase List is Enabled");
        }


        if (recognizer != null)
        {
            Debug.Log("Starting Speech Recognizer.");
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            debugString = "Speech Recognizer is now running.";
            Debug.Log("Speech Recognizer is now running.");
        }
        Debug.Log("Start Continuous Speech Recognition exit");
    }
    
    // TODO: Link event for spellcheck
    //       Prevent text from resetting

    #region Speech Recognition event handlers
    private void SessionStartedHandler(object sender, SessionEventArgs e)
    {
        Debug.Log($"\n    Session started event. Event: {e.ToString()}.");
    }

    private void SessionStoppedHandler(object sender, SessionEventArgs e)
    {
        Debug.Log($"\n    Session event. Event: {e.ToString()}.");
        Debug.Log($"Session Stop detected. Stop the recognition.");
    }

    private void SpeechStartDetectedHandler(object sender, RecognitionEventArgs e)
    {
        Debug.Log($"SpeechStartDetected received: offset: {e.Offset}.");
    }

    private void SpeechEndDetectedHandler(object sender, RecognitionEventArgs e)
    {
        Debug.Log($"SpeechEndDetected received: offset: {e.Offset}.");
        Debug.Log($"Speech end detected.");
    }

    // "Recognizing" events are fired every time we receive interim results during recognition (i.e. hypotheses)
    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizingSpeech)
        {
            Debug.Log($"HYPOTHESIS: Text={e.Result.Text}");

            lock (threadLocker)
            {
                recognizingString = e.Result.Text;
            }
        }
    }

    // "Recognized" events are fired when the utterance end was detected by the server
    private void RecognizedHandler(object sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            lock (threadLocker)
            {
                recognizedString = e.Result.Text;
            }
        }
        else if (e.Result.Reason == ResultReason.NoMatch)
        {
            Debug.Log($"NOMATCH: Speech could not be recognized.");
        }
    }

    // "Canceled" events are fired if the server encounters some kind of error.
    // This is often caused by invalid subscription credentials.
    private void CanceledHandler(object sender, SpeechRecognitionCanceledEventArgs e)
    {
        Debug.Log($"CANCELED: Reason={e.Reason}");

        errorString = e.ToString();
        if (e.Reason == CancellationReason.Error)
        {
            Debug.LogError($"CANCELED: ErrorDetails={e.ErrorDetails}");
            Debug.LogError("CANCELED: Did you update the subscription info?");
        }
    }
    #endregion

    /// <summary>
    /// Main update loop: Runs every frame
    /// </summary>
    void Update()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
        }
#elif PLATFORM_IOS
        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            micPermissionGranted = true;
        }
#endif
        // Used to update results on screen during updates
        lock (threadLocker)
        {
            DebugText.text = debugString;
            ErrorText.text = errorString;
            RecognizedText.text = recognizedString;
            RecognizingText.text = recognizingString;
        }
    }

    void OnDisable()
    {
        StopRecognition();
    }

    /// <summary>
    /// Stops the recognition on the speech recognizer.
    /// Important: Unhook all events & clean-up resources.
    /// </summary>
    public async void StopRecognition()
    {
        if (recognizer != null)
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            recognizer.Recognizing -= RecognizingHandler;
            recognizer.Recognizing -= phraseRecognition.PronunciationAssessment;
            recognizer.Recognized -= RecognizedHandler;
            recognizer.SpeechStartDetected -= SpeechStartDetectedHandler;
            recognizer.SpeechEndDetected -= SpeechEndDetectedHandler;
            recognizer.Canceled -= CanceledHandler;
            recognizer.SessionStarted -= SessionStartedHandler;
            recognizer.SessionStopped -= SessionStoppedHandler;
            //phraseList.Clear();
            recognizer.Dispose();
            recognizer = null;
            debugString = "Speech Recognizer is now stopped.";
            Debug.Log("Speech Recognizer is now stopped.");
        }
        phraseRecognition.OnPhraseRecognized -= StopRecognition;
    }

    //Run from button when selecting which word to read?
    public void SetPhraseRecognizer(PhraseRecognition selectedPhrase)
    {
        phraseRecognition = selectedPhrase;
    }
}
