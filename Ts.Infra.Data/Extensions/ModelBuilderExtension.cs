using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ts.Common.Constant.AppConstants;
using Ts.Common.Constant.AppConstants.PolicyConstants;
using Ts.Common.Constant.SiteConstants;
using Ts.Domain.Models;
namespace Ts.Infra.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder builder)
        {
            var applicationUser = new ApplicationUser
            {
                Id = "214eccb4-402e-4004-807b-80e011db75d2",
                FirstName = "Demo",
                LastName = "D",
                ChangePassword = true,
                IsActive = true,
                CanLogin = true,
                UserName = "DEM20231",
                NormalizedUserName = "DEM20231",
                Email = "demoemailddj@gmail.com",
                NormalizedEmail = "DEMOEMAILDDJ@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAEAACcQAAAAELdnlJJYejLvTetcJEEkiCE7ENyDx6NG+qynlhYqXLvsxON0hriLoPA1mMtc8MUGQw==", // Admin@123
                PhoneNumberConfirmed = false,
                LockoutEnabled = true,
                CreatedOn = Convert.ToDateTime("2022/10/20 12:34:26.4694431"),
                ConcurrencyStamp = "908a4aed-bf84-4558-b3b0-1814fd050858",
                SecurityStamp = "6ded5a7c-bacf-48db-a928-0b5f0dca8aee"
            };
            builder.Entity<ApplicationUser>().HasData(applicationUser);

            var applicationRole = new ApplicationRole
            {
                Id = "3f81e050-9991-4913-bee1-7105a3108de3",
                IsActive = true,
                Name = RoleConstant.Administrator,
                NormalizedName = RoleConstant.Administrator.ToUpper()
            };
            builder.Entity<ApplicationRole>().HasData(applicationRole);
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = "3f81e050-9991-4913-bee1-7105a3108de4",
                    IsActive = true,
                    Name = RoleConstant.Employee,
                    NormalizedName = RoleConstant.Employee.ToUpper()
                },
                new ApplicationRole
                {
                    Id = "3f81e050-9991-4913-bee1-7105a3108de5",
                    IsActive = true,
                    Name = RoleConstant.DotInSiteUser,
                    NormalizedName = RoleConstant.DotInSiteUser.ToUpper()
                },
                new ApplicationRole
                {
                    Id = "3f81e050-9991-4913-bee1-7105a3108de6",
                    IsActive = true,
                    Name = RoleConstant.DotComSiteUser,
                    NormalizedName = RoleConstant.DotComSiteUser.ToUpper()
                },
                new ApplicationRole
                {
                    Id = "3f81e050-9991-4913-bee1-7105a3108de7",
                    IsActive = true,
                    Name = RoleConstant.ShopInUser,
                    NormalizedName = RoleConstant.ShopInUser.ToUpper()
                },
                new ApplicationRole
                {
                    Id = "3f81e050-9991-4913-bee1-7105a3108de8",
                    IsActive = true,
                    Name = RoleConstant.EmployeeShopIn,
                    NormalizedName = RoleConstant.EmployeeShopIn.ToUpper()
                });

            builder.Entity<NextUserSetting>().HasData(new NextUserSetting
            {
                Id = "d95ecbe5-b708-4252-86be-2631fc6634b1",
                NextUserNumber = 2
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = applicationUser.Id,
                RoleId = applicationRole.Id,
            });

            builder.Entity<Gender>().HasData(new List<Gender>
            {
                new() { Id=1, Name = "Male" },
                new() { Id=2, Name = "Female" },
                new() { Id=3, Name = "TransGender" },
                new() { Id=4, Name = "Both" },
                new() { Id=5, Name = "Unknown" }
            });

            builder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = applicationUser.Id,
                    ClaimType = UserPolicy.CanView,
                    ClaimValue = "true"
                },
                new IdentityUserClaim<string>
                {
                    Id = 2,
                    UserId = applicationUser.Id,
                    ClaimType = UserPolicy.CanCreate,
                    ClaimValue = "true"
                },
                new IdentityUserClaim<string>
                {
                    Id = 3,
                    UserId = applicationUser.Id,
                    ClaimType = UserPolicy.CanUpdate,
                    ClaimValue = "true"
                },
                new IdentityUserClaim<string>
                {
                    Id = 4,
                    UserId = applicationUser.Id,
                    ClaimType = UserPolicy.CanUpdateUserPrivilege,
                    ClaimValue = "true"
                },
                new IdentityUserClaim<string>
                {
                    Id = 5,
                    UserId = applicationUser.Id,
                    ClaimType = UserPolicy.CanUpdateUserEmail,
                    ClaimValue = "true"
                }
            );

            builder.Entity<DeliveryPolicy>().HasData(new List<DeliveryPolicy>
            {
                new() {
                    Id=1,
                    DeliveryInDays=15,
                    Name = DeliveryPolicyConstant.Downloadable15Days,
                    Title = "Download will be available for 15 days"
                }
            });

            builder.Entity<ExchangePolicy>().HasData(new List<ExchangePolicy>
            {
                new() { Id=1, ExchangeInDays=0, Name = ExchangePolicyConstant.NoExchange }
            });

            builder.Entity<OrderStatus>().HasData(new List<OrderStatus>
            {
                new() { Id=1, Name = OrderStatusConstant.OrderInitiated },
                new() { Id=2, Name = OrderStatusConstant.OrderCompleted },
                new() { Id=3, Name = OrderStatusConstant.OrderPaymentPending },
                new() { Id=4, Name = OrderStatusConstant.OrderPaymentRefunded },
                new() { Id=5, Name = OrderStatusConstant.OrderPaymentFailed },
                new() { Id=6, Name = OrderStatusConstant.OrderPaymentRefundInProcess },
                new() { Id=7, Name = OrderStatusConstant.OrderPaymentRefundFailed }
            });

            builder.Entity<PaymentMode>().HasData(new List<PaymentMode>
            {
                new() { Id=1, Name = PaymentModeConstant.Online }
            });

            builder.Entity<PaymentGatewayType>().HasData(new List<PaymentGatewayType>
            {
                new() { Id=1, Name = PaymentGatewayTypeConstant.Razorpay }
            });

            builder.Entity<PaymentStatus>().HasData(new List<PaymentStatus>
            {
                new() { Id=1, PaymentGatewayTypeId=1, Name = RazorpayPaymentStatusConstant.Created },
                new() { Id=2, PaymentGatewayTypeId=1, Name = RazorpayPaymentStatusConstant.Authorized },
                new() { Id=3, PaymentGatewayTypeId=1, Name = RazorpayPaymentStatusConstant.Captured },
                new() { Id=4, PaymentGatewayTypeId=1, Name = RazorpayPaymentStatusConstant.Refunded },
                new() { Id=5, PaymentGatewayTypeId=1, Name = RazorpayPaymentStatusConstant.Failed }
            });

            builder.Entity<CurrencyType>().HasData(new List<CurrencyType>
            {
                new() { Id=1, Symbol=CurrencyTypeConstant.INRSymbol, Letter = CurrencyTypeConstant.INR },
            });

            builder.Entity<ReturnPolicy>().HasData(new List<ReturnPolicy>
            {
                new() { Id=1, ReturnInDays=0, Name = ReturnPolicyConstant.NoReturn }
            });
        }
    }
}
