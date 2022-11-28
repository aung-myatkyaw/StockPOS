namespace StockPOS.Models
{
    public static class Extensions
    {
        public static CashierDTO AsDTO(this Cashier item)
        {
            return new CashierDTO
            (
                item.Id,
                item.FullName,
                item.UserName,
                item.DateofBirth,
                item.Gender,
                item.Address,
                item.Phone,
                item.Email
            );
        }
    }
}