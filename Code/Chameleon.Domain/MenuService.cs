using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.Domain
{
    public interface IMenuService : ICommonServiceBase<Menu>
    {
        List<Menu> GetTree();
        Result AddNode(RelationEnum relation, Guid chooseNodeId, Menu entity);
        /// <summary>
        /// 调整位置
        /// </summary>
        /// <param name="relation"></param>
        /// <param name="chooseNodeId"></param>
        /// <param name="ajustNodeId"></param>
        /// <returns></returns>
        Result AjustRelation(RelationEnum relation, Guid chooseNodeId, Guid ajustNodeId);
        /// <summary>
        /// 获取有权限的所有菜单id
        /// </summary>
        /// <param name="userMenu"></param>
        /// <returns></returns>
        List<string> GetSubordinatMenus(Guid userMenu);
        /// <summary>
        /// 获取有权限的所有菜单id
        /// </summary>
        /// <param name="userMenus"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        List<string> GetSubordinatMenus(IEnumerable<Guid> userMenus, int level);
        /// <summary>
        /// 获取名称展示类似树状层级的列表（用于下拉列表）
        /// </summary>
        /// <returns></returns>
        List<Menu> GetTreeNameList();
    }

    public class MenuService : CommonServiceBase<Menu>, IMenuService
    {
        IMenuRepository _MenuRepository;
        public MenuService(IMenuRepository MenuRepository) : base(MenuRepository)
        {
            _MenuRepository = MenuRepository;
        }

        public List<Menu> GetTreeNameList()
        {
            // 获取所有子节点
            var nodes = _MenuRepository.GetListUnDeleted();

            //获取顶级节点
            Menu condition = nodes?.FirstOrDefault(t => t.ParentId == Guid.Empty);

            if (condition == null)
                return new List<Menu>(0);

            //返回值 返回值里面不包括顶级节点
            var result = new List<Menu>() { };

            //深度
            int dep = 0;

            //递归构造子节点
            GetTree(nodes, condition.Id, string.Empty);

            return result;

            //Tree Search
            void GetTree(List<Menu> source, Guid parentId, string parentName)
            {
                dep++;

                var childs = source.Where(t => t.ParentId == parentId).ToList();

                if (childs == null)
                    return;

                foreach (var item in childs)
                {
                    if (!string.IsNullOrEmpty(parentName))
                        item.Name = string.Concat(parentName, " - ", item.Name);

                    result.Add(item);

                    GetTree(source, item.Id, item.Name);
                }

                return;
            }
        }

        public List<Menu> GetTree()
        {
            //获取所有子节点
            var nodes = _MenuRepository.GetListUnDeleted();

            //获取顶级节点
            Menu condition = nodes?.FirstOrDefault(t => t.ParentId == Guid.Empty);

            if (condition == null)
                return new List<Menu>(0);

            //递归构造子节点
            condition.Children = GetTree(nodes, condition.Id);

            return new List<Menu> { condition };

            //Tree Search
            List<Menu> GetTree(List<Menu> source, Guid parentId)
            {
                var childs = source.Where(t => t.ParentId == parentId).ToList();

                if (childs == null)
                    return new List<Menu>(0);

                childs.ForEach(t => t.Children = GetTree(source, t.Id));

                return childs;
            }
        }

        public Result AddNode(RelationEnum relation, Guid chooseNodeId, Menu entity)
        {
            return TransactionHelper.Transaction(() =>
            {
                Menu newNode = new Menu
                {
                    Id = Guid.NewGuid(),
                    ParentId = chooseNodeId,
                    Name = entity.Name,
                    CreateBy = entity.CreateBy,
                    CreateTime = DateTime.Now,
                    ModifyBy = entity.CreateBy,
                    ModifyTime = DateTime.Now,
                    Route = entity.Route,
                    Icon = entity.Icon
                };

                //如果兄弟节点!=空，说明当前树有值。反之，则构建新树
                if (chooseNodeId != Guid.Empty)
                {
                    //判断是否有树存在
                    List<Menu> conditionListExist = _MenuRepository.GetListUnDeleted();

                    //获取选择的节点
                    Menu chooseNode = conditionListExist.FirstOrDefault(t => t.Id == chooseNodeId);

                    //根据选择的方式处理新节点
                    switch (relation)
                    {
                        case RelationEnum.Parent:
                            newNode.ParentId = chooseNode.ParentId;
                            chooseNode.ParentId = newNode.Id;
                            _MenuRepository.Update(chooseNode);
                            break;
                        case RelationEnum.Child:
                            newNode.ParentId = chooseNode.Id;
                            break;
                        default:
                            return Result.Error("关系错误，无法新增菜单");
                    }
                }

                newNode.Code = newNode.Id.ToString();

                _MenuRepository.Add(newNode);

                return Result.Success("保存成功！");
            });
        }

        public List<string> GetSubordinatMenus(Guid userMenu)
        {
            return GetSubordinatMenus(new Guid[] { userMenu }, int.MaxValue);
        }

        public Result AjustRelation(RelationEnum relation, Guid chooseNodeId, Guid ajustNodeId)
        {
            if (chooseNodeId == Guid.Empty || ajustNodeId == Guid.Empty)
                return Result.Success();

            if (chooseNodeId == ajustNodeId)
                return Result.Error("关系错误，无法调整菜单");

            //判断是否有树存在
            List<Menu> conditionListExist = _MenuRepository.GetListUnDeleted();

            if (conditionListExist == null || !conditionListExist.Any())
                return Result.Success();

            if (!conditionListExist.Any(t => t.Id == chooseNodeId) || !conditionListExist.Any(t => t.Id == ajustNodeId))
                return Result.Success("待调整节点未找到，无法继续");

            return TransactionHelper.Transaction(() =>
            {
                //获取选择的节点
                Menu chooseNode = conditionListExist.Find(t => t.Id == chooseNodeId);
                //待调整节点
                Menu ajustNode = conditionListExist.Find(t => t.Id == ajustNodeId);

                //根据选择的方式处理新节点
                switch (relation)
                {
                    case RelationEnum.Parent:
                        if (chooseNode.ParentId == Guid.Empty)
                            return Result.Error("无法调整为根菜单的上级菜单");

                        ajustNode.ParentId = chooseNode.ParentId;
                        chooseNode.ParentId = ajustNode.Id;
                        _MenuRepository.Update(chooseNode);
                        break;
                    case RelationEnum.Child:
                        if (chooseNode.ParentId == ajustNode.Id)
                            return Result.Error("关系错误，无法调整菜单");
                        ajustNode.ParentId = chooseNode.Id;
                        break;
                    default:
                        return Result.Error("关系错误，无法调整菜单");
                }

                _MenuRepository.Update(ajustNode);

                return Result.Success("保存成功！");
            });
        }
        public List<string> GetSubordinatMenus(IEnumerable<Guid> userMenus, int level)
        {
            if (userMenus == null || !userMenus.Any())
                return new List<string> { Guid.Empty.ToString() };

            //结果
            var result = new List<string>();

            //获取所有节点
            var nodes = _MenuRepository.GetListUnDeleted();

            if (nodes == null || !nodes.Any())
                return result;

            result.Add(Guid.Empty.ToString());//默认菜单，任何人都有此菜单权限

            foreach (var item in userMenus ?? Enumerable.Empty<Guid>())
            {
                result.Add(item.ToString());
                result.AddRange(GetTree(nodes, item, level));
            }

            return result.Distinct().ToList();

            List<string> GetTree(List<Menu> source, Guid parentId, int innerLevel)
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
