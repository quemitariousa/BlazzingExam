﻿using System.ComponentModel.DataAnnotations;
using BlazzingExam.DataLibrary.Entities.User;

namespace BlazzingExam.DataLibrary.Entities.Permissions
{
    public record RolePermission
    {
        public RolePermission()
        {

        }

        public RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        [Key] public int RolePermissionId { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        #region Relations

        public Permission Permission { get; set; }
        public Role Role { get; set; }

        #endregion
    }
}