using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectorySearch
{
    internal class ActiveDirectoryHelper
    {
        string m_LDAP;

        string[] properties = new string[]
        {
            ConfigConstants.AD_USER_UPN ,
            ConfigConstants.AD_USER_TITLE ,
            ConfigConstants.AD_USER_MANAGER ,
            ConfigConstants.AD_USER_DEPT ,
            ConfigConstants.AD_USER_MAIL ,
            ConfigConstants.AD_USER_ACCOUNT ,
            ConfigConstants.AD_NAME ,
            ConfigConstants.AD_USER_LAST_NAME,
            ConfigConstants.AD_USER_FIRST_NAME ,
            ConfigConstants.AD_DISTICT_NAME,
            ConfigConstants.AD_USER_DISPLAY_NAME,
            ConfigConstants.AD_USER_OBJECTSID
        };
        public ActiveDirectoryHelper(string ldap)
        {
            m_LDAP = ldap;

        }
        public List<NXUserProfile> GetADUserProfiles(string emailAddress)
        {
            List<NXUserProfile> UserProfilesList = new List<NXUserProfile>();
            try
            {
                DirectoryEntry de = new DirectoryEntry(this.m_LDAP, ADConfig.AgilePointUsername, ADConfig.AgilePointPassword);
                string filter = string.Format("(&(objectCategory=Person)(objectClass=user)({0}={1}))", ConfigConstants.AD_USER_MAIL, emailAddress);
                DirectorySearcher ds = new DirectorySearcher(de, filter, properties, SearchScope.Subtree);
                ds.Sort = new SortOption(ConfigConstants.AD_USER_DISPLAY_NAME, SortDirection.Ascending);
                ds.PageSize = 1;
                ds.ClientTimeout = new TimeSpan(-1);
                SearchResultCollection collection = ds.FindAll();
                if (collection == null)
                {

                    return UserProfilesList;
                }
                NXUserProfile UserProfile = null;
                foreach (SearchResult sr in collection)
                {

                    UserProfile = new NXUserProfile();
                    DirectoryEntry directoryEntry = new DirectoryEntry(sr.Path, ADConfig.AgilePointUsername, ADConfig.AgilePointPassword);

                    string manager = directoryEntry?.Properties[ConfigConstants.AD_USER_MANAGER].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(manager))
                    {
                        DirectoryEntry managerDE = new DirectoryEntry("LDAP://" + manager, ADConfig.AgilePointUsername, ADConfig.AgilePointPassword);
                        UserProfile.Manager = GetUserNameWithDomain(managerDE);

                    }

                    string department = directoryEntry?.Properties[ConfigConstants.AD_USER_DEPT].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(department))
                    {
                        UserProfile.Department = department;
                    }
                    string userEmailAddress = directoryEntry?.Properties[ConfigConstants.AD_USER_MAIL].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(userEmailAddress))
                    {
                        UserProfile.EMailAddress = userEmailAddress;
                    }
                    string fullName = directoryEntry?.Properties[ConfigConstants.AD_USER_DISPLAY_NAME].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(fullName))
                    {
                        UserProfile.FullName = fullName;
                    }
                    object objSID = directoryEntry?.Properties[ConfigConstants.AD_USER_OBJECTSID].Value;
                    if (objSID != null)
                    {
                        UserProfile.Manager = GetUserNameWithDomain(directoryEntry);
                    }



                    //var directoryEntry = sr.GetDirectoryEntry(sr.Path);

                    // UserProfile.UserName = $"{domain}\\{ sr?.Properties[ConfigConstants.AD_USER_ACCOUNT].Value as String}";
                    //string user = directoryEntry.Properties["SAMAccountName"][0].ToString();
                    //string domain = directoryEntry.Path.ToString().Split(new[] { ",DC=" }, StringSplitOptions.None)[1];
                    //UserProfile = new NXUserProfile();
                    //string Manager= sr?.Properties[ConfigConstants.AD_USER_MANAGER][0] as String;
                    //UserProfile.Manager= $"{domain}\\{Manager.Split(new[] { "CN=" }, StringSplitOptions.None)[1].TrimEnd(',')}";
                    //UserProfile.Department= sr?.Properties[ConfigConstants.AD_USER_DEPT][0] as String;
                    //UserProfile.EMailAddress= sr?.Properties[ConfigConstants.AD_USER_MAIL][0] as String;
                    //UserProfile.FullName= sr?.Properties[ConfigConstants.AD_USER_DISPLAY_NAME][0] as String;
                    //UserProfile.Title= sr?.Properties[ConfigConstants.AD_USER_DISPLAY_NAME][0] as String;
                    //UserProfile.UserName = $"{domain}\\{ sr?.Properties[ConfigConstants.AD_USER_ACCOUNT][0] as String}";
                    UserProfilesList.Add(UserProfile);
                }

            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
            }

            return UserProfilesList;
        }

        private string GetUserNameWithDomain(DirectoryEntry directoryEntry)
        {
            string UserName = string.Empty;
            try
            {
                byte[] objectsid = (byte[])directoryEntry?.Properties[ConfigConstants.AD_USER_OBJECTSID].Value;
                SecurityIdentifier sid = new SecurityIdentifier(objectsid, 0);
                NTAccount account = (NTAccount)sid.Translate(typeof(NTAccount));
                UserName = account.ToString();
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
            }

            return UserName;
        }

        private string GetUserName(string logonName)
        {
            string UserName = string.Empty;
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal userObj = UserPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, logonName);
            if (userObj != null)
            {
                NTAccount account = (NTAccount)userObj.Sid.Translate(typeof(NTAccount));
                UserName = account.ToString();
            }
            return UserName;
        }
    }
}
