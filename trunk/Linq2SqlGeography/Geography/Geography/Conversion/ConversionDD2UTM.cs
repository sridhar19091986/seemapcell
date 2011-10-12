///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-11
///-----------------------------------




namespace Geography.CoordinateSystem
{
    using System;
    public class ConversionDD2UTM:ConversionBaseDD2<DD,UTM>
    {

        #region Public  function

        public override UTM Convert(DD dd)
        {
            Validation(dd);

            setVariables(dd.Latitude, dd.Longitude);

            //String longZone = getLongZone(longitude);
            LatZones latZones = new LatZones();
            //String latZone = latZones.getLatZone(latitude);

            //double _easting = getEasting();
            //double _northing = getNorthing(latitude);

            return new UTM()
            {
                Easting = getEasting(),
                Northing = getNorthing(dd.Latitude),
                ZoneLong = getLongZone(dd.Longitude),
                ZoneLat = latZones.getLatZone(dd.Latitude)
            };

            //UTM = longZone + " " + latZone + " " + ((int)_easting) + " "
            //    + ((int)_northing);
            // UTM = longZone + " " + latZone + " " + decimalFormat.format(_easting) +
            // " "+ decimalFormat.format(_northing);

            //return UTM;
        }
         
        #endregion

    }
}
