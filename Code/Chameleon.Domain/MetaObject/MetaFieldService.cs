using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Domain
{
    public interface IMetaFieldService : IMetaObjectCommonServiceBase<MetaField>
    {
        /// <summary>
        /// 预置系统字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="metaObjectCode"></param>
        /// <param name="applicationId"></param>
        void PresetSystemFields(Guid metaObjectId, string metaObjectCode, Guid applicationId);
    }

    public class MetaFieldService : MetaObjectCommonServiceBase<MetaField>, IMetaFieldService
    {
        public MetaFieldService(IMetaFieldRepository repository) : base(repository)
        {
        }

        public void PresetSystemFields(Guid metaObjectId, string metaObjectCode, Guid applicationId)
        {
            Ensure.ArgumentNotNullOrEmpty(metaObjectId, nameof(metaObjectId));
            Ensure.ArgumentNotNullOrEmpty(metaObjectCode, nameof(metaObjectCode));
            Ensure.ArgumentNotNullOrEmpty(applicationId, nameof(applicationId));

            var systemFields = new List<MetaField> {
                new MetaField{
                    ShortCode ="_id",
                    Name ="数据ID",
                    FieldType=(int)DataType.Text,
                },
                new MetaField{
                    ShortCode ="IsDeleted",
                    Name ="是否删除",
                    FieldType=(int)DataType.Boolean,
                },
                new MetaField{
                    ShortCode ="CreateBy",
                    Name ="创建人",
                    FieldType= (int)DataType.Int32,
                },
                new MetaField{
                    ShortCode ="CreateTime",
                    Name ="创建时间",
                    FieldType= (int)DataType.DateTime,
                },
                new MetaField{
                    ShortCode ="ModifyBy",
                    Name ="修改人",
                    FieldType= (int)DataType.Int32,
                },
                new MetaField{
                    ShortCode ="ModifyTime",
                    Name ="修改时间",
                    FieldType= (int)DataType.DateTime,
                }
            };

            systemFields.ForEach(item =>
            {
                item.Id = Guid.NewGuid();
                item.CloudApplicationtId = applicationId;
                item.MetaObjectId = metaObjectId;
                item.Code = string.Concat(metaObjectCode, ".", item.ShortCode);
                item.IsSystem = 1;
                item.Group = "系统";
                item.CreateBy = item.ModifyBy = 0;
                item.CreateTime = item.ModifyTime = DateTime.Now;
                item.SortNumber = -1;
                item.Description = "系统字段";
            });

            base._commonRepositoryBase.BatchAdd(systemFields);
        }
    }
}
