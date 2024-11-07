using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Detect : React
{
    const string INSTRUCTIONS_RED = "Press <color=cyan>Spacebar</color> as soon as you see a white square. Ignore red squares.";
    const string RESPONSE_WRONG = "Wrong!";
    Color STIMULUS_COLOR_NORMAL = Color.white;
    Color STIMULUS_COLOR_BAD = Color.red;

   
    /// <summary>
    /// The object that is normally displayed briefly to the player.
    /// </summary>
    public GameObject badStimulus;
    /// <summary>
    /// Boolean indicating whether to include red stimuli.
    /// </summary>
    private bool includeRed = false;


    /// <summary>
    /// Called when the game session has started.
    /// </summary>
    public override GameBase StartSession(TextAsset sessionFile)
    {

        base.StartSession(sessionFile);

        stimulus.SetActive(false);
        badStimulus.SetActive(false);

        if (includeRed)
        {
            instructionsText.text = INSTRUCTIONS_RED;
        }
        else
        {
            instructionsText.text = INSTRUCTIONS;
        }

        return this;
    }


    /// <summary>
    /// Displays the Stimulus for a specified duration.
    /// During that duration the player needs to respond as quickly as possible.
    /// </summary>
    protected override IEnumerator DisplayStimulus(Trial t)
    {
        DetectData data = base.sessionData.gameData as DetectData;
        DetectTrial trial = t as DetectTrial;
        GameObject stim;
        includeRed = data.includeRed;

        if (includeRed && trial.IsRed)
        {
            stim = badStimulus;
        }
        else
        {
            stim = stimulus;
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
    /// Adds a result to the SessionData for the given trial.
    /// </summary>
    protected override void AddResult(Trial t, float time)
    {
        TrialResult r = new TrialResult(t);
        DetectTrial trial = t as DetectTrial;
        r.responseTime = time;

        //Red (bad) stimulus
        if (includeRed && trial.IsRed)
        {
            if (time == 0)
            {
                // No response. Red stimulus, so no response is correct.
                DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
                r.success = true;
                r.accuracy = 1;
                GUILog.Log("Success, yay! No response!");

            }
            else
            {
                //Red stimulus, so any kind of response is automatically a fail.
                DisplayFeedback(RESPONSE_WRONG, RESPONSE_COLOR_BAD);
                GUILog.Log("Failure! Shouldn't have responded! responseTime = {0}", time);
            }

            sessionData.results.Add(r);
        }
        //Normal stimulus
        else
        {
            base.AddResult(t, time);
        }
 
    }


    




}
