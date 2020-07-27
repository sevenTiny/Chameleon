using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Infrastructure.Consts;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Security;
using SevenTiny.Cloud.ScriptEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface IOrganizationService : ICommonServiceBase<Organization>
    {
        List<Organization> GetTree();
        Result AddNode(RelationEnum relation, Guid chooseNodeId, Organization entity);
        /// <summary>
        /// 调整位置
        /// </summary>
        /// <param name="relation"></param>
        /// <param name="chooseNodeId"></param>
        /// <param name="ajustNodeId"></param>
        /// <returns></returns>
        Result AjustRelation(RelationEnum relation, Guid chooseNodeId, Guid ajustNodeId);
        /// <summary>
        /// 启用节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Result EnableNode(Guid nodeId);
        /// <summary>
        /// 禁用节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Result DisableNode(Guid nodeId);
        /// <summary>
        /// 获取有权限的所有组织id
        /// </summary>
        /// <param name="userOrganization"></param>
        /// <returns></returns>
        List<string> GetSubordinatOrganizations(Guid userOrganization);
        /// <summary>
        /// 获取有权限的所有组织id
        /// </summary>
        /// <param name="userOrganizations"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        List<string> GetSubordinatOrganizations(IEnumerable<Guid> userOrganizations, int level);
        /// <summary>
        /// 获取名称展示类似树状层级的列表（用于下拉列表）
        /// </summary>
        /// <returns></returns>
        List<Organization> GetTreeNameList();
    }

    public class OrganizationService : CommonServiceBase<Organization>, IOrganizationService
    {
        IOrganizationRepository _organizationRepository;
        public OrganizationService(IOrganizationRepository organizationRepository) : base(organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public List<Organization> GetTreeNameList()
        {
            // 获取所有子节点
            var nodes = _organizationRepository.GetEnableList();

            //获取顶级节点
            Organization condition = nodes?.FirstOrDefault(t => t.ParentId == AccountConst.TopOrganization);

            if (condition == null)
                return new List<Organization>(0);

            //返回值
            var result = new List<Organization>() { condition };

            //深度
            int dep = 0;

            //递归构造子节点
            GetTree(nodes, condition.Id, condition.Name);

            return result;

            //Tree Search
            void GetTree(List<Organization> source, Guid parentId, string parentName)
            {
                dep++;

                var childs = source.Where(t => t.ParentId == parentId).ToList();

                if (childs == null)
                    return;

                foreach (var item in childs)
                {
                    item.Name = string.Concat(parentName, " - ", item.Name);
                    result.Add(item);

                    GetTree(source, item.Id, item.Name);
                }

                return;
            }
        }

        public List<Organization> GetTree()
        {
            //获取所有子节点
            var nodes = _organizationRepository.GetEnableList();

            //获取顶级节点
            Organization condition = nodes?.FirstOrDefault(t => t.ParentId == AccountConst.TopOrganization);

            if (condition == null)
                return new List<Organization>(0);

            //递归构造子节点
            condition.Children = GetTree(nodes, condition.Id);

            return new List<Organization> { condition };

            //Tree Search
            List<Organization> GetTree(List<Organization> source, Guid parentId)
            {
                var childs = source.Where(t => t.ParentId == parentId).ToList();

                if (childs == null)
                    return new List<Organization>(0);

                childs.ForEach(t => t.Children = GetTree(source, t.Id));

                return childs;
            }
        }

        public Result AddNode(RelationEnum relation, Guid chooseNodeId, Organization entity)
        {
            return TransactionHelper.Transaction(() =>
            {
                Organization newNode = new Organization
                {
                    Id = Guid.NewGuid(),
                    ParentId = chooseNodeId,
                    Name = entity.Name,
                    CreateBy = entity.CreateBy,
                    CreateTime = DateTime.Now,
                    ModifyBy = entity.CreateBy,
                    ModifyTime = DateTime.Now
                };

                //如果兄弟节点!=空，说明当前树有值。反之，则构建新树
                if (chooseNodeId != AccountConst.TopOrganization && chooseNodeId != Guid.Empty)
                {
                    //判断是否有树存在
                    List<Organization> conditionListExist = _organizationRepository.GetEnableList();

                    //获取选择的节点
                    Organization chooseNode = conditionListExist.FirstOrDefault(t => t.Id == chooseNodeId);

                    //根据选择的方式处理新节点
                    switch (relation)
                    {
                        case RelationEnum.Parent:
                            newNode.ParentId = chooseNode.ParentId;
                            chooseNode.ParentId = newNode.Id;
                            _organizationRepository.Update(chooseNode);
                            break;
                        case RelationEnum.Child:
                            newNode.ParentId = chooseNode.Id;
                            break;
                        default:
                            return Result.Error("关系错误，无法新增组织");
                    }
                }

                newNode.Code = newNode.Id.ToString();

                _organizationRepository.Add(newNode);

                return Result.Success("保存成功！");
            });
        }

        public List<string> GetSubordinatOrganizations(Guid userOrganization)
        {
            return GetSubordinatOrganizations(new Guid[] { userOrganization }, int.MaxValue);
        }

        public Result EnableNode(Guid nodeId)
        {
            if (nodeId == Guid.Empty)
                return Result.Success();

            return base.UpdateWithId(nodeId, t =>
            {
                t.Disable = 0;
            });
        }

        public Result DisableNode(Guid nodeId)
        {
            if (nodeId == Guid.Empty)
                return Result.Success();

            return base.UpdateWithId(nodeId, t =>
            {
                t.Disable = 1;
            });
        }

        public Result AjustRelation(RelationEnum relation, Guid chooseNodeId, Guid ajustNodeId)
        {
            if (chooseNodeId == Guid.Empty || ajustNodeId == Guid.Empty)
                return Result.Success();

            if (chooseNodeId == ajustNodeId)
                return Result.Error("关系错误，无法调整组织");

            //判断是否有树存在
            List<Organization> conditionListExist = _organizationRepository.GetEnableList();

            if (conditionListExist == null || !conditionListExist.Any())
                return Result.Success();

            if (!conditionListExist.Any(t => t.Id == chooseNodeId) || !conditionListExist.Any(t => t.Id == ajustNodeId))
                return Result.Success("待调整节点未找到，无法继续");

            return TransactionHelper.Transaction(() =>
            {
                //获取选择的节点
                Organization chooseNode = conditionListExist.Find(t => t.Id == chooseNodeId);
                //待调整节点
                Organization ajustNode = conditionListExist.Find(t => t.Id == ajustNodeId);

                //根据选择的方式处理新节点
                switch (relation)
                {
                    case RelationEnum.Parent:
                        ajustNode.ParentId = chooseNode.ParentId;
                        chooseNode.ParentId = ajustNode.Id;
                        _organizationRepository.Update(chooseNode);
                        break;
                    case RelationEnum.Child:
                        if (chooseNode.ParentId == ajustNode.Id)
                            return Result.Error("关系错误，无法调整组织");
                        ajustNode.ParentId = chooseNode.Id;
                        break;
                    default:
                        return Result.Error("关系错误，无法调整组织");
                }

                _organizationRepository.Update(ajustNode);

                return Result.Success("保存成功！");
            });
        }
        public List<string> GetSubordinatOrganizations(IEnumerable<Guid> userOrganizations, int level)
        {
            if (userOrganizations == null || !userOrganizations.Any())
                return new List<string> { Guid.Empty.ToString() };

            //结果
            var result = new List<string>();

            //获取所有节点
            var nodes = _organizationRepository.GetEnableList();

            if (nodes == null || !nodes.Any())
                return result;

            result.Add(Guid.Empty.ToString());//默认组织，任何人都有此组织权限

            foreach (var item in userOrganizations ?? Enumerable.Empty<Guid>())
            {
                result.Add(item.ToString());
                result.AddRange(GetTree(nodes, item, level));
            }

            return result.Distinct().ToList();

            List<string> GetTree(List<Organization> source, Guid parentId, int innerLevel)
            {
                var childs = source.Where(t => t.ParentId == parentId).ToList();

                if (childs == null || !childs.Any() || innerLevel <= 0)
                    return new List<string>(0);

                innerLevel--;

                childs.ForEach(t => result.AddRange(GetTree(source, t.Id, innerLevel)));

                return childs.Select(t => t.Id.ToString()).ToList();
            }
        }
    }
}
