using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DCT.Monitor.Entities;
using My.SqlEngine;

namespace Monitor.DAL.Implementation.Geolocations
{
	public class MsSqlGeolocationRepository: SqlBaseRepository<Geolocation, int>, IGeolocationRepository
	{
		public class GeolocationParser : IDataLoader<Geolocation>
		{
			public Geolocation LoadData(System.Data.SqlClient.SqlDataReader reader, Geolocation data)
			{
                try
                {
                    if (data == null) data = new Geolocation();

                    data.IpMin = reader.GetField<long>(IpMin);
                    data.IpMax = reader.GetField<long>(IpMax);
                    /*data.CountryCode = reader.GetField<string>(CountryCode);
                    data.CountryName = reader.GetField<string>(CountryName);
                    data.State = reader.GetField<string>(State);
                    data.City = reader.GetField<string>(City);*/
                    data.Latitude = reader.GetField<float>(Latitude);
                    data.Longitude = reader.GetField<float>(Longitude);
                    /*data.ZipCode = reader.GetField<string>(ZipCode);
                    data.TimeZone = reader.GetField<string>(TimeZone);*/
                    return data;
                }
                catch
                {
                    data.Latitude = -1;
                    return data;
                }
			}
		}

		public const string IpMin = "IpMin";
		public const string IpMax = "IpMax";
		public const string CountryCode = "CountryCode";
		public const string CountryName = "CountryName";
		public const string State = "State";
		public const string City = "City";
		public const string Latitude = "Latitude";
		public const string Longitude = "Longitude";
		public const string ZipCode = "ZipCode";
		public const string TimeZone = "TimeZone";

		protected override void OnInitUpdateSp(StoredProc sp, Geolocation geolocation)
		{
			sp.SetParam(IpMin, SqlDbType.BigInt, geolocation.IpMin);
			sp.SetParam(IpMax, SqlDbType.BigInt, geolocation.IpMax);/*
			sp.SetParam(CountryCode, SqlDbType.NVarChar, 50, geolocation.CountryCode);
			sp.SetParam(CountryName, SqlDbType.NVarChar, 50, geolocation.CountryName);
			sp.SetParam(State, SqlDbType.NVarChar, 50, geolocation.State);
			sp.SetParam(City, SqlDbType.NVarChar, 50, geolocation.City);*/
			sp.SetParam(Latitude, SqlDbType.NVarChar, 50, geolocation.Latitude);
			sp.SetParam(Longitude, SqlDbType.NVarChar, 50, geolocation.Longitude);
			/*sp.SetParam(ZipCode, SqlDbType.NVarChar, 50, geolocation.ZipCode);
			sp.SetParam(TimeZone, SqlDbType.NVarChar, 50, geolocation.TimeZone);*/
		}

		protected override IDataLoader<Geolocation> OnCreateLoader()
		{
			return new GeolocationParser();
		}	
	}
}
