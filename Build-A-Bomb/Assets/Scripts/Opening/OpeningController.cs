using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpeningController : MonoBehaviour
{
    enum TransitionType
    {
        NONE,
        FADE,
        SHAKE
    };

    [Header("Initialise in inspector:")]
    [SerializeField] List<GameObject> slides;

    [Header("Edit values:")]
    [SerializeField] List<TransitionType> transitionIntoSlide;

    List<OpeningImageShake> allShakes = new List<OpeningImageShake>();
    List<OpeningImageFade> allFades = new List<OpeningImageFade>();
    List<OpeningImageSlideInController> allSlideInControllers = new List<OpeningImageSlideInController>();
    PlayerInputActions inputActions;
    int curSlide = -1;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        foreach (GameObject slide in slides)
        {
            allSlideInControllers.Add(slide.GetComponent<OpeningImageSlideInController>());
            allShakes.Add(slide.transform.GetChild(0).gameObject.GetComponent<OpeningImageShake>());    // child 0 is imageshake
            allFades.Add(slide.transform.GetChild(1).GetComponent<OpeningImageFade>());                 // child 1 is fade
        }
    }

    private void OnEnable()
    {
        inputActions.Mouse.LeftClick.Enable();
        inputActions.Mouse.LeftClick.performed += MouseClickedToSkip;

    }

    private void OnDisable()
    {
        inputActions.Mouse.LeftClick.Disable();
        inputActions.Mouse.LeftClick.performed -= MouseClickedToSkip;
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1); // Wait a second just make it look nicer
        TransitionToNextSlide();
    }

    private void MouseClickedToSkip(InputAction.CallbackContext context)
    {
        GoToNextStage();
    }

    private void GoToNextStage()
    {
        //Main menu if reached the last slide
        if (curSlide >= slides.Count - 1) 
        {
            GameManager.instance.MainMenuFromOpening();
            return;
        }

        if (curSlide < 0) return;

        // Skip fade if it is fading
        if (allFades[curSlide].fadeState != OpeningImageFade.FadeType.NONE) 
        {
            allFades[curSlide].SkipFade();

            // If fading to black, then skip the the next fade from black too
            if (allFades[curSlide].fadeState == OpeningImageFade.FadeType.TOBLACK) 
            {
                allFades[curSlide + 1].fadeFromBlackOnEnable = false;
            }
            return;
        }

        TransitionToNextSlide();
    }

    void TransitionToNextSlide()
    {
        TransitionType transitionType = TransitionType.NONE;
        if (curSlide < transitionIntoSlide.Count)
        {
            transitionType = transitionIntoSlide[curSlide+1];
        }

        if(transitionType == TransitionType.SHAKE) allShakes[curSlide + 1].shakeOnEnable = true;
        if(transitionType == TransitionType.FADE) 
        { 
            if (curSlide > -1)
                allFades[curSlide].BeginImageFade(OpeningImageFade.FadeType.TOBLACK);
            allFades[curSlide+1].fadeFromBlackOnEnable = true;
        }
        StartCoroutine(WaitForFadeThenNextSlide());
    }

    IEnumerator WaitForFadeThenNextSlide() // This could be improved, but since the only thing you wait for rn is fade, it is what it is
    {
        if (curSlide > -1)
        {
            // Wait for fade to finish
            while (allFades[curSlide].fadeState != OpeningImageFade.FadeType.NONE) 
            {
                yield return null;
            }
        }
        NextSlide();
    }

    void NextSlide()
    {
        slides[++curSlide].SetActive(true);
        allSlideInControllers[curSlide].BeginSlidingImages();
    }
}
