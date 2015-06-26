using Orkidea.PollaExpress.DAL;
using Orkidea.PollaExpress.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.PollaExpress.Business
{
    public class CustomerBiz
    {
        public List<Customer> GetCustomerList()
        {
            return CustomerCRUD.GetCustomerList();
        }

        public Customer GetCustomer(string id)
        {
            return CustomerCRUD.GetCustomerByKey(id);
        }


        public void SaveCustomer(Customer customer)
        {
            CustomerCRUD.SaveCustomer(customer);
        }


    }
}
