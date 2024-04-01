﻿
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CustomerNotification
    {
        [Key]
        public int Id { get; set; }

        public string NotificationMessage { get; set; }

        public DateTime NotificationDate { get; set; }

        public int IdCustomerNotified { get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsView { get; set; } = false;

        public string RedirectionUrl { get; set; }

        [ForeignKey("IdCustomerNotified")]
        public Customer CustomerNotified { get; set; }
    }
}

