using NYCJobsWeb.Data.Context;
using NYCJobsWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace NYCJobsWeb.Client
{
    public class UserClient
    {
        private readonly ISearchContext _searchContext;
        public UserClient()
        {
            _searchContext = new SearchContext();
        }
        public Result SaveUserDetails(User userViewModel)
        {            
            var result = new Result();
            if (userViewModel.Id == 0)
            {
                if (_searchContext.Users.Any(t => t.UserName.Equals(userViewModel.UserName)))
                {
                    result.IsSuccess = false;
                    result.Message = "User Name already exists";
                }
                else
                {
                    userViewModel.UserName = userViewModel.UserName.Trim();
                    userViewModel.Password = userViewModel.Password.Trim();
                    userViewModel.RoleId = userViewModel.RoleId;
                    userViewModel.FolderName = userViewModel.FolderName.Trim();
                    using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                    {
                        try
                        {
                            var user = new Data.Entities.User()
                            {
                                UserName = userViewModel.UserName,
                                Password = Common.SecurityUtilities.EncryptUrl(userViewModel.Password, Common.Settings.EncSecretkey),
                                FolderName = userViewModel.FolderName
                            };
                            _searchContext.Users.Add(user);
                            _searchContext.SaveChanges();

                            var userRole = new Data.Entities.UserRole()
                            {
                                UserId = user.Id,
                                RoleId = userViewModel.RoleId
                            };
                            _searchContext.UserRoles.Add(userRole);

                            _searchContext.SaveChanges();
                            scope.Complete();
                            userViewModel.Id = user.Id;
                            userViewModel.RoleId = userRole.RoleId;
                            result.IsSuccess = true;
                            result.Message = "User saved successfully";
                        }
                        catch (Exception ex)
                        {
                            result.IsSuccess = false;
                            scope.Dispose();
                            result.Message = string.IsNullOrEmpty(result.Message) ? ex.Message : "Error while saving user details";
                        }
                    }
                }
            }
            else
            {
                var user = _searchContext.Users.FirstOrDefault(_ => _.Id == userViewModel.Id);
                if (user != null)
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                    {
                        try
                        {
                            var userRole = _searchContext.UserRoles.FirstOrDefault(_ => _.UserId == user.Id);
                            userRole.RoleId = userViewModel?.RoleId ?? 0; 

                            user.UserName = userViewModel.UserName;
                            user.Id = userViewModel.Id;
                            user.FolderName = userViewModel.FolderName;
                            user.Password = userViewModel.Password;

                            _searchContext.SaveChanges();
                            scope.Complete();
                            result.IsSuccess = true;
                            result.Message = "User saved successfully";
                        }
                        catch (Exception ex)
                        {
                            result.IsSuccess = false;
                            scope.Dispose();
                            result.Message = string.IsNullOrEmpty(result.Message) ? ex.Message : "Error while saving user details";
                        }
                    }
                }
            }
            return result;
        }

        public List<User> GetUserList(long[] totalRecordsFound, long userId)
        {            
            var userDetailList = new List<User>();
            try
            {
                var userDetailsQuery = (from user in _searchContext.Users
                                        join dp in _searchContext.UserRoles on user.Id equals dp.UserId
                                        join role in _searchContext.Roles on dp.RoleId equals role.Id
                                        where user.Id != userId
                                        select new User()
                                        {
                                            Id = user.Id,
                                            UserName = user.UserName,
                                            RoleName = role.Name,
                                            RoleId=role.Id,
                                            FolderName = user.FolderName,
                                            Password=user.Password
                                        }).ToList();
                
                if (userDetailsQuery.Any())
                {
                    var total = userDetailsQuery.Count;
                    totalRecordsFound[0] = total;
                    userDetailList = userDetailsQuery.ToList();
                }
                return userDetailList;
            }
            catch (Exception exception)
            {                
            }
            return null;
        }

        public User GetUserDetails(long userId)
        {
            var userdetails = new User();
            try
            {  
                var user = _searchContext.Users.FirstOrDefault(_ => _.Id== userId);
                if (user != null && user.Id > 0)
                {
                    var userRole = _searchContext.UserRoles.FirstOrDefault(_ => _.UserId == user.Id);
                    userdetails.RoleId = userRole?.RoleId ?? 0;
                    userdetails.UserName = user.UserName;
                    userdetails.Id = user.Id;
                    userdetails.RoleName = userRole.Role.Name;
                    userdetails.FolderName = user.FolderName;
                    userdetails.Password = user.Password;
                }
                return userdetails;
            }
            catch (Exception exception)
            {
            }
            return userdetails;
        }

    }
}