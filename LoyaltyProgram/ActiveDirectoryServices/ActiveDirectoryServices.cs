using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Caucedo.ActiveDirectoryServices.Entities;
using System.Configuration;

namespace Caucedo.ActiveDirectoryServices.Service
{

    public enum UserContext
    {
        GetAll,
        GetActiveUsers,
        GetInactiveUsers
    }

    public enum GroupScoped
    {
        LocalMachine,
        GrobalDomain,
        UniversalDomains
    }

  
    public class ActiveDirectoryServices
    {
        private string LDAP;
        private DirectoryEntry ldapConnection;
        private PrincipalContext contexto;
        private bool IsCreatedService = false;
        private string LargeLDAP;

        private string _User { get; set; }
        private string _Password { get; set; }

        private bool IsUserAuth = false;

        //GET
        #region User
        public User GetUser(string LogonUserName)
        {
            LogonUserName = LogonUserName.ToUpper();
            string filter = $"(sAMAccountName={LogonUserName})";
            bool isExist = false;
            User Current_User = null;
            using (DirectoryEntry de = ldapConnection)
            {
                using (DirectorySearcher adSearch = new DirectorySearcher(de))
                {
                    adSearch.Filter = filter;
                    SearchResult adSearchResult = adSearch.FindOne();

                    string UserFullName = string.Empty;
                    string UserFirstName = string.Empty;
                    string UserlastName = string.Empty;
                    string UserMail = string.Empty;
                    string UserDepartment = string.Empty;
                    string Usermanager = string.Empty;
                    string UserLogonName = string.Empty;
                    string UsercompanyName = string.Empty;
                    string UserAccountexpires = string.Empty;
                    string UserCountryCode = string.Empty;
                    bool IsUserEnabled = false;
                    string UserJobFunction = string.Empty;
                    string Description = string.Empty;
                    string TelephoneNumber = string.Empty;
                    string Mobile = string.Empty;
                    string Initials = string.Empty;
                    string OU = string.Empty;
                    string CreationDate = string.Empty;

                    List<Group> Groups = new List<Group>();

                    if (adSearchResult != null)
                    {
                        UserFullName = adSearchResult.Properties.Contains("displayname") != false ? adSearchResult.Properties["displayname"][0].ToString() : "";
                        UserFirstName = adSearchResult.Properties.Contains("givenname") != false ? adSearchResult.Properties["givenname"][0].ToString() : "";
                        UserlastName = adSearchResult.Properties.Contains("sn") != false ? adSearchResult.Properties["sn"][0].ToString() : "";
                        UserMail = adSearchResult.Properties.Contains("mail") != false ? adSearchResult.Properties["mail"][0].ToString() : "";
                        UserDepartment = adSearchResult.Properties.Contains("department") != false ? adSearchResult.Properties["department"][0].ToString() : "";
                        Usermanager = adSearchResult.Properties.Contains("manager") != false ? adSearchResult.Properties["manager"][0].ToString() : "";
                        if (adSearchResult.Properties.Contains("manager") != false)
                        {
                            Usermanager = Usermanager.Substring(0, Usermanager.ToString().IndexOf(",")).Replace("CN=", "");
                        }
                        UserLogonName = adSearchResult.Properties.Contains("samaccountname") != false ? adSearchResult.Properties["samaccountname"][0].ToString() : "";
                        UsercompanyName = adSearchResult.Properties.Contains("company") != false ? adSearchResult.Properties["company"][0].ToString() : "";
                        UserAccountexpires = adSearchResult.Properties.Contains("accountexpires") != false ? adSearchResult.Properties["accountexpires"][0].ToString() : "";
                        UserCountryCode = adSearchResult.Properties.Contains("countrycode") != false ? adSearchResult.Properties["countrycode"][0].ToString() : "";
                        int flags = adSearchResult.Properties.Contains("userAccountControl") != false ? (int)adSearchResult.Properties["userAccountControl"][0] : 0;
                        UserJobFunction = GetProperty(adSearchResult, "title"); //de.Properties.Contains("Title") != false ? de.Properties["Title"][0].ToString() : "";
                        Description = GetProperty(adSearchResult, "Description");
                        TelephoneNumber = GetProperty(adSearchResult, "TelephoneNumber"); //
                        Mobile = GetProperty(adSearchResult, "mobile");
                        Initials = GetProperty(adSearchResult, "initials");
                        CreationDate = GetProperty(adSearchResult, "whenCreated");

                        OU = adSearchResult.Path;

                        if (flags > 0) { IsUserEnabled = !Convert.ToBoolean(flags & 0x0002); }

                        if (adSearchResult.Properties.Contains("memberof") != false)
                        {
                            foreach (var item in adSearchResult.Properties["memberof"])
                            {
                                //CN=ProductDevelopment,OU=Group O365,OU=DPWorld-Caucedo,DC=zfmc,DC=local
                                string grupo = item.ToString();
                                grupo = grupo.Substring(0, item.ToString().IndexOf(",")).Replace("CN=", "");
                                Groups.Add(new Group { GroupName = grupo });
                            }
                        }

                        isExist = true;

                        //Fill User Entity..
                        Current_User = new User()
                        {
                            FullName = UserFullName,
                            FirstName = UserFirstName,
                            LastName = UserlastName,
                            Mail = UserMail,
                            Department = UserDepartment,
                            Manager = Usermanager,
                            LogonUser = UserLogonName,
                            CompanyName = UsercompanyName,
                            AccountExpires = UserAccountexpires,
                            CountryCode = UserCountryCode,
                            Groups = Groups,
                            IsExist = isExist,
                            IsAccountActive = IsUserEnabled,
                            JobFunction = UserJobFunction,
                            Description = Description,
                            TelephoneNumber = TelephoneNumber,
                            Mobile = Mobile,
                            Initials = Initials,
                            DomainContext = contexto,
                            OrganizationalUnit = OU,
                            CreationDate = CreationDate
                        };
                    }
                    else { isExist = false; Current_User = null; }


                }


                return Current_User;
            }


        }
        public User GetUser(string FirstName, string LastName)
        {

            string filter = string.Format("(&(objectCategory=person)(objectClass=user)(givenname={0})(sn={1}))", FirstName, LastName);          //$"(sAMAccountName={LogonUserName})";
            bool isExist = false;
            User Current_User = null;
            using (DirectoryEntry de = ldapConnection)
            {
                using (DirectorySearcher adSearch = new DirectorySearcher(de))
                { 
                    adSearch.Filter = filter;
                    SearchResult adSearchResult = adSearch.FindOne();

                    string UserFullName = string.Empty;
                    string UserFirstName = string.Empty;
                    string UserlastName = string.Empty;
                    string UserMail = string.Empty;
                    string UserDepartment = string.Empty;
                    string Usermanager = string.Empty;
                    string UserLogonName = string.Empty;
                    string UsercompanyName = string.Empty;
                    string UserAccountexpires = string.Empty;
                    string UserCountryCode = string.Empty;
                    bool IsUserEnabled = false;
                    string UserJobFunction = string.Empty;
                    string Description = string.Empty;
                    string TelephoneNumber = string.Empty;
                    string Mobile = string.Empty;
                    List<Group> Groups = new List<Group>();
                    string Initials = string.Empty;
                    string OU = string.Empty;
                    string CreationDate = string.Empty;

                    if (adSearchResult != null)
                    {
                        UserFullName = adSearchResult.Properties.Contains("displayname") != false ? adSearchResult.Properties["displayname"][0].ToString() : "";
                        UserFirstName = adSearchResult.Properties.Contains("givenname") != false ? adSearchResult.Properties["givenname"][0].ToString() : "";
                        UserlastName = adSearchResult.Properties.Contains("sn") != false ? adSearchResult.Properties["sn"][0].ToString() : "";
                        UserMail = adSearchResult.Properties.Contains("mail") != false ? adSearchResult.Properties["mail"][0].ToString() : "";
                        UserDepartment = adSearchResult.Properties.Contains("department") != false ? adSearchResult.Properties["department"][0].ToString() : "";
                        Usermanager = adSearchResult.Properties.Contains("manager") != false ? adSearchResult.Properties["manager"][0].ToString() : "";
                        if (adSearchResult.Properties.Contains("manager") != false)
                        {
                            Usermanager = Usermanager.Substring(0, Usermanager.ToString().IndexOf(",")).Replace("CN=", "");
                        }
                        UserLogonName = adSearchResult.Properties.Contains("samaccountname") != false ? adSearchResult.Properties["samaccountname"][0].ToString() : "";
                        UsercompanyName = adSearchResult.Properties.Contains("company") != false ? adSearchResult.Properties["company"][0].ToString() : "";
                        UserAccountexpires = adSearchResult.Properties.Contains("accountexpires") != false ? adSearchResult.Properties["accountexpires"][0].ToString() : "";
                        UserCountryCode = adSearchResult.Properties.Contains("countrycode") != false ? adSearchResult.Properties["countrycode"][0].ToString() : "";
                        UserJobFunction = GetProperty(adSearchResult, "title");
                        Description = GetProperty(adSearchResult, "Description");
                        TelephoneNumber = GetProperty(adSearchResult, "TelephoneNumber");
                        Mobile = GetProperty(adSearchResult, "mobile");
                        Initials = GetProperty(adSearchResult, "initials");
                        OU = adSearchResult.Path;
                        CreationDate = GetProperty(adSearchResult, "whenCreated");

                        int flags = adSearchResult.Properties.Contains("userAccountControl") != false ? (int)adSearchResult.Properties["userAccountControl"][0] : 0;
                        if (flags > 0) { IsUserEnabled = !Convert.ToBoolean(flags & 0x0002); }


                        if (adSearchResult.Properties.Contains("memberof") != false)
                        {
                            foreach (var item in adSearchResult.Properties["memberof"])
                            {
                                //CN=ProductDevelopment,OU=Group O365,OU=DPWorld-Caucedo,DC=zfmc,DC=local
                                string grupo = item.ToString();
                                grupo = grupo.Substring(0, item.ToString().IndexOf(",")).Replace("CN=", "");
                                Groups.Add(new Group { GroupName = grupo });
                            }
                        }

                        isExist = true;

                        //Fill User Entity..
                        Current_User = new User()
                        {
                            FullName = UserFullName,
                            FirstName = UserFirstName,
                            LastName = UserlastName,
                            Mail = UserMail,
                            Department = UserDepartment,
                            Manager = Usermanager,
                            LogonUser = UserLogonName,
                            CompanyName = UsercompanyName,
                            AccountExpires = UserAccountexpires,
                            CountryCode = UserCountryCode,
                            Groups = Groups,
                            IsExist = isExist,
                            IsAccountActive = IsUserEnabled,
                            JobFunction = UserJobFunction,
                            Description = Description,
                            TelephoneNumber = TelephoneNumber,
                            Mobile = Mobile,
                            Initials = Initials,
                            DomainContext = contexto,
                            OrganizationalUnit = OU,
                            CreationDate = CreationDate
                        };
                    }
                    else { isExist = false; Current_User = null; }


                }


                return Current_User;
            }


        }
        public User GetUser(string FirstName, string LastName, string Department)
        {
            string filter = string.Format("(&(objectCategory=person)(objectClass=user)(givenname={0})(sn={1})(department={2}) )", FirstName, LastName, Department);          //$"(sAMAccountName={LogonUserName})";
            bool isExist = false;
            User Current_User = null;
            using (DirectoryEntry de = ldapConnection)
            {
                using (DirectorySearcher adSearch = new DirectorySearcher(de))
                {
                    adSearch.Filter = filter;
                    SearchResult adSearchResult = adSearch.FindOne();

                    string UserFullName = string.Empty;
                    string UserFirstName = string.Empty;
                    string UserlastName = string.Empty;
                    string UserMail = string.Empty;
                    string UserDepartment = string.Empty;
                    string Usermanager = string.Empty;
                    string UserLogonName = string.Empty;
                    string UsercompanyName = string.Empty;
                    string UserAccountexpires = string.Empty;
                    string UserCountryCode = string.Empty;
                    bool IsUserEnabled = false;
                    string UserJobFunction = string.Empty;
                    List<Group> Groups = new List<Group>();
                    string Description = string.Empty;
                    string TelephoneNumber = string.Empty;
                    string Mobile = string.Empty;
                    string Initials = string.Empty;
                    string OU = string.Empty;
                    string CreationDate = string.Empty;

                    if (adSearchResult != null)
                    {
                        UserFullName = adSearchResult.Properties.Contains("displayname") != false ? adSearchResult.Properties["displayname"][0].ToString() : "";
                        UserFirstName = adSearchResult.Properties.Contains("givenname") != false ? adSearchResult.Properties["givenname"][0].ToString() : "";
                        UserlastName = adSearchResult.Properties.Contains("sn") != false ? adSearchResult.Properties["sn"][0].ToString() : "";
                        UserMail = adSearchResult.Properties.Contains("mail") != false ? adSearchResult.Properties["mail"][0].ToString() : "";
                        UserDepartment = adSearchResult.Properties.Contains("department") != false ? adSearchResult.Properties["department"][0].ToString() : "";
                        Usermanager = adSearchResult.Properties.Contains("manager") != false ? adSearchResult.Properties["manager"][0].ToString() : "";
                        UserJobFunction = GetProperty(adSearchResult, "title");
                        Description = GetProperty(adSearchResult, "Description");
                        TelephoneNumber = GetProperty(adSearchResult, "TelephoneNumber");
                        Mobile = GetProperty(adSearchResult, "mobile");
                        Initials = GetProperty(adSearchResult, "initials");
                        OU = adSearchResult.Path;
                        CreationDate = GetProperty(adSearchResult, "whenCreated");

                        if (adSearchResult.Properties.Contains("manager") != false)
                        {
                            Usermanager = Usermanager.Substring(0, Usermanager.ToString().IndexOf(",")).Replace("CN=", "");
                        }
                        UserLogonName = adSearchResult.Properties.Contains("samaccountname") != false ? adSearchResult.Properties["samaccountname"][0].ToString() : "";
                        UsercompanyName = adSearchResult.Properties.Contains("company") != false ? adSearchResult.Properties["company"][0].ToString() : "";
                        UserAccountexpires = adSearchResult.Properties.Contains("accountexpires") != false ? adSearchResult.Properties["accountexpires"][0].ToString() : "";
                        UserCountryCode = adSearchResult.Properties.Contains("countrycode") != false ? adSearchResult.Properties["countrycode"][0].ToString() : "";

                        int flags = adSearchResult.Properties.Contains("userAccountControl") != false ? (int)adSearchResult.Properties["userAccountControl"][0] : 0;
                        if (flags > 0) { IsUserEnabled = !Convert.ToBoolean(flags & 0x0002); }

                        if (adSearchResult.Properties.Contains("memberof") != false)
                        {
                            foreach (var item in adSearchResult.Properties["memberof"])
                            {
                                //CN=ProductDevelopment,OU=Group O365,OU=DPWorld-Caucedo,DC=zfmc,DC=local
                                string grupo = item.ToString();
                                grupo = grupo.Substring(0, item.ToString().IndexOf(",")).Replace("CN=", "");
                                Groups.Add(new Group { GroupName = grupo });
                            }
                        }

                        isExist = true;

                        //Fill User Entity..
                        Current_User = new User()
                        {
                            FullName = UserFullName,
                            FirstName = UserFirstName,
                            LastName = UserlastName,
                            Mail = UserMail,
                            Department = UserDepartment,
                            Manager = Usermanager,
                            LogonUser = UserLogonName,
                            CompanyName = UsercompanyName,
                            AccountExpires = UserAccountexpires,
                            CountryCode = UserCountryCode,
                            Groups = Groups,
                            IsExist = isExist,
                            IsAccountActive = IsUserEnabled,
                            JobFunction = UserJobFunction,
                            Description = Description,
                            TelephoneNumber = TelephoneNumber,
                            Mobile = Mobile,
                            Initials = Initials,
                            DomainContext = contexto,
                            OrganizationalUnit = OU,
                            CreationDate = CreationDate
                        };
                    }
                    else { isExist = false; Current_User = null; }


                }


                return Current_User;
            }


        }
        public User GetUserByEmployeeCode(string EmployeeCode)
        {
            EmployeeCode = EmployeeCode.ToUpper();
            string filter = $"(Description={EmployeeCode})";
            bool isExist = false;
            User Current_User = null;
            using (DirectoryEntry de = ldapConnection)
            {
                using (DirectorySearcher adSearch = new DirectorySearcher(de))
                {
                    adSearch.Filter = filter;
                    SearchResult adSearchResult = adSearch.FindOne();

                    string UserFullName = string.Empty;
                    string UserFirstName = string.Empty;
                    string UserlastName = string.Empty;
                    string UserMail = string.Empty;
                    string UserDepartment = string.Empty;
                    string Usermanager = string.Empty;
                    string UserLogonName = string.Empty;
                    string UsercompanyName = string.Empty;
                    string UserAccountexpires = string.Empty;
                    string UserCountryCode = string.Empty;
                    bool IsUserEnabled = false;
                    string UserJobFunction = string.Empty;
                    string Description = string.Empty;
                    string TelephoneNumber = string.Empty;
                    string Mobile = string.Empty;
                    string Initials = string.Empty;
                    string OU = string.Empty;
                    string CreationDate = string.Empty;

                    List<Group> Groups = new List<Group>();

                    if (adSearchResult != null)
                    {
                        UserFullName = adSearchResult.Properties.Contains("displayname") != false ? adSearchResult.Properties["displayname"][0].ToString() : "";
                        UserFirstName = adSearchResult.Properties.Contains("givenname") != false ? adSearchResult.Properties["givenname"][0].ToString() : "";
                        UserlastName = adSearchResult.Properties.Contains("sn") != false ? adSearchResult.Properties["sn"][0].ToString() : "";
                        UserMail = adSearchResult.Properties.Contains("mail") != false ? adSearchResult.Properties["mail"][0].ToString() : "";
                        UserDepartment = adSearchResult.Properties.Contains("department") != false ? adSearchResult.Properties["department"][0].ToString() : "";
                        Usermanager = adSearchResult.Properties.Contains("manager") != false ? adSearchResult.Properties["manager"][0].ToString() : "";
                        if (adSearchResult.Properties.Contains("manager") != false)
                        {
                            Usermanager = Usermanager.Substring(0, Usermanager.ToString().IndexOf(",")).Replace("CN=", "");
                        }
                        UserLogonName = adSearchResult.Properties.Contains("samaccountname") != false ? adSearchResult.Properties["samaccountname"][0].ToString() : "";
                        UsercompanyName = adSearchResult.Properties.Contains("company") != false ? adSearchResult.Properties["company"][0].ToString() : "";
                        UserAccountexpires = adSearchResult.Properties.Contains("accountexpires") != false ? adSearchResult.Properties["accountexpires"][0].ToString() : "";
                        UserCountryCode = adSearchResult.Properties.Contains("countrycode") != false ? adSearchResult.Properties["countrycode"][0].ToString() : "";
                        int flags = adSearchResult.Properties.Contains("userAccountControl") != false ? (int)adSearchResult.Properties["userAccountControl"][0] : 0;
                        UserJobFunction = GetProperty(adSearchResult, "title"); //de.Properties.Contains("Title") != false ? de.Properties["Title"][0].ToString() : "";
                        Description = GetProperty(adSearchResult, "Description");
                        TelephoneNumber = GetProperty(adSearchResult, "TelephoneNumber");
                        Mobile = GetProperty(adSearchResult, "mobile");
                        Initials = GetProperty(adSearchResult, "initials");
                        OU = adSearchResult.Path;
                        CreationDate = GetProperty(adSearchResult, "whenCreated");

                        if (flags > 0) { IsUserEnabled = !Convert.ToBoolean(flags & 0x0002); }

                        if (adSearchResult.Properties.Contains("memberof") != false)
                        {
                            foreach (var item in adSearchResult.Properties["memberof"])
                            {
                                //CN=ProductDevelopment,OU=Group O365,OU=DPWorld-Caucedo,DC=zfmc,DC=local
                                string grupo = item.ToString();
                                grupo = grupo.Substring(0, item.ToString().IndexOf(",")).Replace("CN=", "");
                                Groups.Add(new Group { GroupName = grupo });
                            }
                        }

                        isExist = true;

                        //Fill User Entity..
                        Current_User = new User()
                        {
                            FullName = UserFullName,
                            FirstName = UserFirstName,
                            LastName = UserlastName,
                            Mail = UserMail,
                            Department = UserDepartment,
                            Manager = Usermanager,
                            LogonUser = UserLogonName,
                            CompanyName = UsercompanyName,
                            AccountExpires = UserAccountexpires,
                            CountryCode = UserCountryCode,
                            Groups = Groups,
                            IsExist = isExist,
                            IsAccountActive = IsUserEnabled,
                            JobFunction = UserJobFunction,
                            Description = Description,
                            TelephoneNumber = TelephoneNumber,
                            Mobile = Mobile,
                            Initials = Initials,
                            DomainContext = contexto,
                            OrganizationalUnit = OU,
                            CreationDate = CreationDate
                        };
                    }
                    else { isExist = false; Current_User = null; }


                }


                return Current_User;
            }


        }
        public List<User> GetUserList(UserContext userContext = UserContext.GetAll)
        {

            string domain = LDAP.Replace("LDAP://", "");
            User Current_User = null;
            List<User> Users = new List<User>();

            PrincipalContext context = null;

            if (IsUserAuth)
            {
                context = new PrincipalContext(ContextType.Domain, domain, _User, _Password);
            }
            else
            {
                context = new PrincipalContext(ContextType.Domain, domain);
            }

            using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
            {

                foreach (var result in searcher.FindAll())
                {
                    string UserFullName = string.Empty;
                    string UserFirstName = string.Empty;
                    string UserlastName = string.Empty;
                    string UserMail = string.Empty;
                    string UserDepartment = string.Empty;
                    string Usermanager = string.Empty;
                    string UserLogonName = string.Empty;
                    string UsercompanyName = string.Empty;
                    string UserAccountexpires = string.Empty;
                    string UserCountryCode = string.Empty;
                    string UserJobFunction = string.Empty;
                    string Description = string.Empty;
                    string TelephoneNumber = string.Empty;
                    string Mobile = string.Empty;
                    string Initials = string.Empty;
                    string OU = string.Empty;
                    string CreationDate = string.Empty;

                    bool IsUserEnabled = false;

                    List<Group> Groups = new List<Group>();

                    DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                    if (de.Properties.Contains("userAccountControl"))
                    {
                        int value = (int)de.Properties["userAccountControl"].Value;
                        IsUserEnabled = !Convert.ToBoolean(value & 0x0002);
                    }

                    if (userContext == UserContext.GetActiveUsers && IsUserEnabled == false)
                    {
                        continue;
                    }

                    if (userContext == UserContext.GetInactiveUsers && IsUserEnabled == true)
                    {
                        continue;
                    }

                    UserFullName = de.Properties.Contains("displayname") != false ? de.Properties["displayname"][0].ToString() : "";
                    UserFirstName = de.Properties.Contains("givenname") != false ? de.Properties["givenname"][0].ToString() : "";
                    UserlastName = de.Properties.Contains("sn") != false ? de.Properties["sn"][0].ToString() : "";
                    UserMail = de.Properties.Contains("mail") != false ? de.Properties["mail"][0].ToString() : ""; //userPrincipalName
                    UserDepartment = de.Properties.Contains("department") != false ? de.Properties["department"][0].ToString() : "";
                    Usermanager = de.Properties.Contains("manager") != false ? de.Properties["manager"].Value.ToString() : "";
                    if (de.Properties.Contains("manager") != false)
                    {
                        Usermanager = Usermanager.Substring(0, Usermanager.ToString().IndexOf(",")).Replace("CN=", "");
                    }
                    UserLogonName = de.Properties.Contains("samaccountname") != false ? de.Properties["samaccountname"][0].ToString() : "";
                    UsercompanyName = de.Properties.Contains("company") != false ? de.Properties["company"][0].ToString() : "";
                    UserAccountexpires = de.Properties.Contains("accountexpires") != false ? de.Properties["accountexpires"].Value.ToString() : "";
                    UserCountryCode = de.Properties.Contains("countrycode") != false ? de.Properties["countrycode"][0].ToString() : "";
                    UserJobFunction = de.Properties.Contains("Title") != false ? de.Properties["Title"][0].ToString() : "";
                    Description = de.Properties.Contains("Description") != false ? de.Properties["Description"][0].ToString() : "";
                    TelephoneNumber = de.Properties.Contains("TelephoneNumber") != false ? de.Properties["TelephoneNumber"][0].ToString() : "";
                    Mobile = de.Properties.Contains("mobile") != false ? de.Properties["mobile"][0].ToString() : "";
                    Initials = de.Properties.Contains("initials") != false ? de.Properties["initials"][0].ToString() : "";
                    CreationDate = de.Properties.Contains("whenCreated") != false ? de.Properties["whenCreated"][0].ToString() : "";

                    OU = de.Path;

                    if (de.Properties.Contains("mail"))
                    {
                        string mail = de.Properties["mail"].Value.ToString();
                    }


                    if (de.Properties.Contains("memberof") != false)
                    {
                        Groups.Clear();
                        foreach (var item in de.Properties["memberof"])
                        {
                            //CN=ProductDevelopment,OU=Group O365,OU=DPWorld-Caucedo,DC=zfmc,DC=local
                            string grupo = item.ToString();
                            grupo = grupo.Substring(0, item.ToString().IndexOf(",")).Replace("CN=", "");
                            Groups.Add(new Group { GroupName = grupo });
                        }
                    }


                    Current_User = new User()
                    {
                        FullName = UserFullName,
                        FirstName = UserFirstName,
                        LastName = UserlastName,
                        Mail = UserMail,
                        Department = UserDepartment,
                        Manager = Usermanager,
                        LogonUser = UserLogonName,
                        CompanyName = UsercompanyName,
                        AccountExpires = UserAccountexpires,
                        CountryCode = UserCountryCode,
                        Groups = Groups,
                        IsAccountActive = IsUserEnabled,
                        JobFunction = UserJobFunction,
                        Description = Description,
                        TelephoneNumber = TelephoneNumber,
                        Mobile = Mobile,
                        Initials = Initials,
                        DomainContext = contexto,
                        OrganizationalUnit = OU,
                        CreationDate = CreationDate
                    };

                    Users.Add(Current_User);

                }
            }

            return Users;
        }
        public List<User> GetGroupMembersByGroupName(string groupName)
        {
            string domain = LDAP.Replace("LDAP://", "");
            List<User> members = new List<User>();

            PrincipalContext ctx = null;
            if (IsUserAuth)
            {
                ctx = new PrincipalContext(ContextType.Domain, domain, _User, _Password);
            }
            else
            {
                ctx = new PrincipalContext(ContextType.Domain, domain);
            }


            // find the group in question
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName);

            // if found....
            if (group != null)
            {
                // iterate over the group's members
                foreach (Principal p in group.GetMembers())
                {
                    //Console.WriteLine("{0}: {1}", p.StructuralObjectClass, p.DisplayName);

                    // do whatever else you need to do to those members
                    var user = GetUser(p.SamAccountName);

                    if (user != null)
                    {
                        members.Add(user);
                    }
                }

            }
            ctx.Dispose();

            return members;
        }
        public bool IsUserValidCredientials(string userLogon, string userPassword)
        {
            bool IsUserAuthenticated = false;
            string domain = LDAP.Replace("LDAP://", "");

            PrincipalContext pc = null;
            if (IsUserAuth)
            {
                pc = new PrincipalContext(ContextType.Domain, domain, _User, _Password);
            }
            else
            {
                pc = new PrincipalContext(ContextType.Domain, domain);
            }


            // validate the credentials
            IsUserAuthenticated = pc.ValidateCredentials(userLogon, userPassword);

            pc.Dispose();

            return IsUserAuthenticated;
        }
        #endregion

        #region Department
        public List<string> GetDepartmentList()
        {
            List<string> departments = new List<string>();

            using (DirectoryEntry de = ldapConnection)
            {
                using (DirectorySearcher adSearch = new DirectorySearcher(de))
                {
                    adSearch.PropertiesToLoad.Add("department");

                    SearchResultCollection results = adSearch.FindAll();

                    foreach (SearchResult item in results)
                    {
                        string department = string.Empty;
                        DirectoryEntry den = item.GetDirectoryEntry();

                        try
                        {
                            if (den.Properties.Contains("department"))
                            {
                                department = den.Properties["department"][0].ToString();

                                if (!departments.Contains(department))
                                {
                                    departments.Add(department);
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return departments;
        }
        #endregion

        #region Security Groups
        public List<Group> GetGroupsByUserName(string userLogon)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();
            string domain = LDAP.Replace("LDAP://", "");

            List<Group> Groups = new List<Group>();

            // establish domain context
            PrincipalContext yourDomain =  contexto;

            try
            {
                // find your user
                UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, userLogon.ToUpper());

                // if found - grab its groups
                if (user != null)
                {
                    PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

                    // iterate over all groups
                    foreach (Principal p in groups)
                    {
                        // make sure to add only group principals
                        //if (p is GroupPrincipal)
                        //{
                        //result.Add((GroupPrincipal)p);
                        GroupPrincipal currentGroup = (GroupPrincipal)p;
                        Groups.Add(new Group
                        {
                            GroupName = currentGroup.Name,
                            GroupDescription = currentGroup.Description,
                            IsSecurityGroup = currentGroup.IsSecurityGroup,
                            Context = domain
                        });

                        //}
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }

            return Groups;
        }


        public List<Group> GetGroupList()
        {

            string domain = LDAP.Replace("LDAP://", "");
            List<Group> groups = new List<Group>();

            DirectoryEntry de = new DirectoryEntry(domain);
            de.Username = _User;
            de.Password = _Password;

            DirectorySearcher deSearch = new DirectorySearcher(de.Path);

            SearchResultCollection results;
            try
            {
                deSearch.Filter = ("(&(objectCategory=group))");
                deSearch.SearchScope = SearchScope.Subtree;
                //deSearch.SizeLimit = 10000;
                deSearch.PageSize = 1000;
                results = deSearch.FindAll();

                foreach (SearchResult result in results)
                {

                    string grupo = result.Properties["cn"][0].ToString();
                    groups.Add(new Group { GroupName = grupo });

                }
                de.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (deSearch != null)
                {
                    deSearch.Dispose();
                }
                if (de != null)
                {
                    de.Dispose();
                }
            }

            return groups;

        }

        public Group GetGroupByName(string groupName)
        {
            Group group = null;
            string domain = LDAP.Replace("LDAP://", "");
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain,domain))
            {
                // define a "query-by-example" principal - here, we search for a GroupPrincipal 
                // and with the name like some pattern
                GroupPrincipal qbeGroup = new GroupPrincipal(ctx);
                qbeGroup.Name = $"{groupName}*";

                // create your principal searcher passing in the QBE principal    
                PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

                int scoped = qbeGroup.GroupScope != null ? (int)qbeGroup.GroupScope : 0; 

                // find all matches
                foreach (var found in srch.FindAll())
                {
                    // do whatever here - "found" is of type "Principal"
                    group = new Group
                    {
                        GroupDescription = found.Description,
                        GroupName = found.Name,
                        Context = domain,
                        IsSecurityGroup = qbeGroup.IsSecurityGroup,
                        Scope = scoped
                    };

                    break;
                }
            }

            return group;
        }
        public bool AddUserToGroup(string userLogon, string groupName)
        {
            bool IsAdded = false;
            try
            {
                string domain = LDAP.Replace("LDAP://", "");
                using (PrincipalContext pc = contexto)
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupName);
                    group.Members.Add(pc, IdentityType.SamAccountName, userLogon);
                    group.Save();
                    IsAdded = true;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //doSomething with E.Message.ToString(); 
                IsAdded = false;

            }

            return IsAdded;
        }
        public bool RemoveUserFromGroup(string userLogon, string groupName)
        {
            bool IsRemoved = false;
            try
            {
                string domain = LDAP.Replace("LDAP://", "");

                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain, _User, _Password))
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupName);
                    group.Members.Remove(pc, IdentityType.SamAccountName, userLogon);
                    group.Save();
                    IsRemoved = true;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                IsRemoved = false;

            }

            return IsRemoved;
        }
        public bool IsGroupExist(string groupName)
        {

            bool isExist = false;
            string domain = LDAP.Replace("LDAP://", "");
            try
            {
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain,domain))
                {
                    // define a "query-by-example" principal - here, we search for a GroupPrincipal 
                    // and with the name like some pattern
                    GroupPrincipal qbeGroup = new GroupPrincipal(ctx);
                    qbeGroup.Name = $"{groupName}*";

                    // create your principal searcher passing in the QBE principal    
                    PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

                    foreach (var found in srch.FindAll())
                    {
                        isExist = true;
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                isExist = false;
            }

            return isExist;
        }
        public List<Group> GetGroupsByListName(List<string> groupsName)
        {
            List<Group> groups = new List<Group>();

            foreach (var group_name in groupsName)
            {
                var group = GetGroupByName(group_name);

                if (group != null)
                {
                    groups.Add(group);
                }
            }
            return groups;
        }
        public int GetGroupMembersCount(string groupName)
        {
            int count = 0;
            string domain = LDAP.Replace("LDAP://", "");
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain,domain)) //new PrincipalContext(ContextType.Domain, null, "OU=YourOU,DC=YourCompany,DC=Com"))
            {
                // find the group in question
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName);

                // if found....
                if (group != null)
                {
                    count = group.Members.Count;
                }
            }

            return count;
        }
        public Group CreateGroup(string groupName,string groupDescription,bool issecurityGroup, GroupScoped groupScope)
        {

            Group New_group = null;
            GroupScope current = GroupScope.Local;

            switch (groupScope)
            {
                case GroupScoped.GrobalDomain:
                    current = GroupScope.Global;
                    break;

                case GroupScoped.LocalMachine:
                    current = GroupScope.Local;
                    break;

                case GroupScoped.UniversalDomains:
                    current = GroupScope.Universal;
                    break;
            }

            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(groupDescription))
            {
                return null;
            }

            using (PrincipalContext ctx = contexto) //new PrincipalContext(ContextType.Domain, null, "OU=YourOU,DC=YourCompany,DC=Com"))
            {
                // find the group in question
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx,groupName);

                // if found....
                if (group != null)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        GroupPrincipal oGroupPrincipal = new GroupPrincipal(ctx);
                        oGroupPrincipal.Name = groupName;
                        oGroupPrincipal.Description = groupDescription;
                        oGroupPrincipal.GroupScope = current;
                        oGroupPrincipal.IsSecurityGroup = issecurityGroup;
                        oGroupPrincipal.Save();

                        New_group = new Group
                        {
                            Context = contexto.ConnectedServer,
                            GroupName = groupName,
                            GroupDescription = groupDescription,
                            IsSecurityGroup = issecurityGroup
                        };
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

            return New_group;
        }
        #endregion

        #region OU
        public  List<string> GetOUList()
        {

            string domain = LDAP.Replace("LDAP://", "");

            List<string> orgUnits = new List<string>();

            DirectoryEntry startingPoint = null;

            if (IsUserAuth)
            {
                startingPoint = new DirectoryEntry(LargeLDAP, _User, _Password); //"LDAP://DC=zfmc,DC=local"
            }
            else
            {
                startingPoint = new DirectoryEntry(LargeLDAP);
            }

            DirectorySearcher searcher = new DirectorySearcher(startingPoint);
            searcher.Filter = "(objectCategory=organizationalUnit)";

            foreach (SearchResult res in searcher.FindAll())
            {
                orgUnits.Add(res.Path);

            }

            searcher.Dispose();
            return orgUnits;
        }
        #endregion

        //Constructors
        public ActiveDirectoryServices(string LDAP_Connection, string User, string Password){
            CreateService(LDAP_Connection, User, Password);
        }

        public ActiveDirectoryServices(string LDAP_Connection) {
            CreateService(LDAP_Connection);
        }

      

        //Service
        public bool CreateService(string LDAP_Connection, string User = "", string Password = "")
        {
            string domain = string.Empty;
            try
            {

                LDAP = LDAP_Connection;
                domain = LDAP.Replace("LDAP://", "");

                GetDomainConnection(domain);

                if (User.Length > 0 && Password.Length > 0)
                {
                    ldapConnection = new DirectoryEntry(LDAP_Connection, User, Password);
                    IsCreatedService = true;

                    IsUserAuth = true;
                    _User = User;
                    _Password = Password;

                    CreateContext(ContextType.Domain, User, Password);

                    
                }
                else
                {
                    ldapConnection = new DirectoryEntry(LDAP_Connection);
                    IsCreatedService = true;

                    IsUserAuth = false;
                    _User = "";
                    _Password = "";

                    CreateContext(ContextType.Domain);
                }

                ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
            }
            catch (Exception ex)
            {
                IsCreatedService = false;
            }

            return ldapConnection.Properties.Count > 0 && IsCreatedService;
        }

        public bool UpdateUserProperty(User user, string PropertyName, string NewPropertyValue)
        {
            //Success:true | PropertyName:Description | OldValue:CAU3909 | NewValue:CAU3909
            bool IsSuccess = false;

            using (DirectoryEntry de = ldapConnection)
            {
                DirectorySearcher search = new DirectorySearcher(de);
                search.Filter = $"(sAMAccountName={user.LogonUser})";
                search.PropertiesToLoad.Add(PropertyName);

                SearchResult result = search.FindOne();

                if (result != null)
                {
                    DirectoryEntry entryToUpdate = result.GetDirectoryEntry();
                    if (!(String.IsNullOrEmpty(PropertyName)))
                    {   

                        if (result.Properties.Contains("" + PropertyName + ""))
                        {
                            entryToUpdate.Properties["" + PropertyName + ""].Value = NewPropertyValue;
                        }

                        try
                        {
                            entryToUpdate.CommitChanges();
                            IsSuccess = true;
                        }
                        catch(Exception ex) { IsSuccess = false; }

                    }
                    else { IsSuccess = false; }
                }
                else { IsSuccess = false; }

                search.Dispose();

            }
            return IsSuccess;
        }

        public bool MoveUserOU(User user, string NewouPath)
        {
            bool isSuccess = false;

            using (DirectoryEntry de = ldapConnection)
            {
                DirectorySearcher search = new DirectorySearcher(de);
                search.Filter = $"(sAMAccountName={user.LogonUser})";
                search.PropertiesToLoad.Add("sAMAccountName");

                SearchResult result = search.FindOne();

                if (result != null)
                {

                    try
                    {
                        DirectoryEntry entryToUpdate = result.GetDirectoryEntry();
                        DirectoryEntry newDirectory = new DirectoryEntry(NewouPath);

                        entryToUpdate.MoveTo(newDirectory);
                        entryToUpdate.CommitChanges();
                        entryToUpdate.Close();
                        newDirectory.Close();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }
                }
            }

            return isSuccess;
        }

        //Created Methods
        public User CreateUser(string LogonUser, string FirstName, string LastName, string Mail,
            string CompanyName, string CountryCode, string Department, string Manager, bool IsAccountActive, List<Group> Groups)
        {
            User user = null;
            string domain = LDAP.Replace("LDAP://", "");

            using (var pc = contexto)
            {
                using (var up = new UserPrincipal(pc))
                {
                    up.SamAccountName = LogonUser;
                    up.EmailAddress = Mail;
                    up.DisplayName = $"{FirstName} {LastName}";
                    up.GivenName = FirstName;
                    up.Surname = LastName;

                    up.PasswordNeverExpires = false;
                    up.PasswordNotRequired = false;
                    up.UserCannotChangePassword = false;

                }
            }


            return user;
        }


        //Private Actions
        private void GetDomainConnection(string domainCnn)
        {
            string cnn = "LDAP://";

            string[] dc_Collection = domainCnn.Split('.');

            foreach (var item in dc_Collection)
            {
                string s = $"DC={item},";

                cnn = $"{cnn}{s}";
            }

            LargeLDAP = cnn.Substring(0, cnn.Length - 1);
            //return LargeLDAP;
        }
        private void CreateContext(ContextType contextType, string User = "", string Password = "")
        {
            string domain = LDAP.Replace("LDAP://", "");
            try
            {

                if (User.Length > 0 && Password.Length > 0)
                {
                    contexto = new PrincipalContext(contextType, domain, User, Password);
                }
                else
                {
                    contexto = new PrincipalContext(contextType, domain);
                }
            }
            catch (Exception ex) { }

        }

        private string GetProperty(SearchResult searchResult,string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

     
    }

    public static class Extensions
    {
        /// <summary>
        /// Inactivate an user in active directory
        /// </summary>
        /// <param name="user"></param>
        public static void Inactivate(this User user)
        {
            bool IsUserActive = true;

            if (user.IsAccountActive)
            {
                try
                {
                    PrincipalContext principalContext = user.DomainContext; //new PrincipalContext(ContextType.Domain);
                    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity
                            (principalContext, user.LogonUser);
                    userPrincipal.Enabled = false;
                    userPrincipal.Save();

                    if (userPrincipal.Enabled == false)
                    {
                        user.IsAccountActive = false;
                        IsUserActive = user.IsAccountActive;
                    }
                }
                catch (Exception ex)
                {
                    IsUserActive = true;
                }
            }
            else { IsUserActive = false; }

            //return IsUserActive;
        }

        /// <summary>
        /// Activate an user in active directory
        /// </summary>
        /// <param name="user"></param>
        public static void Activate(this User user)
        {
            bool IsUserActive = false;

            if (user.IsAccountActive == false)
            {
                try
                {
                    PrincipalContext principalContext = user.DomainContext; //new PrincipalContext(ContextType.Domain);
                    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity
                            (principalContext, user.LogonUser);
                    userPrincipal.Enabled = true;
                    userPrincipal.Save();

                    if (userPrincipal.Enabled == true)
                    {
                        user.IsAccountActive = true;
                        IsUserActive = user.IsAccountActive;
                    }
                }
                catch (Exception ex)
                {
                    IsUserActive = false;
                }
            }
            else { IsUserActive = true; }

            //return IsUserActive;
        }

        public static void ResetPassword(this User userLogon,string newPassword)
        {
            if (userLogon != null)
            {
                using (var context = userLogon.DomainContext) //new PrincipalContext(ContextType.Domain)
                {
                    using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userLogon.LogonUser))
                    {
                        user.SetPassword(newPassword);
                        // or
                        //user.ChangePassword("", "newpassword");

                        user.Save();
                    }
                }
            }
        }

        public static void ResetPassword(this User userLogon,string oldPassword,string newPassword)
        {
            if (userLogon != null)
            {
                using (var context = userLogon.DomainContext) //new PrincipalContext(ContextType.Domain)
                {
                    using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userLogon.LogonUser))
                    {
                        //user.SetPassword(newPassword);
                        // or
                        user.ChangePassword(oldPassword, newPassword);

                        user.Save();
                    }
                }
            }
        }

        public static void Unlock(this User user)
        {

            //string domain = LDAP.Replace("LDAP://", "");

            // set up domain context
            PrincipalContext ctx = user.DomainContext; //new PrincipalContext(ContextType.Domain, domain, _User, _Password);

            // find a user
            UserPrincipal userp = UserPrincipal.FindByIdentity(ctx, user.LogonUser);

            if (user != null)
            {
                // unlock user
                userp.UnlockAccount();
            }


        }
    }

}







/*
 *   public static bool Inactivate(this User user)
        {
            bool IsUserActive = true;
            string filter = $"(sAMAccountName={user.LogonUser})";

            if (user.IsAccountActive)
            {
                try
                {
                    DirectoryEntry _user = new DirectoryEntry(user.LogonUser);
                    int val = (int)_user.Properties["userAccountControl"].Value;
                    _user.Properties["userAccountControl"].Value = val | 0x2;
                    //ADS_UF_ACCOUNTDISABLE;

                    _user.CommitChanges();
                    _user.Close();

                    //Revalidate User Status
                    val = (int)_user.Properties["userAccountControl"].Value;
                    bool temp_status = !Convert.ToBoolean(val & 0x0002);

                    user.IsAccountActive = temp_status;
                    IsUserActive = temp_status;
                }
                catch (System.DirectoryServices.DirectoryServicesCOMException E)
                {
                    //DoSomethingWith --> E.Message.ToString();

                }
            }
            else { IsUserActive = false; }

            return IsUserActive;
        }
 * 
 * 
 * 
 * 
 */




/*
   public static List<User> GetUserList(bool IsGetOnlyActiveUsers)
        {
            List<User> users = new List<User>();
            List<Group> Groups = new List<Group>();

            using (DirectoryEntry searchRoot = ldapConnection)
            {
                using (DirectorySearcher directorySearcher = new DirectorySearcher(searchRoot))
                {
                    directorySearcher.Filter = "(&(objectCategory=person)(objectClass=user))"; //Filter only users object

                    directorySearcher.PropertiesToLoad.Add("displayname"); //Full Name
                    directorySearcher.PropertiesToLoad.Add("givenname"); //First Name
                    directorySearcher.PropertiesToLoad.Add("sn"); //Last Name
                    directorySearcher.PropertiesToLoad.Add("userPrincipalName"); //User Mail
                    directorySearcher.PropertiesToLoad.Add("department"); //User Department
                    directorySearcher.PropertiesToLoad.Add("manager"); //User Manager
                    directorySearcher.PropertiesToLoad.Add("samaccountname"); //User Logon
                    directorySearcher.PropertiesToLoad.Add("accountexpires"); //User Expired
                    directorySearcher.PropertiesToLoad.Add("company"); //User Company Name
                    directorySearcher.PropertiesToLoad.Add("countrycode"); //User Company Name
                    directorySearcher.PropertiesToLoad.Add("userAccountControl"); //Account Active
                    directorySearcher.PropertiesToLoad.Add("memberof"); //Account Member Of

                    using (SearchResultCollection searchResultCollection = directorySearcher.FindAll())
                    {

                        foreach (SearchResult searchResult in searchResultCollection)
                        {
                            // Create new AD User instance
                            var user = new User();

                            bool IsUserEnabled = false;

                            if (searchResult.Properties.Contains("userAccountControl"))
                            {
                                int value = (int.Parse(searchResult.Properties["userAccountControl"][0].ToString()));
                                IsUserEnabled = !Convert.ToBoolean(value & 0x0002);
                                user.IsAccountActive = IsUserEnabled;
                            }

                            if (IsGetOnlyActiveUsers == IsUserEnabled) //If diferent with the criteria continue to gain performance.
                            {
                                user.IsAccountActive = IsUserEnabled;
                            }
                            else
                            {
                                continue;
                            }

                            //Setting Properties if exist
                            if (searchResult.Properties["displayname"].Count > 0)
                            {
                                user.FullName = searchResult.Properties["displayname"][0].ToString();
                            }

                            if (searchResult.Properties["givenname"].Count > 0)
                            {
                                user.FirstName = searchResult.Properties["givenname"][0].ToString();
                            }

                            if (searchResult.Properties["sn"].Count > 0)
                            {
                                user.LastName = searchResult.Properties["sn"][0].ToString();
                            }

                            if (searchResult.Properties["userPrincipalName"].Count > 0)
                            {
                                user.Mail = searchResult.Properties["userPrincipalName"][0].ToString();
                            }

                            if (searchResult.Properties["department"].Count > 0)
                            {
                                user.Department = searchResult.Properties["department"][0].ToString();
                            }

                            if (searchResult.Properties["manager"].Count > 0)
                            {
                                string Usermanager = searchResult.Properties["manager"][0].ToString();
                                Usermanager = Usermanager.Substring(0, Usermanager.ToString().IndexOf(",")).Replace("CN=", "");
                                user.Manager = Usermanager;
                            }

                            if (searchResult.Properties["samaccountname"].Count > 0)
                            {
                                user.LogonUser = searchResult.Properties["samaccountname"][0].ToString();
                            }

                            if (searchResult.Properties["accountexpires"].Count > 0)
                            {
                                user.AccountExpires = searchResult.Properties["accountexpires"][0].ToString();
                            }

                            if (searchResult.Properties["company"].Count > 0)
                            {
                                user.CompanyName = searchResult.Properties["company"][0].ToString();
                            }

                            if (searchResult.Properties["countrycode"].Count > 0)
                            {
                                user.CountryCode = searchResult.Properties["countrycode"][0].ToString();
                            }

                            Groups.Clear();
                            if (searchResult.Properties["memberof"].Count > 0)
                            {
                                directorySearcher.Filter = $"(sAMAccountName={user.LogonUser})";
                                SearchResult result = directorySearcher.FindOne();

                                foreach (var item in result.Properties["memberof"]) //searchResult.Properties["memberof"]
                                {
                                    string grupo = item.ToString();
                                    grupo = grupo.Substring(0, item.ToString().IndexOf(",")).Replace("CN=", "");
                                    Groups.Add(new Group { GroupName = grupo }); 
                                }

                                user.Groups = Groups;  
                            }

                            //Add if match with criteria
                            if (user.IsAccountActive == IsGetOnlyActiveUsers)
                            {
                                users.Add(user);
                            }

                        }
                    }
                }
            }

            return users;
        }
 */




/*
    public static void AddUserToGroup(string userLogon, string groupName)
    {
        try
        {
            DirectoryEntry dirEntry = new DirectoryEntry(ldapConnection);
            dirEntry.Properties["member"].Add(userLogon);
            dirEntry.CommitChanges();
            dirEntry.Close();
        }
        catch (System.DirectoryServices.DirectoryServicesCOMException E)
        {
            //doSomething with E.Message.ToString();

        }
    }
 */
