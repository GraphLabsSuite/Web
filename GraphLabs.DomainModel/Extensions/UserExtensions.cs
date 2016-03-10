namespace GraphLabs.DomainModel.Extensions
{
    /// <summary> ���������� ��� ������������� </summary>
    public static class UserExtensions
    {
        /// <summary> ��������� ��� ������������ � ������� ������� �.�. </summary>
        public static string GetShortName(this User user)
        {
            return user.FatherName != null 
                ? string.Format("{0} {1}.{2}.", user.Surname, user.Name[0], user.FatherName[0])
                : string.Format("{0} {1}.", user.Surname, user.Name[0]);
        }

    }
}
