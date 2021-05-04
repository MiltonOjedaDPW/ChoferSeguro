using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.ActiveDirectoryServices.Entities
{
    public class User
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Department { get; set; }
        public string Manager { get; set; }
        public string LogonUser { get; set; }
        public string CompanyName { get; set; }
        public string AccountExpires { get; set; }
        public string CountryCode { get; set; }
        public bool IsExist { get; set; }
        public List<Group> Groups { get; set; }
        public bool IsAccountActive { get; set; }
        public string JobFunction { get; set; }
        public string Description { get; set; }
        public string TelephoneNumber { get; set; }
        public string Mobile { get; set; }
        public string Initials { get; set; }
        public string CreationDate { get; set; }
        public string OrganizationalUnit { get; set; }
        public PrincipalContext DomainContext { get; set; }
    }
}
