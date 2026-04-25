using System;
using UnityEngine;

public enum MUSIC_TRACKS
{
    MAIN_MENU,
    TUTORIAL,
    GAMEPLAY1,
    GAMEPLAY2,
    GAMEPLAY3,
    OPENING,
    LOSING,
    WINNING
}

public enum TASK_TYPES_FOR_MUSIC
{
    NULL,
    HAMMER,
    BOLTING,
    SWITCH,
    VALVE,
    PUZZLE,
    ISOTOPE,
    KEYPAD,
    GREMLIN,
    FUELING,
    ULTIMATE
}

public enum LAYER_NUMS_FOR_MUSIC
{
    NULL,
    TUTORIAL,
    L2,
    L3,
    L4,
    L5,
    L6,
    L7,
    L8,
    L9,
    L10
}

public class MusicManager : MonoBehaviour
{
    const float taskFadeInTime = 7.0f;
    const float layerFadeInTime = 10.0f;
    const float tutorialMusicWaitTime = 0.9f;
    const float tutorialMusicFadeInTime = 2.5f;
    const int numberTransitionOptions1 = 48;
    const int numberTransitionOptions2 = 48;
    const float fade1LastVolume = 0.06f;
    const float fade2LastVolume = 0.1f;
    double track1AveSamples = 0;
    double track2AveSamples = 0;
    double transitiontime1Samples = 0;
    double transitiontime2Samples = 0;

    bool hammer_ = false;
    bool bolting_ = false;
    bool switch_ = false;
    bool valve_ = false;
    bool puzzle_ = false;
    bool isotope_ = false;
    bool keypad_ = false;
    bool gremlin_ = false;
    bool fueling_ = false;
    bool ultimate_ = false;

    bool layer2_ = false;
    bool layer3_ = false;
    bool layer4_ = false;
    bool layer5_ = false;
    bool layer6_ = false;
    bool layer7_ = false;
    bool layer8_ = false;
    bool layer9_ = false;
    bool layer10_ = false;

    bool gameplay1 = false;
    bool gameplay2 = false;
    bool gameplay3 = false;

    bool taskGoesWrong = false;

    private void OnEnable()
    {
        // temp.onBaseGameStart +=
        TutorialControl.onTutorialStart += StartTutorialMusic;
        GameStartCount.onCountdownFinished += NewGame;
        BeginMainMenu.startMainMenuMusic += NewTrack;
        GameStartCount.startGamplay1Music += NewTrack;
        TaskStatus.onTaskSelected += AddNewTrackFromTask;
        LayerStatus.onLayerCompleted += AddNewTrackFromLayer;
        TaskStatus.onTaskGoneWrong += AddNewTrackFromTaskGoneWrong;
    }

    private void OnDisable()
    {
        // temp.onBaseGameStart +=
        TutorialControl.onTutorialStart -= StartTutorialMusic;
        GameStartCount.onCountdownFinished -= NewGame;
        BeginMainMenu.startMainMenuMusic -= NewTrack;
        GameStartCount.startGamplay1Music -= NewTrack;
        TaskStatus.onTaskSelected -= AddNewTrackFromTask;
        LayerStatus.onLayerCompleted -= AddNewTrackFromLayer;
        TaskStatus.onTaskGoneWrong -= AddNewTrackFromTaskGoneWrong;
    }

    private void Awake()
    {
        // Get the average number of samples in transition 1
        Sound sound = Array.Find(AudioManager.instance.musicSounds, x => x.name == "Main1 Pt1 Start");
        track1AveSamples = sound.clip.samples / (double)numberTransitionOptions1;
        transitiontime1Samples = 4f * track1AveSamples;

        // Get the average number of samples in transition 2
        sound = Array.Find(AudioManager.instance.musicSounds, x => x.name == "Main2 Pt5 Start");
        track2AveSamples = sound.clip.samples / (double)numberTransitionOptions2;
        transitiontime2Samples = 6f * track2AveSamples;
    }

    void NewGame()
    {
        ResetBools();

        // Fade in the start track
        MixerFXManager.instance.SetMusicParam("Main1 Pt1 Start", EX_PARA.VOLUME, 0.01f);
    }

    void ResetBools()
    {
        hammer_ = false;
        bolting_ = false;
        switch_ = false;
        valve_ = false;
        puzzle_ = false;
        keypad_ = false;
        gremlin_ = false;
        fueling_ = false;
        ultimate_ = false;

        layer2_ = false;
        layer3_ = false;
        layer4_ = false;
        layer5_ = false;
        layer6_ = false;
        layer7_ = false;
        layer8_ = false;
        layer9_ = false;
        layer10_ = false;

        taskGoesWrong = false;
    }

    void NewTrack(MUSIC_TRACKS track, bool reset, double musicStartTime)
    {
        double startTime = AudioSettings.dspTime + musicStartTime;

        // If we want to start all prior tracks
        if (reset)
        {
            gameplay1 = false;
            gameplay2 = false;
            gameplay3 = false;

            // Stop all current music tracks and sfx at the same time
            AudioManager.instance.StopAllSFX();
            AudioManager.instance.StopAllMusic();

            // Set all individual music groups to zero volume
            MixerFXManager.instance.ForceSetParam(GROUP_OPTIONS.MUSIC_COLLECTION, EX_PARA.VOLUME, 0f);

            // Set overall music collection to normal
            MixerFXManager.instance.SetMusicOverallParam(EX_PARA.VOLUME, 0);
            MixerFXManager.instance.SetSfxOverallParam(EX_PARA.VOLUME, 0);
        }

        switch (track)
        {
            case MUSIC_TRACKS.MAIN_MENU: MainMenuTracks(startTime); break;
            case MUSIC_TRACKS.TUTORIAL: TutorialTrack(startTime); break;
            case MUSIC_TRACKS.OPENING:
            case MUSIC_TRACKS.LOSING:
            case MUSIC_TRACKS.WINNING:
            case MUSIC_TRACKS.GAMEPLAY1: GameplayTracks1(startTime); break;
            case MUSIC_TRACKS.GAMEPLAY2: GameplayTracks2(startTime); break;
            case MUSIC_TRACKS.GAMEPLAY3: GameplayTracks3(startTime); break;
            default:break;
        }
    }

    void MainMenuTracks(double time)
    {
        // Play all main menu music tracks at the same time
        AudioManager.instance.PlayMusic("Menu Alarms", time);
        AudioManager.instance.PlayMusic("Menu Bass", time);
        AudioManager.instance.PlayMusic("Menu Beeps", time);
        AudioManager.instance.PlayMusic("Menu Choir", time);
        AudioManager.instance.PlayMusic("Menu FullChoirCrash", time);
        AudioManager.instance.PlayMusic("Menu Hats", time);
        AudioManager.instance.PlayMusic("Menu KickSnare", time);
        AudioManager.instance.PlayMusic("Menu OfficeNoise", time);
        AudioManager.instance.PlayMusic("Menu Organ", time);
        AudioManager.instance.PlayMusic("Menu StartMelody", time);
        AudioManager.instance.PlayMusic("Menu StringsXyphone", time);
    }

    void TutorialTrack(double time)
    {
        // Play all main menu music tracks at the same time
        AudioManager.instance.PlayMusic("Tutorial", time);
    }

    void GameplayTracks1(double time)
    {
        if (!gameplay1)
        {
            gameplay1 = true;
            // Play all music tracks at the same time for start of game
            AudioManager.instance.PlayMusic("Main1 Pt1 FX", time);
            AudioManager.instance.PlayMusic("Main1 Pt1 Hammer", time);
            AudioManager.instance.PlayMusic("Main1 Pt1 Start", time);
            AudioManager.instance.PlayMusic("Main1 Pt2 Bolting", time);
            AudioManager.instance.PlayMusic("Main1 Pt2 FX", time);
            AudioManager.instance.PlayMusic("Main1 Pt2 Switch", time);
            AudioManager.instance.PlayMusic("Main1 Pt3 Fade", time);
            AudioManager.instance.PlayMusic("Main1 Pt3 Task Goes Wrong", time);
            AudioManager.instance.PlayMusic("Main1 Pt3(4) FX", time);
            AudioManager.instance.PlayMusic("Main1 Pt4 Fade", time);
            AudioManager.instance.PlayMusic("Main1 Pt4 Valve", time);

            // Start everything muted
            MixerFXManager.instance.SetMusicParam("Main1 Pt1 Hammer", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt1 Start", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt2 Bolting", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt2 FX", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt2 Switch", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt3 Fade", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt3 Task Goes Wrong", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt3(4) FX", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt4 Fade", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main1 Pt4 Valve", EX_PARA.VOLUME, 0f, 0f);

            // Have the FX playing automatically
            MixerFXManager.instance.SetMusicParam("Main1 Pt1 FX", EX_PARA.VOLUME, 0.01f);
        }
    }

    void GameplayTracks2(double time)
    {
        if (!gameplay2)
        {
            gameplay2 = true;

            // Prevent error messages
            hammer_ = true;
            bolting_ = true;
            switch_ = true;
            valve_ = true;
            taskGoesWrong = true;

            // Play all music tracks at the same time for start of game
            AudioManager.instance.PlayMusic("Main2 Pt5 Fade", time);
            AudioManager.instance.PlayMusic("Main2 Pt5 Puzzle", time);
            AudioManager.instance.PlayMusic("Main2 Pt5 Start", time);
            AudioManager.instance.PlayMusic("Main2 Pt6 Fade", time);
            AudioManager.instance.PlayMusic("Main2 Pt6(not 8) Reactor", time);
            AudioManager.instance.PlayMusic("Main2 Pt7 Keypad", time);
            AudioManager.instance.PlayMusic("Main2 Pt7(8) FX", time);
            AudioManager.instance.PlayMusic("Main2 Pt8 Gremlin", time);

            // Start everything muted
            MixerFXManager.instance.SetMusicParam("Main2 Pt5 Puzzle", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main2 Pt6 Fade", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main2 Pt6(not 8) Reactor", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main2 Pt7 Keypad", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main2 Pt7(8) FX", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main2 Pt8 Gremlin", EX_PARA.VOLUME, 0f, 0f);

            // Have the start playing automatically and fade additional track slowly
            MixerFXManager.instance.SetMusicParam("Main2 Pt5 Start", EX_PARA.VOLUME, 0.01f);
            MixerFXManager.instance.SetMusicParam("Main2 Pt5 Fade", EX_PARA.VOLUME, (layerFadeInTime * 1.5f) + (float)time - (float)AudioSettings.dspTime);
        }
    }

    void GameplayTracks3(double time)
    {
        if (!gameplay3)
        {
            gameplay3 = true;

            // Prevent error messages
            hammer_ = true;
            bolting_ = true;
            switch_ = true;
            valve_ = true;
            puzzle_ = true;
            isotope_ = true;
            keypad_ = true;
            gremlin_ = true;
            taskGoesWrong = true;

            // Play all music tracks at the same time for start of game
            AudioManager.instance.PlayMusic("Main3 Pt9 Fueling", time);
            AudioManager.instance.PlayMusic("Main3 Pt9 Start", time);
            AudioManager.instance.PlayMusic("Main3 Pt10 Fade", time);
            AudioManager.instance.PlayMusic("Main3 Pt10 Ultimate", time);

            // Start everything muted
            MixerFXManager.instance.SetMusicParam("Main3 Pt9 Fueling", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main3 Pt10 Fade", EX_PARA.VOLUME, 0f, 0f);
            MixerFXManager.instance.SetMusicParam("Main3 Pt10 Ultimate", EX_PARA.VOLUME, 0f, 0f);

            // Have the start playing automatically
            MixerFXManager.instance.SetMusicParam("Main3 Pt9 Start", EX_PARA.VOLUME, 0.01f);
        }
    }

    void AddNewTrackFromTask(GameObject task)
    {
        TASK_TYPES_FOR_MUSIC thisTaks = task.GetComponent<TaskStatus>().thisTask;
        switch (thisTaks)
        {
            case TASK_TYPES_FOR_MUSIC.HAMMER: FadeInTaskMusic(thisTaks, ref hammer_); break;
            case TASK_TYPES_FOR_MUSIC.BOLTING: FadeInTaskMusic(thisTaks, ref bolting_); break;
            case TASK_TYPES_FOR_MUSIC.SWITCH: FadeInTaskMusic(thisTaks, ref switch_); break;
            case TASK_TYPES_FOR_MUSIC.VALVE: FadeInTaskMusic(thisTaks, ref valve_); break;
            case TASK_TYPES_FOR_MUSIC.PUZZLE: FadeInTaskMusic(thisTaks, ref puzzle_); break;
            case TASK_TYPES_FOR_MUSIC.ISOTOPE: FadeInTaskMusic(thisTaks, ref isotope_); break;
            case TASK_TYPES_FOR_MUSIC.KEYPAD: FadeInTaskMusic(thisTaks, ref keypad_); break;
            case TASK_TYPES_FOR_MUSIC.GREMLIN: FadeInTaskMusic(thisTaks, ref gremlin_); break;
            case TASK_TYPES_FOR_MUSIC.FUELING: FadeInTaskMusic(thisTaks, ref fueling_); break;
            case TASK_TYPES_FOR_MUSIC.ULTIMATE: FadeInTaskMusic(thisTaks, ref ultimate_); break;
            default:
                if (gameplay1 || gameplay2 || gameplay3)
                {
                    Debug.LogWarning("Error, Invalid task type!");
                }
                break;
        }
    }

    void AddNewTrackFromLayer(GameObject layer)
    {
        LAYER_NUMS_FOR_MUSIC thisLayer = layer.GetComponent<LayerStatus>().nextLayer;
        switch (thisLayer)
        {
            case LAYER_NUMS_FOR_MUSIC.L2: FadeInLayerMusic(thisLayer, ref layer2_); break;
            case LAYER_NUMS_FOR_MUSIC.L3: FadeInLayerMusic(thisLayer, ref layer3_); break;
            case LAYER_NUMS_FOR_MUSIC.L4: FadeInLayerMusic(thisLayer, ref layer4_); break;
            case LAYER_NUMS_FOR_MUSIC.L5: FadeInLayerMusic(thisLayer, ref layer5_); break;
            case LAYER_NUMS_FOR_MUSIC.L6: FadeInLayerMusic(thisLayer, ref layer6_); break;
            case LAYER_NUMS_FOR_MUSIC.L7: FadeInLayerMusic(thisLayer, ref layer7_); break;
            case LAYER_NUMS_FOR_MUSIC.L8: FadeInLayerMusic(thisLayer, ref layer8_); break;
            case LAYER_NUMS_FOR_MUSIC.L9: FadeInLayerMusic(thisLayer, ref layer9_); break;
            case LAYER_NUMS_FOR_MUSIC.L10: FadeInLayerMusic(thisLayer, ref layer10_); break;
            default:
                if (gameplay1 || gameplay2 || gameplay3)
                {
                    Debug.LogWarning("Error, Invalid layer type!");
                }
                break;
        }
    }

    void AddNewTrackFromTaskGoneWrong(GameObject task)
    {
        if (!taskGoesWrong && (gameplay1 || gameplay2 || gameplay3))
        {
            taskGoesWrong = true;
            MixerFXManager.instance.SetMusicParam("Main1 Pt3 Task Goes Wrong", EX_PARA.VOLUME, taskFadeInTime);
        }
    }

    void FadeInTaskMusic(TASK_TYPES_FOR_MUSIC task, ref bool hasBeenPlayed)
    {
        if (!hasBeenPlayed)
        {
            hasBeenPlayed = true;

            switch (task)
            {
                case TASK_TYPES_FOR_MUSIC.HAMMER: MixerFXManager.instance.SetMusicParam("Main1 Pt1 Hammer", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.BOLTING: MixerFXManager.instance.SetMusicParam("Main1 Pt2 Bolting", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.SWITCH: MixerFXManager.instance.SetMusicParam("Main1 Pt2 Switch", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.VALVE: MixerFXManager.instance.SetMusicParam("Main1 Pt4 Valve", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.PUZZLE: MixerFXManager.instance.SetMusicParam("Main2 Pt5 Puzzle", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.ISOTOPE: MixerFXManager.instance.SetMusicParam("Main2 Pt6(not 8) Reactor", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.KEYPAD: MixerFXManager.instance.SetMusicParam("Main2 Pt7 Keypad", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.GREMLIN: MixerFXManager.instance.SetMusicParam("Main2 Pt8 Gremlin", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.FUELING: MixerFXManager.instance.SetMusicParam("Main3 Pt9 Fueling", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.ULTIMATE: MixerFXManager.instance.SetMusicParam("Main3 Pt10 Ultimate", EX_PARA.VOLUME, taskFadeInTime); break;
                default: break;
            }
        }
    }

    void FadeInLayerMusic(LAYER_NUMS_FOR_MUSIC layer, ref bool hasBeenPlayed)
    {
        if (!hasBeenPlayed)
        {
            hasBeenPlayed = true;

            switch (layer)
            {
                case LAYER_NUMS_FOR_MUSIC.L2: Layer2(); break;
                case LAYER_NUMS_FOR_MUSIC.L3: Layer3(); break;
                case LAYER_NUMS_FOR_MUSIC.L4: Layer4(); break;
                case LAYER_NUMS_FOR_MUSIC.L5: Layer5(); break;
                case LAYER_NUMS_FOR_MUSIC.L6: Layer6(); break;
                case LAYER_NUMS_FOR_MUSIC.L7: Layer7(); break;
                case LAYER_NUMS_FOR_MUSIC.L8: Layer8(); break;
                case LAYER_NUMS_FOR_MUSIC.L9: Layer9(); break;
                case LAYER_NUMS_FOR_MUSIC.L10: Layer10(); break;
                default: break;
            }
        }
    }

    void StartTutorialMusic()
    {
        layer2_ = true;
        NewTrack(MUSIC_TRACKS.TUTORIAL, true, tutorialMusicWaitTime);
        MixerFXManager.instance.SetMusicParam("Tutorial", EX_PARA.VOLUME, tutorialMusicFadeInTime);
    }

    void Layer2()
    {
        // Fade out
        MixerFXManager.instance.SetMusicParam("Main1 Pt1 FX", EX_PARA.VOLUME, layerFadeInTime, 0f);

        // Fade in
        MixerFXManager.instance.SetMusicParam("Main1 Pt2 FX", EX_PARA.VOLUME, layerFadeInTime);
    }

    void Layer3()
    {
        // Fade out
        MixerFXManager.instance.SetMusicParam("Main1 Pt2 FX", EX_PARA.VOLUME, layerFadeInTime, 0f);

        // Fade in
        MixerFXManager.instance.SetMusicParam("Main1 Pt3 Fade", EX_PARA.VOLUME, layerFadeInTime);
        MixerFXManager.instance.SetMusicParam("Main1 Pt3(4) FX", EX_PARA.VOLUME, layerFadeInTime);
    }

    void Layer4()
    {
        // Fade out
        MixerFXManager.instance.SetMusicParam("Main1 Pt3(4) FX", EX_PARA.VOLUME, layerFadeInTime, 0.5f);

        // Fade in
        MixerFXManager.instance.SetMusicParam("Main1 Pt4 Fade", EX_PARA.VOLUME, layerFadeInTime);
    }

    void Layer5()
    {
        // Find a music source that's currently playing the start track
        AudioSource source = Array.Find(AudioManager.instance.musicSourceList, x => x.soundName == "Main1 Pt1 Start").audioSource;

        double samples = (double)source.timeSamples;
        double timeRemaining = (track1AveSamples - (samples % track1AveSamples)) / (double)source.clip.frequency;

        // Prevent missing the slot
        double transitionTimeTrack1 = (track1AveSamples / (double)source.clip.frequency);
        if (timeRemaining < 0.08f) timeRemaining += transitionTimeTrack1;

        // Calculate when to transition
        double transitionTime = (transitiontime1Samples / (double)source.clip.frequency);
        double swapTimeDSP = AudioSettings.dspTime + timeRemaining + transitionTime;

        // Start and stop required tracks at time
        FadeStop1(timeRemaining + transitionTime, swapTimeDSP);
        NewTrack(MUSIC_TRACKS.GAMEPLAY2, false, timeRemaining + transitionTime);

        // Transition track
        double startTransitionDSP = AudioSettings.dspTime + timeRemaining;
        AudioManager.instance.PlayMusic("Main Transition 1", startTransitionDSP);
        MixerFXManager.instance.SetMusicParam("Main Transition 1", EX_PARA.VOLUME, (float)transitionTimeTrack1 * 1.5f);
        AudioManager.instance.StopMusic("Main Transition 1", startTransitionDSP + transitionTime + (layerFadeInTime / 2));
    }

    void Layer6()
    {
        // Fade in
        MixerFXManager.instance.SetMusicParam("Main2 Pt6 Fade", EX_PARA.VOLUME, layerFadeInTime);
    }

    void Layer7()
    {
        // Fade in
        MixerFXManager.instance.SetMusicParam("Main2 Pt7(8) FX", EX_PARA.VOLUME, layerFadeInTime);
    }

    void Layer8()
    {
        // Fade out
        MixerFXManager.instance.SetMusicParam("Main2 Pt6(not 8) Reactor", EX_PARA.VOLUME, layerFadeInTime, 0f);
    }

    void Layer9()
    {
        // Find a music source that's currently playing the start track
        AudioSource source = Array.Find(AudioManager.instance.musicSourceList, x => x.soundName == "Main2 Pt5 Start").audioSource;

        double samples = (double)source.timeSamples;
        double timeRemaining = (track2AveSamples - (samples % track2AveSamples)) / (double)source.clip.frequency;

        // Prevent missing the slot
        double transitionTimeTrack2 = (track2AveSamples / (double)source.clip.frequency);
        if (timeRemaining < 0.08f) timeRemaining += transitionTimeTrack2;

        // Calculate when to transition
        double transitionTime = (transitiontime2Samples / (double)source.clip.frequency);
        double swapTimeDSP = AudioSettings.dspTime + timeRemaining + transitionTime;

        // Start and stop required tracks at time
        FadeStop2(timeRemaining + transitionTime, swapTimeDSP);
        NewTrack(MUSIC_TRACKS.GAMEPLAY3, false, timeRemaining + transitionTime);

        // Transition track
        double startTransitionDSP = AudioSettings.dspTime + timeRemaining;
        AudioManager.instance.PlayMusic("Main Transition 2", startTransitionDSP);
        MixerFXManager.instance.SetMusicParam("Main Transition 2", EX_PARA.VOLUME, (float)transitionTimeTrack2 * 1.5f);
        AudioManager.instance.StopMusic("Main Transition 2", startTransitionDSP + transitionTime + (layerFadeInTime / 2));
    }

    void Layer10()
    {
        // Fade in
        MixerFXManager.instance.SetMusicParam("Main3 Pt10 Fade", EX_PARA.VOLUME, layerFadeInTime);
    }

    void FadeStop1(double transition, double swapTime)
    {
        MixerFXManager.instance.SetMusicParam("Main1 Pt1 FX", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt1 Hammer", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt1 Start", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt2 Bolting", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt2 FX", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt2 Switch", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt3 Fade", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt3 Task Goes Wrong", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt3(4) FX", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt4 Fade", EX_PARA.VOLUME, (float)transition, fade1LastVolume);
        MixerFXManager.instance.SetMusicParam("Main1 Pt4 Valve", EX_PARA.VOLUME, (float)transition, fade1LastVolume);

        AudioManager.instance.StopMusic("Main1 Pt1 FX", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt1 Hammer", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt1 Start", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt2 Bolting", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt2 FX", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt2 Switch", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt3 Fade", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt3 Task Goes Wrong", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt3(4) FX", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt4 Fade", swapTime);
        AudioManager.instance.StopMusic("Main1 Pt4 Valve", swapTime);
    }

    void FadeStop2(double transition, double swapTime)
    {
        MixerFXManager.instance.SetMusicParam("Main2 Pt5 Fade", EX_PARA.VOLUME, (float)transition, fade2LastVolume);
        MixerFXManager.instance.SetMusicParam("Main2 Pt5 Puzzle", EX_PARA.VOLUME, (float)transition, fade2LastVolume);
        MixerFXManager.instance.SetMusicParam("Main2 Pt5 Start", EX_PARA.VOLUME, (float)transition, fade2LastVolume);
        MixerFXManager.instance.SetMusicParam("Main2 Pt6 Fade", EX_PARA.VOLUME, (float)transition, fade2LastVolume);
        MixerFXManager.instance.SetMusicParam("Main2 Pt7 Keypad", EX_PARA.VOLUME, (float)transition, fade2LastVolume);
        MixerFXManager.instance.SetMusicParam("Main2 Pt7(8) FX", EX_PARA.VOLUME, (float)transition, fade2LastVolume);
        MixerFXManager.instance.SetMusicParam("Main2 Pt8 Gremlin", EX_PARA.VOLUME, (float)transition, fade2LastVolume);

        AudioManager.instance.StopMusic("Main2 Pt5 Fade", swapTime);
        AudioManager.instance.StopMusic("Main2 Pt5 Puzzle", swapTime);
        AudioManager.instance.StopMusic("Main2 Pt5 Start", swapTime);
        AudioManager.instance.StopMusic("Main2 Pt6 Fade", swapTime);
        AudioManager.instance.StopMusic("Main2 Pt6(not 8) Reactor", swapTime);
        AudioManager.instance.StopMusic("Main2 Pt7 Keypad", swapTime);
        AudioManager.instance.StopMusic("Main2 Pt7(8) FX", swapTime);
        AudioManager.instance.StopMusic("Main2 Pt8 Gremlin", swapTime);
    }
}
