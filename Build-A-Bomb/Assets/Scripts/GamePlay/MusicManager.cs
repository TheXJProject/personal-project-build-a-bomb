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
    KEYPAD,
    GREMLIN,
    FUELING,
    ULTIMATE
}

public class MusicManager : MonoBehaviour
{
    public float taskFadeInTime = 2.0f;

    bool hammer_ = false;
    bool bolting_ = false;
    bool switch_ = false;
    bool valve_ = false;
    bool puzzle_ = false;
    bool keypad_ = false;
    bool gremlin_ = false;
    bool fueling_ = false;
    bool ultimate_ = false;

    bool gameplay1 = false;
    bool gameplay2 = false;
    bool gameplay3 = false;

    bool taskGoesWrong = false;

    private void OnEnable()
    {
        // temp.onTutorialStart += // TODO
        // temp.onBaseGameStart +=
        GameStartCount.onCountdownFinished += NewGame;
        BeginMainMenu.startMainMenuMusic += NewTrack;
        GameStartCount.startGamplay1Music += NewTrack;
        TaskStatus.onTaskSelected += AddNewTrackFromTask;
        LayerStatus.onLayerCompleted += AddNewTrackFromLayer;
        TaskStatus.onTaskGoneWrong += AddNewTrackFromTaskGoneWrong;
    }

    private void OnDisable()
    {
        // temp.onTutorialStart += // TODO
        // temp.onBaseGameStart +=
        GameStartCount.onCountdownFinished -= NewGame;
        BeginMainMenu.startMainMenuMusic -= NewTrack;
        GameStartCount.startGamplay1Music -= NewTrack;
        TaskStatus.onTaskSelected -= AddNewTrackFromTask;
        LayerStatus.onLayerCompleted -= AddNewTrackFromLayer;
        TaskStatus.onTaskGoneWrong -= AddNewTrackFromTaskGoneWrong;
    }

    void NewGame()
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

        gameplay1 = false;
        gameplay2 = false;
        gameplay3 = false;

        taskGoesWrong = false;
    }

    void NewTrack(MUSIC_TRACKS track, bool reset, double musicStartTime)
    {
        double startTime = AudioSettings.dspTime + musicStartTime;

        // If we want to start all prior tracks
        if (reset)
        {
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
            case MUSIC_TRACKS.TUTORIAL:
            case MUSIC_TRACKS.GAMEPLAY1: GameplayTracks1(startTime); break;
            case MUSIC_TRACKS.GAMEPLAY2: GameplayTracks2(startTime); break;
            case MUSIC_TRACKS.GAMEPLAY3: GameplayTracks3(startTime); break;
            case MUSIC_TRACKS.OPENING:
            case MUSIC_TRACKS.LOSING:
            case MUSIC_TRACKS.WINNING:
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
        }
    }

    void GameplayTracks2(double time)
    {
        if (!gameplay2)
        {
            gameplay2 = true;
            // Play all music tracks at the same time for start of game
            AudioManager.instance.PlayMusic("Main2 Pt5 Fade", time);
            AudioManager.instance.PlayMusic("Main2 Pt5 Puzzle", time);
            AudioManager.instance.PlayMusic("Main2 Pt5 Start", time);
            AudioManager.instance.PlayMusic("Main2 Pt6 Fade", time);
            AudioManager.instance.PlayMusic("Main2 Pt6(not 8) FX", time);
            AudioManager.instance.PlayMusic("Main2 Pt7 Keypad", time);
            AudioManager.instance.PlayMusic("Main2 Pt7(8) FX", time);
            AudioManager.instance.PlayMusic("Main2 Pt8 Gremlin", time);
        }
    }

    void GameplayTracks3(double time)
    {
        if (!gameplay3)
        {
            gameplay3 = true;
            // Play all music tracks at the same time for start of game
            AudioManager.instance.PlayMusic("Main3 Pt9 Fueling", time);
            AudioManager.instance.PlayMusic("Main3 Pt9 Start", time);
            AudioManager.instance.PlayMusic("Main3 Pt10 Fade", time);
            AudioManager.instance.PlayMusic("Main3 Pt10 Ultimate", time);
        }
    }

    void AddNewTrackFromTask(GameObject task)
    {
        TASK_TYPES_FOR_MUSIC thisTaks = task.GetComponent<TaskStatus>().thisTask;
        switch (thisTaks)
        {
            case TASK_TYPES_FOR_MUSIC.HAMMER: FadeIns(thisTaks, ref hammer_); break;
            case TASK_TYPES_FOR_MUSIC.BOLTING: FadeIns(thisTaks, ref bolting_); break;
            case TASK_TYPES_FOR_MUSIC.SWITCH: FadeIns(thisTaks, ref switch_); break;
            case TASK_TYPES_FOR_MUSIC.VALVE: FadeIns(thisTaks, ref valve_); break;
            case TASK_TYPES_FOR_MUSIC.PUZZLE: FadeIns(thisTaks, ref puzzle_); break;
            case TASK_TYPES_FOR_MUSIC.KEYPAD: FadeIns(thisTaks, ref keypad_); break;
            case TASK_TYPES_FOR_MUSIC.GREMLIN: FadeIns(thisTaks, ref gremlin_); break;
            case TASK_TYPES_FOR_MUSIC.FUELING: FadeIns(thisTaks, ref fueling_); break;
            case TASK_TYPES_FOR_MUSIC.ULTIMATE: FadeIns(thisTaks, ref ultimate_); break;
            default: break;
        }
    }

    void AddNewTrackFromLayer(GameObject layer
    {

    }

    void AddNewTrackFromTaskGoneWrong(GameObject task)
    {
        if (!taskGoesWrong)
        {
            taskGoesWrong = true;
            MixerFXManager.instance.SetMusicParam("Main1 Pt3 Task Goes Wrong", EX_PARA.VOLUME, taskFadeInTime);
        }
    }

    void FadeIns(TASK_TYPES_FOR_MUSIC task, ref bool hasBeenPlayed)
    {
        if (!hasBeenPlayed)
        {
            hasBeenPlayed = true;

            switch (task)
            {
                case TASK_TYPES_FOR_MUSIC.HAMMER: MixerFXManager.instance.SetMusicParam("Main1 Pt1 Hammer", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.BOLTING: MixerFXManager.instance.SetMusicParam("Main1 Pt2 Bolting", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.SWITCH: MixerFXManager.instance.SetMusicParam("Main1 Pt3 Switch", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.VALVE: MixerFXManager.instance.SetMusicParam("Main1 Pt4 Valve", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.PUZZLE: MixerFXManager.instance.SetMusicParam("Main1 Pt5 Puzzle", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.KEYPAD: MixerFXManager.instance.SetMusicParam("Main1 Pt6 Keypad", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.GREMLIN: MixerFXManager.instance.SetMusicParam("Main1 Pt7 Gremlin", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.FUELING: MixerFXManager.instance.SetMusicParam("Main1 Pt8 Fueling", EX_PARA.VOLUME, taskFadeInTime); break;
                case TASK_TYPES_FOR_MUSIC.ULTIMATE: MixerFXManager.instance.SetMusicParam("Main1 Pt9 Ultimate", EX_PARA.VOLUME, taskFadeInTime); break;
                default: break;
            }
        }
    }
}
