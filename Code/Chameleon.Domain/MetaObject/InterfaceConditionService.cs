using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
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
        /// <summary>
        /// 通过条件构造查询filter
        /// </summary>
        /// <param name="conditionId"></param>
        /// <param name="conditionUpperKeyDic"></param>
        /// <param name="isIgnoreArgumentsCheck">是否忽略参数检查</param>
        /// <returns></returns>
        FilterDefinition<BsonDocument> GetFilterDefinitionByCondition(Guid conditionId, Dictionary<string, string> conditionUpperKeyDic, bool isIgnoreArgumentsCheck = false);
    }

    public class InterfaceConditionService : MetaObjectCommonServiceBase<InterfaceCondition>, IInterfaceConditionService
    {
        IInterfaceConditionRepository _InterfaceConditionRepository;
        IMetaFieldRepository _metaFieldRepository;
        IMetaFieldService _metaFieldService;
        public InterfaceConditionService(IMetaFieldService metaFieldService, IInterfaceConditionRepository repository, IMetaFieldRepository metaFieldRepository) : base(repository)
        {
            _metaFieldService = metaFieldService;
            _metaFieldRepository = metaFieldRepository;
            _InterfaceConditionRepository = repository;
        }

        public Result AddTopInterfaceCondition(InterfaceCondition entity)
        {
            entity.BelongToCondition = Guid.Empty;
            entity.ParentId = Guid.Empty;
            entity.MetaFieldId = Guid.Empty;

            return base.AddCheckCode(entity);
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

        public FilterDefinition<BsonDocument> GetFilterDefinitionByCondition(Guid conditionId, Dictionary<string, string> conditionUpperKeyDic, bool isIgnoreArgumentsCheck = false)
        {
            List<InterfaceCondition> conditions = _InterfaceConditionRepository.GetInterfaceConditionByBelongToId(conditionId);

            var bf = Builders<BsonDocument>.Filter;

            if (conditions == null || !conditions.Any())
                return bf.Empty;

            //全部字段字典缓存
            var metaFieldUpperShortCodeKeyDic = _metaFieldRepository.GetMetaFieldShortCodeUpperDicByMetaObjectId(conditions.First().MetaObjectId);

            InterfaceCondition condition = conditions.FirstOrDefault(t => t.ParentId == Guid.Empty);

            if (condition == null)
                return bf.Empty;

            //如果连接条件
            if (condition.GetConditionNodeType() == NodeTypeEnum.Joint)
            {
                //通过链接条件解析器进行解析
                return ConditionRouter(condition);
            }
            //语句
            else
            {
                //通过条件表达式语句解析器解析
                return ConditionValue(condition);
            }

            //连接条件解析器。如果是连接条件， 则执行下面逻辑将左...右子条件解析
            FilterDefinition<BsonDocument> ConditionRouter(InterfaceCondition routeCondition)
            {
                FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Empty;
                //将子节点全部取出
                var routeConditionChildren = conditions.Where(t => t.ParentId == routeCondition.Id).ToList();
                var first = routeConditionChildren.FirstOrDefault();
                if (first != null)
                {
                    //如果字节点是连接条件
                    if (first.GetConditionNodeType() == NodeTypeEnum.Joint)
                    {
                        filterDefinition = ConditionRouter(first);
                    }
                    //如果是语句
                    else
                    {
                        filterDefinition = ConditionValue(first);
                        //根据根节点的连接条件执行不同的连接操作
                        switch (routeCondition.GetConditionJointType())
                        {
                            case ConditionJointTypeEnum.And:
                                //子节点全部是与逻辑
                                foreach (var item in routeConditionChildren.Except(new[] { first }))
                                {
                                    //如果是连接条件
                                    if (item.GetConditionNodeType() == NodeTypeEnum.Joint)
                                    {
                                        var tempCondition = ConditionRouter(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.And(filterDefinition, tempCondition);
                                        }
                                    }
                                    //如果是表达式语句
                                    else
                                    {
                                        var tempCondition = ConditionValue(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.And(filterDefinition, tempCondition);
                                        }
                                    }
                                }
                                break;
                            case ConditionJointTypeEnum.Or:
                                //子节点全部是或逻辑
                                foreach (var item in routeConditionChildren.Except(new[] { first }))
                                {
                                    //如果是连接条件
                                    if (item.GetConditionNodeType() == NodeTypeEnum.Joint)
                                    {
                                        var tempCondition = ConditionRouter(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.Or(filterDefinition, tempCondition);
                                        }
                                    }
                                    //如果是表达式语句
                                    else
                                    {
                                        var tempCondition = ConditionValue(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.Or(filterDefinition, tempCondition);
                                        }
                                    }
                                }
                                break;
                            default:
                                return bf.Empty;
                        }
                    }
                    return filterDefinition;
                }
                return bf.Empty;
            }

            //条件值解析器
            FilterDefinition<BsonDocument> ConditionValue(InterfaceCondition routeCondition)
            {
                //如果条件值来自参数,则从参数列表里面获取
                if (routeCondition.ConditionValue.Equals("?"))
                {
                    //从参数获取到值
                    string key = routeCondition.MetaFieldShortCode;
                    var keyUpper = key.ToUpperInvariant();
                    //如果没有传递参数值，则抛出异常
                    if (!conditionUpperKeyDic.ContainsKey(keyUpper))
                    {
                        //如果忽略参数检查，则直接返回null
                        if (isIgnoreArgumentsCheck)
                            return Builders<BsonDocument>.Filter.Empty;
                        //如果不忽略参数检查，则抛出异常
                        else
                            throw new ArgumentNullException(key, $"Conditions define field parameters [{key}] but do not provide values.");
                    }

                    var arguemntValue = conditionUpperKeyDic.SafeGet(keyUpper);

                    //将值转化为字段同类型的类型值
                    var metaField = metaFieldUpperShortCodeKeyDic[routeCondition.MetaFieldShortCode.ToUpperInvariant()];

                    var convertResult = _metaFieldService.CheckAndGetFieldValueByFieldType(metaField, arguemntValue);

                    if (!convertResult.IsSuccess)
                        throw new InvalidCastException($"Condition parameters data type of field [{metaField.ShortCode}] invalid. field define is [{metaField.GetFieldType().GetDescription()}], but value is [{arguemntValue}]");

                    object value = convertResult.Data;

                    switch (routeCondition.GetConditionType())
                    {
                        case ConditionTypeEnum.Equal:
                            return bf.Eq(key, value);
                        case ConditionTypeEnum.GreaterThan:
                            return bf.Gt(key, value);
                        case ConditionTypeEnum.GreaterThanEqual:
                            return bf.Gte(key, value);
                        case ConditionTypeEnum.LessThan:
                            return bf.Lt(key, value);
                        case ConditionTypeEnum.LessThanEqual:
                            return bf.Lte(key, value);
                        case ConditionTypeEnum.NotEqual:
                            return bf.Ne(key, value);
                        default:
                            return Builders<BsonDocument>.Filter.Empty;
                    }
                }
                //如果来自配置，则直接从配置里面获取到值
                else
                {
                    //校验字段以及转换字段值为目标类型
                    var convertResult = _metaFieldService.CheckAndGetFieldValueByFieldType(metaFieldUpperShortCodeKeyDic[routeCondition.MetaFieldShortCode.ToUpperInvariant()], routeCondition.ConditionValue);
                    if (!convertResult.IsSuccess)
                    {
                        throw new ArgumentException("配置的字段值不符合字段的类型");
                    }

                    switch (routeCondition.GetConditionType())
                    {
                        case ConditionTypeEnum.Equal:
                            return bf.Eq(routeCondition.MetaFieldShortCode, convertResult.Data);
                        case ConditionTypeEnum.GreaterThan:
                            return bf.Gt(routeCondition.MetaFieldShortCode, convertResult.Data);
                        case ConditionTypeEnum.GreaterThanEqual:
                            return bf.Gte(routeCondition.MetaFieldShortCode, convertResult.Data);
                        case ConditionTypeEnum.LessThan:
                            return bf.Lt(routeCondition.MetaFieldShortCode, convertResult.Data);
                        case ConditionTypeEnum.LessThanEqual:
                            return bf.Lte(routeCondition.MetaFieldShortCode, convertResult.Data);
                        case ConditionTypeEnum.NotEqual:
                            return bf.Ne(routeCondition.MetaFieldShortCode, convertResult.Data);
                        default:
                            return Builders<BsonDocument>.Filter.Empty;
                    }
                }
            }
        }
    }
}
