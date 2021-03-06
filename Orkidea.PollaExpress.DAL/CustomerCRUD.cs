﻿using Orkidea.PollaExpress.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.PollaExpress.DAL
{
    public static class CustomerCRUD
    {
        /*CRUD customer*/

        public static List<Customer> GetCustomerList()
        {

            List<Customer> lstCustomer = new List<Customer>();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstCustomer = ctx.Customer.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstCustomer;
        }

        public static Customer GetCustomerByKey(string id)
        {
            Customer oCustomer = new Customer();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oCustomer =
                        ctx.Customer.Where(x => x.id == id).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oCustomer;
        }

        public static void SaveCustomer(Customer customer)
        {

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the student exists
                    Customer oCustomer = GetCustomerByKey(customer.id);

                    if (oCustomer != null)
                    {
                        // if exists then edit 
                        ctx.Customer.Attach(oCustomer);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oCustomer, customer);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Customer.Add(customer);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (DbEntityValidationException e)
            {
                StringBuilder oError = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    oError.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                    {
                        oError.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                string msg = oError.ToString();
                throw new Exception(msg);
            }
            catch (Exception ex) { throw ex; }
        }

        public static void DeleteCustomer(string id)
        {
            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the school exists
                    Customer oCustomer = GetCustomerByKey(id);

                    if (oCustomer != null)
                    {
                        // if exists then edit 
                        ctx.Customer.Attach(oCustomer);
                        ctx.Customer.Remove(oCustomer);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    throw new Exception("No se puede eliminar esta sede porque existe información asociada a esta.");
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
