using Rpo.ApiServices.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.Efiling
{
    public class Common
    {
        public static Address GetContactAddressForJobDocument(JobContact jobContact)
        {
            Address address = new Address();

            //address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Addresses != null ? jobContact.Contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;

            //if (address == null)
            //{
            //    address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null && jobContact.Contact.Company.Addresses != null ? jobContact.Contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            //}
            address = null;
            if (jobContact != null && jobContact.Contact.IsPrimaryCompanyAddress != null && jobContact.Contact.IsPrimaryCompanyAddress == true)
            {
                address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null && jobContact.Contact.Company.Addresses != null ? jobContact.Contact.Company.Addresses.Where(x => x.Id == jobContact.Contact.IdPrimaryCompanyAddress).OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            }
            else if (address == null)
            {
                address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Addresses != null ? jobContact.Contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
            }
            return address;
        }

        public static Address GetContactAddressForJobDocument(Contact contact)
        {
            Address address = new Address();

            //address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;

            //if (address == null)
            //{
            //    address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            //}
            address = null;

            if (contact != null && contact.IsPrimaryCompanyAddress != null && contact.IsPrimaryCompanyAddress == true)
            {
                address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.Id == contact.IdPrimaryCompanyAddress).OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            }
            //address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;

            else if (address == null)
            {
                address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                //address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            }

            return address;
        }

        //public static string GetContactPhoneNumberForJobDocument(Contact contact)
        //{
        //    string phoneNumber = string.Empty;

        //    if (contact != null)
        //    {
        //        if (!string.IsNullOrEmpty(contact.WorkPhone))
        //        {
        //            phoneNumber = contact.WorkPhone;
        //        }

        //        if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.MobilePhone))
        //        {
        //            phoneNumber = contact.MobilePhone;
        //        }

        //        if (string.IsNullOrEmpty(phoneNumber))
        //        {
        //            Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
        //            phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
        //        }

        //        if (string.IsNullOrEmpty(phoneNumber))
        //        {
        //            Address address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
        //            phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
        //        }
        //    }

        //    return phoneNumber;
        //}

        //public static string GetContactPhoneNumberForJobDocument(JobContact jobContact)
        //{
        //    string phoneNumber = string.Empty;

        //    if (jobContact != null && jobContact.Contact != null)
        //    {
        //        Contact contact = jobContact.Contact;
        //        if (!string.IsNullOrEmpty(contact.WorkPhone))
        //        {
        //            phoneNumber = contact.WorkPhone;
        //        }

        //        if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.MobilePhone))
        //        {
        //            phoneNumber = contact.MobilePhone;
        //        }

        //        if (string.IsNullOrEmpty(phoneNumber))
        //        {
        //            Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
        //            phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
        //        }

        //        if (string.IsNullOrEmpty(phoneNumber))
        //        {
        //            Address address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
        //            phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
        //        }
        //    }

        //    return phoneNumber;
        //}


        public static string GetContactPhoneNumberForJobDocument(Contact contact)
        {
            string phoneNumber = string.Empty;

            if (contact != null)
            {
                Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                string addressPhoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
                if (!string.IsNullOrEmpty(addressPhoneNumber))
                {
                    phoneNumber = addressPhoneNumber;
                }
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    Address address1 = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                    phoneNumber = address1 != null && !string.IsNullOrEmpty(address1.Phone) ? address1.Phone : string.Empty;
                }
                if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.WorkPhone))
                {
                    //phoneNumber = contact.WorkPhone + (!string.IsNullOrEmpty(contact.WorkPhoneExt) ? " X" + contact.WorkPhoneExt : string.Empty);
                    phoneNumber = contact.WorkPhone;
                }
                if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.MobilePhone))
                {
                    phoneNumber = contact.MobilePhone;
                }
            }

            return phoneNumber;
        }

        public static string GetContactPhoneNumberForJobDocument(JobContact jobContact)
        {
            string phoneNumber = string.Empty;

            if (jobContact != null && jobContact.Contact != null)
            {
                Contact contact = jobContact.Contact;
                Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;

                if (address !=null &&!string.IsNullOrEmpty(address.Phone))
                {
                    //Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                    phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
                }
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    Address address1 = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;

                    phoneNumber = address1 != null && !string.IsNullOrEmpty(address1.Phone) ? address1.Phone : string.Empty;
                }
                if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.WorkPhone))
                {
                    //phoneNumber = contact.WorkPhone + (!string.IsNullOrEmpty(contact.WorkPhoneExt) ? " X" + contact.WorkPhoneExt : string.Empty);
                    phoneNumber = contact.WorkPhone;
                }
                if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.MobilePhone))
                {
                    phoneNumber = contact.MobilePhone;
                }
            }

            return phoneNumber;
        }

        public static string GetContactFaxNumberForJobDocument(JobContact jobContact)
        {
            string faxNumber = string.Empty;

            if (jobContact != null && jobContact.Contact != null)
            {
                Contact contact = jobContact.Contact;

                Address address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                faxNumber = address != null && !string.IsNullOrEmpty(address.Fax) ? address.Fax : string.Empty;
            }

            return faxNumber;
        }

        public static string GetContactFaxNumberForJobDocument(Contact contact)
        {
            string faxNumber = string.Empty;

            if (contact != null)
            {
                Address address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                faxNumber = address != null && !string.IsNullOrEmpty(address.Fax) ? address.Fax : string.Empty;
            }

            return faxNumber;
        }
    }
}
