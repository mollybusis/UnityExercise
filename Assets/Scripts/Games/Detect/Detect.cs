using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Detect : GameBase
{
    const string INSTRUCTIONS_NO_RED = "Press <color=cyan>Spacebar</color> as soon as you see the square.";
    const string INSTRUCTIONS_RED = "Press <color=cyan>Spacebar</color> as soon as you see a white square. Ignore red squares.";
    const string FINISHED = "FINISHED!";
    const string RESPONSE_GUESS = "No Guessing!";
    const string RESPONSE_CORRECT = "Good!";
    const string RESPONSE_TIMEOUT = "Missed it!";
    const string RESPONSE_SLOW = "Too Slow!";
    const string RESPONSE_WRONG = "Wrong!";
    Color RESPONSE_COLOR_GOOD = Color.green;
    Color RESPONSE_COLOR_BAD = Color.red;
    Color STIMULUS_COLOR_NORMAL = Color.white;
    Color STIMULUS_COLOR_BAD = Color.red;

    /// <summary>
    /// A reference to the UI canvas so we can instantiate the feedback text.
    /// </summary>
    public GameObject uiCanvas;
    /// <summary>
    /// The object that is normally displayed briefly to the player.
    /// </summary>
    public GameObject normalStimulus;
    /// <summary>
    /// The object that is displayed briefly to the player and should be ignored.
    /// </summary>
    public GameObject badStimulus;
    /// <summary>
    /// A prefab for an animated text label that appears when a trial fails/succeeds.
    /// </summary>
    public GameObject feedbackTextPrefab;
    /// <summary>
    /// The instructions text label.
    /// </summary>
    public Text instructionsText;


    /// <summary>
    /// Called when the game session has started.
    /// </summary>
    public override GameBase StartSession(TextAsset sessionFile)
    {
        base.StartSession(sessionFile);

        normalStimulus.SetActive(false);
        badStimulus.SetActive(false);

        DetectData data = base.sessionData.gameData as DetectData;

        if (data.IncludeRed)
        {
            instructionsText.text = INSTRUCTIONS_RED;
        }
        else
        {
            instructionsText.text = INSTRUCTIONS_NO_RED;
        }

        StartCoroutine(RunTrials(SessionData));

        return this;
    }


    /// <summary>
    /// Iterates through all the trials, and calls the appropriate Start/End/Finished events.
    /// </summary>
    protected virtual IEnumerator RunTrials(SessionData data)
    {
        foreach (Trial t in data.trials)
        {
            StartTrial(t);
            yield return StartCoroutine(DisplayStimulus(t));
            EndTrial(t);
        }
        FinishedSession();
        yield break;
    }


    /// <summary>
    /// Displays the Stimulus for a specified duration.
    /// During that duration the player needs to respond as quickly as possible.
    /// </summary>
    protected virtual IEnumerator DisplayStimulus(Trial t)
    {
        DetectData data = base.sessionData.gameData as DetectData;
        DetectTrial trial = t as DetectTrial;
        GameObject stim = normalStimulus;

        if (data.IncludeRed && trial.IsRed)
        {
            stim = badStimulus;
        }

        RectTransform rt = stim.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(trial.PositionX, trial.positionY);

        stim.SetActive(false);

        yield return new WaitForSeconds(t.delay);

        StartInput();
        stim.SetActive(true);

        yield return new WaitForSeconds(((DetectTrial)t).duration);
        stim.SetActive(false);
        EndInput();

        yield break;
    }


    /// <summary>
    /// Called when the game session is finished.
    /// e.g. All session trials have been completed.
    /// </summary>
    protected override void FinishedSession()
    {
        base.FinishedSession();
        instructionsText.text = FINISHED;
    }


    /// <summary>
    /// Called when the player makes a response during a Trial.
    /// StartInput needs to be called for this to execute, or override the function.
    /// </summary>
    public override void PlayerResponded(KeyCode key, float time)
    {
        if (!listenForInput)
        {
            return;
        }
        base.PlayerResponded(key, time);
        if (key == KeyCode.Space)
        {
            EndInput();
            AddResult(CurrentTrial, time);
        }
    }


    /// <summary>
    /// Adds a result to the SessionData for the given trial.
    /// </summary>
    protected override void AddResult(Trial t, float time)
    {
        TrialResult r = new TrialResult(t);
        DetectTrial trial = t as DetectTrial;

        r.responseTime = time;
        if (time == 0)
        {
            // No response.
            if (trial.IsRed)
            {
                //Red stimulus, so no response is correct.
                DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
                r.success = true;
                r.accuracy = 1;
                GUILog.Log("Success, yay! No response!");
            }
            else
            {
                DisplayFeedback(RESPONSE_TIMEOUT, RESPONSE_COLOR_BAD);
                GUILog.Log("Failure! No response!");
            }
            
        }
        else
        {
            if (trial.IsRed)
            {
                //Red stimulus, so any kind of response is automatically a fail.
                DisplayFeedback(RESPONSE_WRONG, RESPONSE_COLOR_BAD);
                GUILog.Log("Failure! Shouldn't have responded! responseTime = {0}", time);
            }
            else
            {
                if (IsGuessResponse(time))
                {
                    // Responded before the guess limit, aka guessed.
                    DisplayFeedback(RESPONSE_GUESS, RESPONSE_COLOR_BAD);
                    GUILog.Log("Failure! Guess response! responseTime = {0}", time);
                }
                else if (IsValidResponse(time))
                {
                    // Responded correctly.
                    DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
                    r.success = true;
                    r.accuracy = GetAccuracy(t, time);
                    GUILog.Log("Success, yay! responseTime = {0}", time);
                }
                else
                {
                    // Responded too slow.
                    DisplayFeedback(RESPONSE_SLOW, RESPONSE_COLOR_BAD);
                    GUILog.Log("Failure! Slow response! responseTime = {0}", time);
                }
            }

            
        }
        sessionData.results.Add(r);
    }


    /// <summary>
    /// Display visual feedback on whether the trial has been responded to correctly or incorrectly.
    /// </summary>
    private void DisplayFeedback(string text, Color color)
    {
        GameObject g = Instantiate(feedbackTextPrefab);
        g.transform.SetParent(uiCanvas.transform);
        g.transform.localPosition = feedbackTextPrefab.transform.localPosition;
        Text t = g.GetComponent<Text>();
        t.text = text;
        t.color = color;
    }


    /// <summary>
    /// Returns the players response accuracy.
    /// The perfect accuracy would be 1, most inaccuracy is 0.
    /// </summary>
    protected float GetAccuracy(Trial t, float time)
    {
        DetectData data = sessionData.gameData as DetectData;
        bool hasResponseTimeLimit = data.ResponseTimeLimit > 0;

        float rTime = time - data.GuessTimeLimit;
        float totalTimeWindow = hasResponseTimeLimit ?
            data.ResponseTimeLimit : (t as DetectTrial).duration;

        return 1f - (rTime / (totalTimeWindow - data.GuessTimeLimit));
    }


    /// <summary>
    /// Returns True if the given response time is considered a guess.
    /// </summary>
    protected bool IsGuessResponse(float time)
    {
        DetectData data = sessionData.gameData as DetectData;
        return data.GuessTimeLimit > 0 && time < data.GuessTimeLimit;
    }


    /// <summary>
    /// Returns True if the given response time is considered valid.
    /// </summary>
    protected bool IsValidResponse(float time)
    {
        DetectData data = sessionData.gameData as DetectData;
        return data.ResponseTimeLimit <= 0 || time < data.ResponseTimeLimit;
    }




}
