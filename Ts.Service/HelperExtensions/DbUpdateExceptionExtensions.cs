using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Ts.Service.HelperExtensions
{
    public static class DbUpdateExceptionExtensions
    {
        public static string GetFriendlySqlExceptionMessage(this DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2601:
                        return "The value already exists. Please use a unique value.";
                    case 2628:
                        return "The input is too long for the field.";
                    case 987:
                        return "A duplicate key error occurred.";
                    case 547:
                        return "The operation was violated by a foreign key constraint.";
                    case 515:
                        return "A required column is missing a value.";
                    default:
                        return "A database error occurred. Please try again.";
                }
            }
            return "An unexpected database error occurred.";
        }
    }
}
