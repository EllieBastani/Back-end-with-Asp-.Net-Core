using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Assign2_Library
{
    public class ChildInfo
    {
        public ChildInfo()
        {}
        public ChildInfo(string id,string userName,string firstName,string lastName,string email, DateTime birthdate,string street,string city,string province,string postalCode,string country,
            double latitude, double longitude,bool isNaughty,DateTime dateCreated)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthdate;
            Street = street;
            City = city;
            Province = province;
            PostalCode = postalCode;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
            IsNaughty = isNaughty;
            DateCreated = dateCreated;
        }
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public bool IsNaughty { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
