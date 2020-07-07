﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ImdbServerCore
{
	public class MySqlUsersManager : MySqlDataBase, IUsersRepository
	{
		private readonly AppSettings _appSettings;

		public MySqlUsersManager(IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
		}

		public MySqlUsersManager()
		{
		}

		public UserModel Authenticate(string username, string password)
		{
			var user = ReturnUserByNamePassword(username, password);

			// return null if user not found
			if (user == null || user.userID == null || user.userImdbPass == null || user.userID.Equals("") || user.userImdbPass.Equals(""))
				return null;

			// authentication successful so generate jwt token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.userID),
					new Claim(ClaimTypes.SerialNumber, user.userImdbPass)
				}),
				Expires = DateTime.UtcNow.AddMinutes(30),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			// remove additiona data before returning
			UserModel userModel = new UserModel();
			userModel.usertoken = tokenHandler.WriteToken(token);
			userModel.userNickName = user.userNickName;
			userModel.userPicture = user.userPicture;
			return userModel;
		}




		public List<UserModel> GetAllUsers()
		{
			DataTable dt = new DataTable();
			List<UserModel> arrUser = new List<UserModel>();

			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.GetAllUsers());
			}

			foreach (DataRow ms in dt.Rows)
			{
				arrUser.Add(UserModel.ToObject(ms));
			}

			return arrUser;
		}


		public UserModel GetOneUserById(string id)
		{
			DataTable dt = new DataTable();
			if (id.Equals(string.Empty) || id.Equals(""))
				throw new ArgumentOutOfRangeException();
			UserModel userModel = new UserModel();
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.GetOneUserById(id));
			}

			foreach (DataRow ms in dt.Rows)
			{
				userModel = UserModel.ToObject(ms);
			}

			return userModel;
		}


		public UserModel GetOneUserByName(string name)
		{
			DataTable dt = new DataTable();
			if (name.Equals(string.Empty) || name.Equals(""))
				throw new ArgumentOutOfRangeException();

			UserModel userModel = new UserModel();
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.GetOneUserByName(name));
			}

			foreach (DataRow ms in dt.Rows)
			{
				userModel = UserModel.ToObject(ms);
			}

			return userModel;
		}

		public UserModel AddUser(UserModel userModel)
		{
			string orPass = userModel.userPassword;
			DataTable dt = new DataTable();
			userModel.userPassword = ComputeHash.ComputeNewHash(userModel.userPassword);
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.AddUser(userModel));
			}
			foreach (DataRow ms in dt.Rows)
			{
				userModel = UserModel.ToObject(ms);
			}

			if (ComputeHash.ComputeNewHash(orPass).Equals(userModel.userPassword))
			{
				userModel.userPassword = orPass;
			}

			return userModel;
		}


		public UserModel UpdateUser(UserModel userModel)
		{
			string orPass = userModel.userPassword;
			DataTable dt = new DataTable();
			userModel.userPassword = ComputeHash.ComputeNewHash(userModel.userPassword);
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.UpdateUser(userModel));
			}

			foreach (DataRow ms in dt.Rows)
			{
				userModel = UserModel.ToObject(ms);
			}

			if (ComputeHash.ComputeNewHash(orPass).Equals(userModel.userPassword))
			{
				userModel.userPassword = orPass;
			}

			return userModel;
		}

		public int DeleteUser(string id)
		{
			int i = 0;
			using (MySqlCommand command = new MySqlCommand())
			{
				i = ExecuteNonQuery(UserStringsMySql.DeleteUser(id));
			}

			return i;
		}

		public LoginModel ReturnUserByNamePassword(LoginModel checkUser)
		{
			DataTable dt = new DataTable();
			checkUser.userPassword = ComputeHash.ComputeNewHash(checkUser.userPassword);
			if (checkUser == null)
				throw new ArgumentOutOfRangeException();
			LoginModel loginModel = new LoginModel();

			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.ReturnUserByNamePassword(checkUser));
			}

			foreach (DataRow ms in dt.Rows)
			{
				loginModel = LoginModel.ToObject(ms);
			}

			return loginModel;
		}

		public UserModel ReturnUserByNamePassword(string userNickName, string userPassword)
		{
			if (userNickName == null || userNickName.Equals(""))
				throw new ArgumentOutOfRangeException();
			if (userPassword == null || userPassword.Equals(""))
				throw new ArgumentOutOfRangeException();

			if (!CheckStringFormat.IsBase64String(userPassword))
			{
				userPassword = ComputeHash.ComputeNewHash(userPassword);
			}
			DataTable dt = new DataTable();

			UserModel userModel = new UserModel();

			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.ReturnUserByNamePassword(userNickName, userPassword));
			}

			foreach (DataRow ms in dt.Rows)
			{
				userModel = UserModel.ToObject(ms);
			}

			return userModel;
		}

		public string ReturnImdbPassByNamePassword(LoginModel checkUser)
		{
			DataTable dt = new DataTable();
			checkUser.userPassword = ComputeHash.ComputeNewHash(checkUser.userPassword);
			if (checkUser == null)
				throw new ArgumentOutOfRangeException();
			LoginModel loginModel = new LoginModel();

			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.ReturnUserByNamePassword(checkUser));
			}

			foreach (DataRow ms in dt.Rows)
			{
				loginModel = LoginModel.ToObject(ms);
			}

			return loginModel.userImdbPass;
		}

		public string ReturnUserIdByUserPass(string userPass)
		{
			DataTable dt = new DataTable();
			string userId = "";
			if (userPass.Equals(string.Empty) || userPass.Equals(""))
				throw new ArgumentOutOfRangeException();
			userPass = ComputeHash.ComputeNewHash(userPass);

			UserModel userModel = new UserModel();
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.ReturnUserIdByUserPass(userPass));
			}
			foreach (DataRow ms in dt.Rows)
			{
				userId = ms[0].ToString();
			}

			return userId;
		}

		public bool IsNameTaken(string name)
		{
			DataTable dt = new DataTable();
			if (name.Equals(string.Empty) || name.Equals(""))
				throw new ArgumentOutOfRangeException();
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.IsNameTaken(name));
			}

			int num = 0;

			foreach (DataRow ms in dt.Rows)
			{
				num = int.Parse(ms[0].ToString());
			}

			if (num == 0) return false;
			else return true;
		}

		public UserModel UploadUserImage(string id, string img)
		{
			UserModel userModel = new UserModel();
			DataTable dt = new DataTable();
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.UploadUserImage(id, img));
			}
			foreach (DataRow ms in dt.Rows)
			{
				userModel = UserModel.ToObject(ms);
			}

			return userModel;
		}

		public string ReturnUserIdByImdbPass(string imdbPass)
		{
			DataTable dt = new DataTable();
			string userId = "";
			if (imdbPass.Equals(string.Empty) || imdbPass.Equals(""))
				throw new ArgumentOutOfRangeException();
			using (MySqlCommand command = new MySqlCommand())
			{
				dt = GetMultipleQuery(UserStringsMySql.ReturnUserIdByImdbPass(imdbPass));
			}
			foreach (DataRow ms in dt.Rows)
			{
				userId = ms[0].ToString();
			}

			return userId;
		}
	}
}
