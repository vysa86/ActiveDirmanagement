using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectorySearch
{
    internal sealed class ConfigConstants
    {
        internal const string SECTION_AD = "ActiveDirectory";
        // Common COLUMN NAME
       internal const string AD_DISTICT_NAME = "distinguishedName";
       internal const string AD_CN = "cn";
       internal const string AD_NAME = "name";
        // COLUMN NAME of USER in Active Directory
        internal const string AD_USER_UPN = "userPrincipalName";
        internal const string AD_USER_TITLE = "title";
        internal const string AD_USER_MANAGER = "manager";
        internal const string AD_USER_DEPT = "department";
        internal const string AD_USER_MAIL = "mail";
        internal const string AD_USER_ACCOUNT = "sAMAccountName";
        internal const string AD_USER_LAST_NAME = "sn";
        internal const string AD_USER_FIRST_NAME = "givenName";
        internal const string AD_USER_DISPLAY_NAME = "displayname";
        internal const string AD_USER_OBJECTSID = "objectsid";

        internal const string AD_USERNAME = "username";
        internal const string AD_PASSWORD = "password";
        internal const string AGILEPOINT_APP_NAME = "My Appplication";
    }
}
