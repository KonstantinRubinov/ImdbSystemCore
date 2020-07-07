﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ImdbServerCore
{
	public class USER
	{
		private string _userID;
		private string _userFirstName;
		private string _userLastName;
		private string _userNickName;
		private string _userPassword;
		private string _userEmail;
		private string _userGender;
		private DateTime? _userBirthDate;
		private string _userPicture;
		private string _userImdbPass;

		[Key]
		[Required(ErrorMessage = "Missing user id.")]
		public string userID
		{
			get { return _userID; }
			set { _userID = value; }
		}

		[Required(ErrorMessage = "Missing user first name.")]
		[StringLength(40, ErrorMessage = "User first name can't exceeds 40 chars.")]
		[MinLength(2, ErrorMessage = "User first name mast be minimum 2 chars.")]
		[RegularExpression("^[A-Z].*$", ErrorMessage = "User first name must start with a capital letter.")]
		public string userFirstName
		{
			get { return _userFirstName; }
			set { _userFirstName = value; }
		}

		[Required(ErrorMessage = "Missing user last name.")]
		[StringLength(40, ErrorMessage = "User last name can't exceeds 40 chars.")]
		[MinLength(2, ErrorMessage = "User first last mast be minimum 2 chars.")]
		[RegularExpression("^[A-Z].*$", ErrorMessage = "User last name must start with a capital letter.")]
		public string userLastName
		{
			get { return _userLastName; }
			set { _userLastName = value; }
		}

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

		//const string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
		[Required(ErrorMessage = "Missing user email.")]
		//[RegularExpression(pattern, ErrorMessage = "User mail wrong.")]
		public string userEmail
		{
			get { return _userEmail; }
			set { _userEmail = value; }
		}


		[Required(ErrorMessage = "Missing user gender.")]
		public string userGender
		{
			get { return _userGender; }
			set { _userGender = value; }
		}



		public DateTime? userBirthDate
		{
			get { return _userBirthDate; }
			set { _userBirthDate = value; }
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
				userID + " " +
				userFirstName + " " +
				userLastName + " " +
				userNickName + " " +
				userBirthDate + " " +
				userGender + " " +
				userEmail + " " +
				userPassword + " " +
				userPicture + " " +
				userImdbPass;
		}



		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public USER()
		{
			this.MOVIES = new HashSet<MOVy>();
			this.MOVIEEXTENDS = new HashSet<MOVIEEXTEND>();
		}

		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<MOVy> MOVIES { get; set; }

		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<MOVIEEXTEND> MOVIEEXTENDS { get; set; }
	}
}
