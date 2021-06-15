using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NovinTehran.Core.Models;

namespace NovinTehran.Web.ViewModels
{
    public class ServiceOrderTableViewModel
    {
        public ServiceOrderTableViewModel()
        {
        }
        public ServiceOrderTableViewModel(ServiceOrder serviceOrder)
        {
            this.ServiceOrder = serviceOrder;
            this.PersianDate = new PersianDateTime(serviceOrder.AddedDate).ToString();
            this.Customer = serviceOrder.CustomerFirstName;
            this.Customer = "-";
            if (serviceOrder.CustomerLastName != null)
            {
                this.Customer = serviceOrder.CustomerLastName;
            }
            else
            {
                if (serviceOrder.CustomerId != null)
                {
                    var user = serviceOrder.Customer.User;
                    this.Customer = $"{user.FirstName} {user.LastName}";
                }
                else
                {
                    this.Customer = "-";
                }
            }
        }
        public ServiceOrder ServiceOrder { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public string PersianDate { get; set; }

        public string Customer { get; set; }

        public string Phone { get; set; }
    }

    public class ViewServiceOrderViewModel
    {
        public ServiceOrder ServiceOrder { get; set; }
        public string PersianDate { get; set; }
        public List<ServiceOrderItemWithMainFeatureViewModel> ServiceOrderItems { get; set; }
    }
    public class ServiceOrderItemWithMainFeatureViewModel
    {
        //public ServiceOrderItem ServiceOrderItem { get; set; }
        public string MainFeature { get; set; }
    }
}