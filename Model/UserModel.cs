using System.Collections.Generic;

namespace ScaleAddon
{
    public class UserModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public List<string> UserRoles = new List<string>();

        public void ClearUser()
        {
            UserID = "";
            UserName = "";
            Fullname = "";

            UserRoles.Clear();
        }
    }
}