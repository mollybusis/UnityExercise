using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// Contains general Game data for the Detect gametype.
/// </summary>

public class DetectData : ReactData
{
    const string ATTRIBUTE_RANDOM_POSITIONS = "randomPositions";
    const string ATTRIBUTE_INCLUDE_RED = "includeRed";
    const string ATTRIBUTE_POSITION_RANGE_MIN_X = "positionRangeMinX";
    const string ATTRIBUTE_POSITION_RANGE_MAX_X = "positionRangeMaxX";
    const string ATTRIBUTE_POSITION_RANGE_MIN_Y = "positionRangeMinY";
    const string ATTRIBUTE_POSITION_RANGE_MAX_Y = "positionRangeMaxY";

    /// <summary>
    /// Indicates whether random Positions should be generated for Trials. If false, pre-determined Positions are used.
    /// </summary>
    private bool randomPositions = false;
    /// <summary>
    /// Indicates whether Stimuli should sometimes appear red and be ignored by the player.
    /// </summary>
    public bool includeRed = false;
    /// <summary>
    /// The minimum of the X range for randomly-generated Positions.
    /// </summary>
    private float positionRangeMinX = 0;
    /// <summary>
    /// The maximum of the X range for randomly-generated Positions.
    /// </summary>
    private float positionRangeMaxX = 0;
    /// <summary>
    /// The minimum of the Y range for randomly-generated Positions.
    /// </summary>
    private float positionRangeMinY = 0;
    /// <summary>
    /// The maximum of the Y range for randomly-generated Positions.
    /// </summary>
    private float positionRangeMaxY = 0;




    #region ACCESSORS

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

    public float PositionRangeMinX
    {
        get
        {
            return positionRangeMinX;
        }
    }

    public float PositionRangeMaxX
    {
        get
        {
            return positionRangeMaxX;
        }
    }

    public float PositionRangeMinY
    {
        get
        {
            return positionRangeMinY;
        }
    }

    public float PositionRangeMaxY
    {
        get
        {
            return positionRangeMaxY;
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

        XMLUtil.ParseAttribute(elem, ATTRIBUTE_RANDOM_POSITIONS, ref randomPositions, true);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_INCLUDE_RED , ref includeRed, true);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_POSITION_RANGE_MIN_X, ref positionRangeMinX, true);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_POSITION_RANGE_MAX_X, ref positionRangeMaxX, true);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_POSITION_RANGE_MIN_Y, ref positionRangeMinY, true);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_POSITION_RANGE_MAX_Y, ref positionRangeMaxY, true);
    }


    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);

        XMLUtil.CreateAttribute(ATTRIBUTE_RANDOM_POSITIONS, randomPositions.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_INCLUDE_RED, includeRed.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_RANGE_MIN_X, positionRangeMinX.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_RANGE_MAX_X, positionRangeMaxX.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_RANGE_MIN_Y, positionRangeMinY.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSITION_RANGE_MAX_Y, positionRangeMaxY.ToString(), ref elem);
    }
}
