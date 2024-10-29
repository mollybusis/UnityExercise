using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// Contains general Game data for the Detect gametype.
/// </summary>

public class DetectData : GameData
{
    const string ATTRIBUTE_GUESS_TIMELIMIT = "guessTimeLimit";
    const string ATTRIBUTE_RESPONSE_TIMELIMIT = "responseTimeLimit";
    public const string ATTRIBUTE_DURATION = "duration";

    const string ATTRIBUTE_RANDOM_POSITIONS = "randomPositions";
    const string ATTRIBUTE_INCLUDE_RED = "includeRed";
    const string ATTRIBUTE_POSITION_RANGE_MIN = "positionRangeMin";
    const string ATTRIBUTE_POSITION_RANGE_MAX = "positionRangeMax";

    /// <summary>
    /// The amount of time that needs to pass before the player can respond without being penalized.
    /// </summary>
    private float guessTimeLimit = 0;
    /// <summary>
    /// The amount of time that the user has to respond; 
    /// Starts when input becomes enabled during a Trial. 
    /// Responses that fall within this time constraint will be marked as Successful.
    /// </summary>
    private float responseTimeLimit = 0;
    /// <summary>
    /// The visibility Duration for the Stimulus.
    /// </summary>
    private float duration = 0;
    /// <summary>
    /// Indicates whether random Positions should be generated for Trials. If false, pre-determined Positions are used.
    /// </summary>
    private bool randomPositions = false;
    /// <summary>
    /// Indicates whether Stimuli should sometimes appear red and be ignored by the player.
    /// </summary>
    private bool includeRed = false;
    /// <summary>
    /// The minimum of the range for randomly-generated Positions.
    /// </summary>
    private float positionRangeMin = 0;
    /// <summary>
    /// The maximum of the range for randomly-generated Positions.
    /// </summary>
    private float positionRangeMax = 0;




    #region ACCESSORS

    public float GuessTimeLimit
    {
        get
        {
            return guessTimeLimit;
        }
    }
    public float ResponseTimeLimit
    {
        get
        {
            return responseTimeLimit;
        }
    }
    public float GeneratedDuration
    {
        get
        {
            return duration;
        }
    }

    public bool RandomPositions
    {
        get
        { 
                return randomPositions;
        }
    }

    public bool IncludeRed
    {
        get
        {
            return includeRed;
        }
    }

    public float PositionRangeMin
    {
        get
        {
            return positionRangeMin;
        }
    }

    public float PositionRangeMax
    {
        get
        {
            return positionRangeMax;
        }
    }

    #endregion


    public DetectData(XmlElement elem)
        : base(elem)
    {
    }


    public override void ParseElement(XmlElement elem)
    {
        base.ParseElement(elem);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_DURATION, ref duration);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_RESPONSE_TIMELIMIT, ref responseTimeLimit);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_GUESS_TIMELIMIT, ref guessTimeLimit);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_RANDOM_POSITIONS, ref randomPositions);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_INCLUDE_RED , ref includeRed);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_POSITION_RANGE_MIN, ref positionRangeMin);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_POSITION_RANGE_MAX, ref positionRangeMax);
    }


    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_GUESS_TIMELIMIT, guessTimeLimit.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_RESPONSE_TIMELIMIT, responseTimeLimit.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_DURATION, duration.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_RANDOM_POSITIONS, randomPositions.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_INCLUDE_RED, includeRed.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_RANGE_MIN, positionRangeMin.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_RANGE_MAX, positionRangeMax.ToString(), ref elem);
    }
}
