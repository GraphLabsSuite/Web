using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    public static class UserGroupChecking
    {
        public static bool UserNotLogged(this Controller controller)
        {
            if (controller.Session["UserID"] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UserIsAdmin(this Controller controller)
        {
            if (UserNotLogged(controller))
            {
                return false;
            }
            if ((int)controller.Session["GroupID"] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UserIsTeacher(this Controller controller)
        {
            if (UserNotLogged(controller))
            {
                return false;
            }
            if ((int)controller.Session["GroupID"] == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UserIsStudent(this Controller controller)
        {
            if (UserNotLogged(controller))
            {
                return false;
            }
            if ((int)controller.Session["GroupID"] > 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UserIsAdminOrTeacher(this Controller controller)
        {
            return (UserIsAdmin(controller)||UserIsTeacher(controller));
        }
    }
}