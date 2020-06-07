using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface IInterfaceConditionService : IMetaObjectCommonServiceBase<InterfaceCondition>
    {
        /// <summary>
        /// 添加顶级接口字段配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result AddTopInterfaceCondition(InterfaceCondition entity);
        /// <summary>
        /// 获取树形结构的条件节点
        /// </summary>
        /// <param name="interfaceConditionId"></param>
        /// <returns></returns>
        List<InterfaceCondition> GetTree(Guid interfaceConditionId);
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="brotherNodeId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result AddNode(Guid brotherNodeId, InterfaceCondition entity);
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="conditionId"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Result DeleteNode(Guid conditionId, Guid nodeId);
    }

    public class InterfaceConditionService : MetaObjectCommonServiceBase<InterfaceCondition>, IInterfaceConditionService
    {
        IInterfaceConditionRepository _InterfaceConditionRepository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceConditionService(IInterfaceConditionRepository repository, IMetaFieldRepository metaFieldRepository) : base(repository)
        {
            _metaFieldRepository = metaFieldRepository;
            _InterfaceConditionRepository = repository;
        }

        public Result AddTopInterfaceCondition(InterfaceCondition entity)
        {
            entity.BelongToCondition = Guid.Empty;
            entity.ParentId = Guid.Empty;
            entity.MetaFieldId = Guid.Empty;

            return base.Add(entity);
        }

        public Result AddNode(Guid brotherNodeId, InterfaceCondition entity)
        {
            return TransactionHelper.Transaction(() =>
            {
                Guid parentId = brotherNodeId;

                //如果兄弟节点!=空，说明当前树有值。反之，则构建新树
                if (parentId != Guid.Empty)
                {
                    //判断是否有树存在
                    List<InterfaceCondition> conditionListExist = _InterfaceConditionRepository.GetInterfaceConditionByBelongToId(entity.BelongToCondition);

                    //查看当前兄弟节点的父节点id
                    InterfaceCondition brotherCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherNodeId);

                    parentId = brotherCondition.ParentId;

                    //拿到父节点的信息
                    InterfaceCondition parentCondition = conditionListExist.FirstOrDefault(t => t.Id == parentId);

                    //如果父节点的连接条件和当前新建的条件一致，则不需要新建条件节点，直接附加在已有的条件下面
                    if (parentCondition == null || parentCondition.ConditionJointType != entity.ConditionJointType)
                    {
                        //先添加一个父节点，然后把兄弟节点的父节点指向新建的父节点
                        InterfaceCondition newParentCondition = new InterfaceCondition
                        {
                            Id = Guid.NewGuid(),
                            Code = Guid.NewGuid().ToString(),
                            BelongToCondition = entity.BelongToCondition,
                            ParentId = conditionListExist.Count > 0 ? parentId : Guid.Empty,//如果有树，则插入节点的父节点为刚才的兄弟节点的父节点，否则，插入根节点
                            MetaFieldId = Guid.Empty,//连接节点没有field
                            MetaFieldShortCode = "-1",
                            ConditionType = entity.ConditionType,
                            ConditionJointType = entity.ConditionJointType,
                            Name = ((ConditionJointTypeEnum)entity.ConditionJointType).GetDescription(),
                            ConditionValue = "-1",
                            ConditionValueType = -1,
                            ConditionNodeType = (int)NodeTypeEnum.Joint,
                            CloudApplicationtId = entity.CloudApplicationtId,
                            MetaObjectId = entity.MetaObjectId
                        };

                        _InterfaceConditionRepository.Add(newParentCondition);

                        //将兄弟节点的父节点指向新插入的节点
                        brotherCondition.ParentId = newParentCondition.Id;

                        _InterfaceConditionRepository.Update(brotherCondition);

                        //重新赋值parentId
                        parentId = newParentCondition.Id;
                    }
                }

                //检验是否没有条件节点
                //if (parentId == Guid.Empty)
                //{
                //    if (_InterfaceConditionRepository.GetById(parentId) != null)
                //    {
                //        return Result.Error("已经存在条件节点，请查证后操作！");
                //    }
                //}

                //新增节点
                MetaField metaField = _metaFieldRepository.GetById(entity.MetaFieldId);

                InterfaceCondition newCondition = new InterfaceCondition
                {
                    Id = Guid.NewGuid(),
                    Code = Guid.NewGuid().ToString(),
                    BelongToCondition = entity.BelongToCondition,
                    ParentId = parentId,
                    MetaFieldId = entity.MetaFieldId,
                    MetaFieldShortCode = metaField.ShortCode,
                    ConditionType = entity.ConditionType,
                    Name = $"{metaField.ShortCode} {((ConditionTypeEnum)entity.ConditionType).GetDescription()} {entity.ConditionValue}",
                    ConditionValue = entity.ConditionValue,
                    ConditionValueType = entity.ConditionValueType,
                    ConditionNodeType = (int)NodeTypeEnum.Condition,
                    CloudApplicationtId = entity.CloudApplicationtId,
                    MetaObjectId = entity.MetaObjectId
                };

                _InterfaceConditionRepository.Add(newCondition);

                return Result.Success("保存成功！");
            });
        }

        public Result DeleteNode(Guid conditionId, Guid nodeId)
        {
            //将要删除的节点id集合
            var willBeDeleteIds = new List<Guid>();

            List<InterfaceCondition> allConditions = _InterfaceConditionRepository.GetInterfaceConditionByBelongToId(conditionId);
            if (allConditions == null || !allConditions.Any())
            {
                return Result.Success("删除成功！");
            }
            InterfaceCondition condition = allConditions.FirstOrDefault(t => t.Id == nodeId);
            if (condition == null)
            {
                return Result.Success("删除成功！");
            }
            //获取父节点id
            Guid parentId = condition.ParentId;
            //如果是顶级节点，直接删除
            if (parentId == Guid.Empty)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return Result.Success("删除成功！");
            }
            //如果不是顶级节点，查询所有兄弟节点。
            //如果所有兄弟节点（包含自己）多余两个，则直接删除此节点;
            List<InterfaceCondition> conditionList = allConditions.Where(t => t.ParentId == parentId).ToList();
            if (conditionList.Count > 2)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return Result.Success("删除成功！");
            }
            //如果兄弟节点为两个，则将父亲节点删除，将另一个兄弟节点作为父节点。
            else if (conditionList.Count == 2)
            {
                //获取到父节点
                InterfaceCondition parentNode = allConditions.FirstOrDefault(t => t.Id == parentId);
                //找到兄弟节点（同一个父节点，id!=当前节点）
                InterfaceCondition brotherNode = allConditions.FirstOrDefault(t => t.ParentId == parentId && t.Id != nodeId);
                //将兄弟节点的父节点指向父节点的父节点
                brotherNode.ParentId = parentNode.ParentId;
                //更新兄弟节点
                _InterfaceConditionRepository.Update(brotherNode);
                //将父节点删除
                _InterfaceConditionRepository.Delete(parentId);
                //删除要删除的节点以及下级节点
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
            }
            //如果没有兄弟节点，则直接将节点以及父节点都删除（如果数据不出问题，默认不存在此种情况，直接返回结果）
            else
            {
                return Result.Success("删除成功！");
            }

            return Result.Success("删除成功！");

            //删除节点及所有下级节点
            void DeleteNodeAndChildrenNodes(List<InterfaceCondition> sourceConditions, Guid currentNodeId)
            {
                FindDeleteNodeAndChildrenNodes(sourceConditions, currentNodeId);
                _InterfaceConditionRepository.Delete(nodeId);
                foreach (var item in willBeDeleteIds)
                {
                    _InterfaceConditionRepository.Delete(item);
                }
            }

            //遍历整棵树，找到要删除的节点以及下级节点
            void FindDeleteNodeAndChildrenNodes(List<InterfaceCondition> sourceConditions, Guid currentNodeId)
            {
                var children = sourceConditions.Where(t => t.ParentId == currentNodeId).ToList();
                if (children != null && children.Any())
                {
                    foreach (var item in children)
                    {
                        willBeDeleteIds.Add(item.Id);
                        FindDeleteNodeAndChildrenNodes(children, item.Id);
                    }
                }
            }
        }

        public List<InterfaceCondition> GetTree(Guid interfaceConditionId)
        {
            //获取当前条件的所有子节点
            var nodes = _InterfaceConditionRepository.GetInterfaceConditionByBelongToId(interfaceConditionId);

            //获取顶级节点
            InterfaceCondition condition = nodes?.FirstOrDefault(t => t.ParentId == Guid.Empty);

            if (condition == null)
                return new List<InterfaceCondition>(0);

            //递归构造子节点
            condition.Children = GetTree(nodes, condition.Id);

            return new List<InterfaceCondition> { condition };

            //Tree Search
            List<InterfaceCondition> GetTree(List<InterfaceCondition> source, Guid parentId)
            {
                var childs = source.Where(t => t.ParentId == parentId).ToList();

                if (childs == null)
                    return new List<InterfaceCondition>(0);

                childs.ForEach(t => t.Children = GetTree(source, t.Id));

                return childs;
            }
        }
    }
}
