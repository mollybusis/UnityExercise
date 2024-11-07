using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Data;

public class DetectTrial : ReactTrial
{

    #region ATTRIBUTES

    public const string ATTRIBUTE_POSITION_X = "positionX";
    public const string ATTRIBUTE_POSITION_Y = "positionY";
    public const string ATTRIBUTE_IS_RED = "isRed";

    #endregion

    /// <summary>
    /// The pre-determined X position for the stimulus in this trial.
    /// </summary>
    public float positionX = 0;
    /// <summary>
    /// The pre-determined Y position for the stimulus in this trial.
    /// </summary>
    public float positionY = 0;
    /// <summary>
    /// Indicates whether this trial should be red; ignored if includeRed is false.
    /// </summary>
    private bool isRed = false;


    #region ACCESSORS

    public float PositionX
    {
        get
        {
            return positionX;
        }
    }

    public float PositionY
    {
        get
        {
            return positionY;
        }
    }

    public bool IsRed
    {
        get
        {
            return isRed;
        }
    }

    #endregion


    public DetectTrial(SessionData data, XmlElement n = null)
        : base(data, n)
    {
    }


    /// <summary>
    /// Parses Game specific variables for this Trial from the given XmlElement.
    /// If no parsable attributes are found, or fail, then it will generate some from the given GameData.
    /// Used when parsing a Trial that IS defined in the Session file.
    /// </summary>
    public override void ParseGameSpecificVars(XmlNode n, SessionData session)
    {
        base.ParseGameSpecificVars(n, session);

        XMLUtil.ParseAttribute(n, ATTRIBUTE_POSITION_X, ref positionX, true);
        XMLUtil.ParseAttribute(n, ATTRIBUTE_POSITION_Y, ref positionY, true);

        DetectData data = (DetectData)(session.gameData);

        if (data.RandomPositions)
        {
            positionX = Random.Range(data.PositionRangeMinX, data.PositionRangeMaxX);
            positionY = Random.Range(data.PositionRangeMinY, data.PositionRangeMaxY);
        }

     

        XMLUtil.ParseAttribute(n, ATTRIBUTE_IS_RED, ref isRed, true);
    }


    /// <summary>
    /// Writes any tracked variables to the given XElement.
    /// </summary>
    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);
        
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_X, positionX.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_Y, positionY.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_IS_RED, isRed.ToString(), ref elem);
    }


}
