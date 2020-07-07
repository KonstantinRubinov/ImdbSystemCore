﻿using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;

namespace ImdbServerCore
{
	public class LoginModel
	{
		private string _userNickName;
		private string _userPassword;
		private int? _userLevel;
		private string _userPicture;
		private string _userImdbPass;
		

		[Required(ErrorMessage = "Missing user nick name.")]
		[StringLength(40, ErrorMessage = "User nick name can't exceeds 40 chars.")]
		[MinLength(2, ErrorMessage = "User first nick mast be minimum 2 chars.")]
		public string userNickName
		{
			get { return _userNickName; }
			set { _userNickName = value; }
		}

		[Required(ErrorMessage = "Missing user password.")]
		public string userPassword
		{
			get { return _userPassword; }
			set { _userPassword = value; }
		}

		public int? userLevel
		{
			get { return _userLevel; }
			set { _userLevel = value; }
		}

		public string userPicture
		{
			get { return _userPicture; }
			set { _userPicture = value; }
		}

		public string userImdbPass
		{
			get { return _userImdbPass; }
			set { _userImdbPass = value; }
		}

		public override string ToString()
		{
			return
				userNickName + " " +
				userImdbPass + " " +
				userPicture;
		}

		public static LoginModel ToObject(DataRow reader)
		{
			LoginModel loginModel = new LoginModel();
			loginModel.userNickName = reader[0].ToString();
			loginModel.userImdbPass = reader[1].ToString();
			loginModel.userPicture = reader[2].ToString();
			if (!loginModel.userPicture.Equals(string.Empty) && !loginModel.userPicture.Equals(""))
			{
				loginModel.userPicture = "/assets/images/users/" + loginModel.userPicture;
			}

			Debug.WriteLine("LoginModel:" + loginModel.ToString());
			return loginModel;
		}
	}
}
