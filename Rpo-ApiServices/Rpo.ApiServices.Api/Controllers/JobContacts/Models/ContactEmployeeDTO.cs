using System.Collections.Generic;

namespace Rpo.ApiServices.Api.Controllers.JobContacts
{
    public class ContactEmployeeDTO
    {

        public string Value { get; set; }

        public string Text { get; set; }

        public bool Checked { get; set; }

        //public bool IsContact { get; set; }
    }

    public class ContactEmployeeGroupDTO
    {

        public string Value { get; set; }

        public string Text { get; set; }

        public bool Checked { get; set; }

        //public bool IsContact { get; set; }

        public List<ContactEmployeeDTO> Children { get; set; }
    }



    //public class ContactEmployeeDTO
    //{
    //    public string Id { get; set; }

    //    public bool IsContact { get; set; }

    //    public string ItemName { get; set; }

    //    public string LastName { get; set; }

    //    public string NameWithEmail { get; set; }

    //    public string Email { get; set; }

    //    public string FirstName { get; set; }

    //    public int? IdContact { get; set; }

    //    public int? IdGroup { get; set; }

    //}

}