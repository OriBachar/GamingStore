﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Threading;
using GamingStore.Contracts;
using GamingStore.Models.Relationships;

namespace GamingStore.Models
{
    public class Store
    {
        public static int StoreCounter;

        public Store()
        {
            Orders = new List<Order>();
            StoreItems = new List<StoreItem>();
            Id = StoreCounter;
            Interlocked.Increment(ref StoreCounter);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, DataType(DataType.Text), StringLength(50)]
        public string Name { get; set; }

        [Required]
        public Address Address { get; set; }

        [DisplayName("Phone")]
        [Required]
        [RegularExpression("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-/0-9]*$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        //[DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [DisplayName("Opening Hours")]
        public List<OpeningHours> OpeningHours { get; set; } = new List<OpeningHours>(7);

        //public Dictionary<Item,uint> Stock { get; set; } // determines how many items there are in the store. example: {{fridge, 5},{mouse,6}}
        public ICollection<Order> Orders { get; set; }

        public ICollection<StoreItem> StoreItems { get; set; } // many to many relationship
        public bool Active { get; set; } = true;

        public bool IsOpen()
        {
            var currentDateTime = DateTime.Now;
            var dayOfWeek = (int)currentDateTime.DayOfWeek;
            var curTime = currentDateTime.TimeOfDay;

            //return true;
            return TimeSpan.Parse(OpeningHours[dayOfWeek].OpeningTime) <= curTime && curTime <= TimeSpan.Parse(OpeningHours[dayOfWeek].ClosingTime);
        }
    }
}