using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using YooPoon.WebFramework.Authentication.Entity;
using YooPoon.WebFramework.Authentication.Services;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.UC;



namespace Zerg.Controllers.UC
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("角色管理类")]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;
        private readonly IControllerActionService _actionService;

        public RoleController(IRoleService roleService, IControllerActionService actionService)
        {
            _roleService = roleService;
            _actionService = actionService;
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns>角色列表</returns>
        [Description("获取角色列表")]
        public HttpResponseMessage GetRoles()
        {
            var list = _roleService.ListRoles().Select(r => new RoleModel
            {
                Id = r.Id,
                Description = r.Description,
                RoleName = r.RoleName,
                Status = r.Status
            }).ToList();
            return PageHelper.toJson(list);
        }
        /// <summary>
        /// 获取角色详细信息
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns>角色详细信息</returns>
        [Description("获取角色详细信息")]
        public HttpResponseMessage GetDetail(int id)
        {
            var role = _roleService.GetRoleById(id);
            return PageHelper.toJson(new RoleModel
            {
                Description = role.Description,
                Id = role.Id,
                RoleName = role.RoleName,
                Status = role.Status
            });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>返回删除结果状态信息</returns>
        [Description("删除角色")]
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            var role = _roleService.GetRoleById(id);
            if (_roleService.DeleteRole(role))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="model">角色参数</param>
        /// <returns>创建结果</returns>
        [Description(" 创建角色")]
        [HttpPost]
        public HttpResponseMessage Create(RoleModel model)
        {
            if (_roleService.CreateRole(new Role
            {
                RoleName = model.RoleName,
                Status = RoleStatus.Normal,
                Description = model.Description
            }) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
        }
        /// <summary>
        /// 编辑角色信息
        /// </summary>
        /// <param name="model">角色参数</param>
        /// <returns>编辑结果状态信息</returns>
        [Description("角色角色")]
        [HttpPost]
        public HttpResponseMessage Edit(RoleModel model)
        {
            var role = _roleService.GetRoleById(model.Id);
            role.RoleName = model.RoleName;
            role.Description = model.Description;
            role.Status = model.Status;
            if (_roleService.ModifyRole(role))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
        }
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色权限</returns>
        [Description("获取角色权限")]
        public HttpResponseMessage GetRolePermission(int roleId)
        {
            var role = _roleService.GetRoleById(roleId);
            var models = CreateControllerModel();
            if (role.RolePermissions != null)
            {
                foreach (var rolePermission in role.RolePermissions)
                {
                    models.First(m => m.ControllerFullName == rolePermission.ControllerAction.ControllerName)
                        .Actions.First(a => a.ActionName == rolePermission.ControllerAction.ActionName)
                        .IsAllowed = rolePermission.IsAllowed;
                }
            }
            return PageHelper.toJson(models);
        }

        private List<ControllerModel> CreateControllerModel()
        {
            return Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.Name == "ApiController")
                .Select(c => new ControllerModel
                {
                    ControllerName = c.Name,
                    ControllerFullName = c.FullName,
                    Description =
                        c.GetCustomAttributes(typeof(DescriptionAttribute), true).Length > 0
                            ? ((DescriptionAttribute)
                                c.GetCustomAttributes(typeof(DescriptionAttribute), true)
                                    .First()).Description
                            : "",
                    Actions =
                        c.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                            .Select(m => new ActionPermissionModel
                            {
                                ActionName = m.Name,
                                Description =
                                    m.GetCustomAttribute<DescriptionAttribute>() == null
                                        ? ""
                                        : m.GetCustomAttribute<DescriptionAttribute>().Description
                            }).ToList()
                }).ToList();
        }

        [HttpPost]
        public HttpResponseMessage SavePermission([FromBody]List<ControllerModel> model, int roleId)
        {
            var role = _roleService.GetRoleById(roleId);
            var rolePermission = new List<RolePermission>();
            foreach (var controller in model)
            {
                foreach (var action in controller.Actions)
                {
                    var actionEntity = _actionService.ExistOrCreate(controller.ControllerFullName, action.ActionName);
                    if (role.RolePermissions == null || role.RolePermissions.Count(rp => rp.ControllerAction == actionEntity) <= 0)
                    {
                        rolePermission.Add(new RolePermission
                        {
                            ControllerAction = actionEntity,
                            IsAllowed = action.IsAllowed,
                            Role = role
                        });
                    }
                    else
                    {
                        role.RolePermissions.First(rp => rp.ControllerAction == actionEntity).IsAllowed = action.IsAllowed;
                    }
                }
            }
            if (role.RolePermissions == null || role.RolePermissions.Count == 0)
            {
                role.RolePermissions = rolePermission;
            }
            else
            {
                var oldRole = role.RolePermissions.ToList();
                oldRole.AddRange(rolePermission);
                role.RolePermissions = oldRole;
            }
            if (_roleService.ModifyRole(role))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加失败！"));
        }
    }
}
