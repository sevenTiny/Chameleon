using Chameleon.Entity;
using Chameleon.Infrastructure;
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
        Result DeleteNode(Guid nodeId);
    }

    public class OrganizationService : CommonServiceBase<Organization>, IOrganizationService
    {
        IOrganizationRepository _organizationRepository;
        public OrganizationService(IOrganizationRepository organizationRepository) : base(organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public List<Organization> GetTree()
        {
            //获取所有子节点
            var nodes = _organizationRepository.GetListUnDeleted();

            //获取顶级节点
            Organization condition = nodes?.FirstOrDefault(t => t.ParentId == Guid.Empty);

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
                if (chooseNodeId != Guid.Empty)
                {
                    //判断是否有树存在
                    List<Organization> conditionListExist = _organizationRepository.GetListUnDeleted();

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

        public Result DeleteNode(Guid nodeId)
        {
            if (nodeId == Guid.Empty)
                return Result.Success();

            List<Organization> conditionListExist = _organizationRepository.GetListUnDeleted();

            if (conditionListExist != null)
            {
                var needToDelete = conditionListExist.Where(t => t.ParentId == nodeId).ToList();

                foreach (var item in needToDelete)
                {
                    _organizationRepository.LogicDelete(item.Id);
                }

                _organizationRepository.LogicDelete(nodeId);
            }

            return Result.Success();
        }
    }
}
