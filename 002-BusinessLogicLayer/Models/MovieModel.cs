﻿using MongoDB.Bson.Serialization.Attributes;
using System.Data;
using System.Diagnostics;

namespace ImdbServerCore
{
	public class MovieModel
	{
		private string _imdbID;
		private string _title;
		private string _poster;
		private string _userId;
		private int _year;

		[BsonId]
		public string imdbID
		{
			get { return _imdbID; }
			set { _imdbID = value; }
		}

		public string title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string poster
		{
			get { return _poster; }
			set { _poster = value; }
		}

		//[Required(ErrorMessage = "Missing user id.")]
		[PossibleID]
		public string userID
		{
			get { return _userId; }
			set { _userId = value; }
		}

		public int year
		{
			get { return _year; }
			set { _year = value; }
		}

		public override string ToString()
		{
			return
				imdbID + " " +
				title + " " +
				poster + " " +
				year + " " +
				userID;
		}

		public static MovieModel ToObject(DataRow reader)
		{
			MovieModel movieModel = new MovieModel();
			movieModel.imdbID = reader[0].ToString();
			movieModel.title = reader[1].ToString();
			movieModel.poster = reader[2].ToString();
			movieModel.year = int.Parse(reader[3].ToString());
			movieModel.userID = reader[4].ToString();

			Debug.WriteLine("MovieModel:" + movieModel.ToString());
			return movieModel;
		}
	}
}
