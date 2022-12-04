namespace StockPOS.Models
{
    public static class Extensions
    {
        public static UserDTO AsDTO(this User item)
        {
            return new UserDTO
            (
                item.UserId,
                item.FullName,
                item.UserName,
                item.DateofBirth,
                item.Gender,
                item.Address,
                item.Phone,
                item.Email,
                item.UserTypeId
            );
        }
    }
}